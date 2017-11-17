using DG.Tweening;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private Tween roundTimer;

    private GameContext gameContext;

    private MatchComponent matchComponent;

    private AgentsFactory agentsFactory;
    
    private GameEntity player;
    private GameEntity enemy;
    private SignalEntityFactory signalFactory;

    public GameFlowSystem(GameContext context, InputContext inputContext, IEntityDeserializer deserializer)
        : base(context)
    {
        this.gameContext = context;

        gameContext.SetMatch(0, 0, 0, 0, null); //only way to create match entity
        gameContext.matchEntity.AddEntityBinding(EntityPrefabNameBinding.GAME_FLOW_CONFIG_BINDING);
        deserializer.DeserializeEnitity(gameContext.matchEntity);

        matchComponent = context.match;

        signalFactory = new SignalEntityFactory();

        agentsFactory = new AgentsFactory(context, inputContext, deserializer);
    }

    public void Initialize()
    {
        gameContext.GetGroup(GameMatcher.GameStart).OnEntityAdded += OnGameStart;
        gameContext.GetGroup(GameMatcher.GameRestart).OnEntityAdded += OnGameRestart;
        gameContext.GetGroup(GameMatcher.RoundRestart).OnEntityAdded += OnRoundRestart;

        StartGame();
    }

    private void OnRoundRestart(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        FinishRound();
        StartRound(gameContext.round.currentRound);
    }

    private void OnGameRestart(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        FinishRound();
        StartGame();
    }
    
    private void OnGameStart(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        entity.isGameStart = false;
        StartGame();
    }

    private void StartGame()
    {
        gameContext.ReplaceScores(new Dictionary<int, int>());
        gameContext.ReplaceRound(0);
        StartRound(gameContext.round.currentRound+1);
    }

    private void StartRound(int roundNumber)
    {
        signalFactory.Create().isRoundStarted = true;

        gameContext.ReplaceRound(roundNumber);

        roundTimer = DOVirtual.DelayedCall(matchComponent.roundTime, OnTimeOut);
        roundTimer.Play();

        agentsFactory.CreateAgents(out player, out enemy);
    }

    private void OnTimeOut()
    {
        FinishRound();
        ConsiderNextRound();
    }

    //ToDo: reactive system is bit noisy in this case readability-wise
    //this class should demonstrate logic flow within specifics of the domain
    //change to group
    protected override void Execute(List<GameEntity> entities)
    {
        bool playerIsDead = false;
        bool enemyIsDead = false;

        GameEntity agent;

        foreach (var entity in entities)
        {
            agent = entity.agentDead.agent;

            if (agent.isPlayer)
            {
                playerIsDead = true;
            }
            else if (agent.isEnemy)
            {
                enemyIsDead = true;
            }
            entity.Destroy();
        }

        if (playerIsDead)
        {
            OnAgentWon(enemy);
        }
        else if (enemyIsDead)
        {
            OnAgentWon(player);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.AgentDead.Added());
    }

    private void OnAgentWon(GameEntity agentEntity)
    {
        gameContext.scores.agentIdToScoreMapping[agentEntity.agent.id]+=matchComponent.roundScoreReward;
        
        FinishRound();
        ConsiderNextRound();
    }

    private void FinishRound()
    {
        signalFactory.Create().isRoundFinished = true;

        roundTimer.Kill();

        if (player.isEnabled)
        {
            player.Destroy();
        }

        if (enemy.isEnabled)
        {
            enemy.Destroy();
        }
    }

    private void ConsiderNextRound()
    {
        if (gameContext.round.currentRound < matchComponent.numberRounds)
        {
            StartRound(gameContext.round.currentRound+1);
        }
        else
        {
            signalFactory.Create().isGameOver = true;
        }
    }
}