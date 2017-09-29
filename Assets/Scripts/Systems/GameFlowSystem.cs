using DG.Tweening;
using Entitas;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;

    private Tween roundTimer;

    private GameContext gameContext;
    private InputContext inputContext;

    private LevelComponent levelComponent;

    private AgentsFactory agentsFactory;
    
    private GameEntity player;
    private GameEntity enemy;


    public GameFlowSystem(GameContext context, InputContext inputContext, IEntityDeserializer deserializer)
        : base(context)
    {
        this.entityDeserializer = deserializer;
        this.gameContext = context;
        this.inputContext = inputContext;

        //ToDo: deserialize from outside like the rest
        context.SetLevel(
            newNumberRounds: 5, 
            newEffectsAtTimeCap: 4, 
            newCurrentRound: 0, 
            newRoundTime: 180, 
            newRoundScoreReward: 1,
            newSeed: 0);

        //ToDo: unitys random is not portable (makes "ECall" into editor), replace
        Random.InitState(context.level.seed);

        levelComponent = context.level;

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
        StartRound(levelComponent.currentRound);
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
        levelComponent.currentRound = 0;
        StartRound(levelComponent.currentRound+1);
    }

    private void StartRound(int roundNumber)
    {
        gameContext.CreateEntity().isRoundStarted = true;

        levelComponent.currentRound=roundNumber;

        roundTimer = DOVirtual.DelayedCall(levelComponent.roundTime, OnTimeOut);
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
        gameContext.scores.agentIdToScoreMapping[agentEntity.agent.id]+=levelComponent.roundScoreReward;
        
        FinishRound();
        ConsiderNextRound();
    }

    private void FinishRound()
    {
        gameContext.CreateEntity().isRoundFinished = true;

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
        if (levelComponent.currentRound < levelComponent.numberRounds)
        {
            StartRound(levelComponent.currentRound+1);
        }
        else
        {
            gameContext.CreateEntity().isGameOver = true;
        }
    }
}