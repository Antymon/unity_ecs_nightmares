using Entitas;
using UnityEngine;

public class UISystem : IInitializeSystem, IExecuteSystem//, IReactiveSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    private GameEntity gameOverScreen;
    private GameEntity healthBarLeft;
    private GameEntity healthBarRight;
    private GameEntity roundCounter;

    private IGroup<GameEntity> agentGroup;
    private ICollector<GameEntity> healthCollector;

    public UISystem(GameContext context, IEntityDeserializer entityDeserializer) //: base(context)
    {
        this.context = context;
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {
        context.GetGroup(GameMatcher.GameOver).OnEntityAdded += OnGameOver;

        gameOverScreen = CreateUIEntity(EntityPrefabNameBinding.GAME_OVER_SCREEN_BINDING);
        healthBarLeft = CreateUIEntity(EntityPrefabNameBinding.HEALTH_BAR_BINDING);
        healthBarRight = CreateUIEntity(EntityPrefabNameBinding.HEALTH_BAR_BINDING);
        
        //just binding, no calling into required
        CreateUIEntity(EntityPrefabNameBinding.PAUSE_SCREEN_BINDING);


        agentGroup = context.GetGroup(GameMatcher.Agent);

        context.GetGroup(GameMatcher.Round).OnEntityUpdated += OnRoundUpdated;
    }

    //recreating round counter entity on each value change, which is not that frequent
    //both entities and game objects are pooled so shouldn't be heavy at all
    private void OnRoundUpdated(IGroup<GameEntity> group, GameEntity entity, int index, IComponent previousComponent, IComponent newComponent)
    {
        if (roundCounter != null)
        {
            roundCounter.Destroy();
        }

        roundCounter=context.CreateEntity();
        roundCounter.AddRoundCounter(context.round.currentRound);
        roundCounter.AddEntityBinding(EntityPrefabNameBinding.ROUND_COUNTER_BINDING);
        entityDeserializer.DeserializeEnitity(roundCounter);
    }

    //syncing health points and names on the health bars with actors data, could be optimized in the latter case
    private void UpdateHealthBarIfApplicable(HealthBarComponent healthBar, GameEntity agentEntity)
    {
        if (healthBar.agentId.Equals(agentEntity.agent.id))
        {
            var normalizedHealth = HealthHelpers.GetNormalizedHealth(agentEntity.health);
            healthBar.listener.OnHealthChanged(normalizedHealth*100);

            var name = agentEntity.agent.name;
            healthBar.listener.OnNameChanged(name);

            var score = context.scores.agentIdToScoreMapping[agentEntity.agent.id];
            healthBar.listener.OnScoreChanged(score);
        }
    }

    private GameEntity CreateUIEntity(EntityPrefabNameBinding binding)
    {
        var entity = context.CreateEntity();
        entity.AddEntityBinding(binding);
        entityDeserializer.DeserializeEnitity(entity);
        return entity;
    }

    private void OnGameOver(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        //ToDo: put proper message on outcome
        gameOverScreen.gameOverScreen.listener.OnShow("Match Finished");
    }

    public void Execute()
    {
        foreach (var entity in agentGroup.GetEntities())
        {
            UpdateHealthBarIfApplicable(healthBarLeft.healthBar, entity);
            UpdateHealthBarIfApplicable(healthBarRight.healthBar, entity);          
        }
    }
}

