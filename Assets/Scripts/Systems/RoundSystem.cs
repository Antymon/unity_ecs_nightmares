using DG.Tweening;
using Entitas;
using System.Collections.Generic;

public class RoundSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;

    private Tween roundTimer;

    private GameContext gameContext;

    private LevelComponent levelComponent;
    private ScoreComponent scoreComponent;

    public RoundSystem(GameContext context, IEntityDeserializer deserializer)
        : base(context)
    {
        this.entityDeserializer = deserializer;
        this.gameContext = context;

        //ToDo: should be soft-configurable
        context.SetLevel(
            newNumberRounds: 5, 
            newEffectsAtTimeCap: 4, 
            newCurrentRound: 0, 
            newRoundTime: 180, 
            newRoundScoreReward: 1);

        context.SetScore(0);

        scoreComponent = context.score;
        levelComponent = context.level;
    }

    public void Initialize()
    {
        gameContext.GetGroup(GameMatcher.GameStart).OnEntityAdded += OnGameStart;
        gameContext.CreateEntity().isGameStart = true;
    }

    private void OnGameStart(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        entity.Destroy();
        scoreComponent.currentScore = 0;
        levelComponent.currentRound = 0;
        StartNextRound();
    }

    private void StartNextRound()
    {
        levelComponent.currentRound++;

        roundTimer = DOVirtual.DelayedCall(levelComponent.roundTime, FinishRound);
        roundTimer.Play();
    }

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
            else if (agent.hasEnemy)
            {
                enemyIsDead = true;
            }

            agent.Destroy();
            entity.Destroy();
        }

        if (playerIsDead)
        {
            OnPlayerLost();
        }
        else if (enemyIsDead)
        {
            OnPlayerWon();
        }
    }

    private void OnPlayerWon()
    {
        scoreComponent.currentScore += levelComponent.roundScoreReward;
        FinishRound();
    }

    private void OnPlayerLost()
    {
        FinishRound();
    }

    private void FinishRound()
    {
        roundTimer.Kill();

        if (levelComponent.currentRound < levelComponent.numberRounds)
        {
            StartNextRound();
        }
        else
        {
            gameContext.CreateEntity().isGameOver = true;
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
}