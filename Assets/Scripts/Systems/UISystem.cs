using Entitas;
using UnityEngine;

public class UISystem : IInitializeSystem, IExecuteSystem//, IReactiveSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    private GameEntity gameOverScreen;
    private GameEntity healthBarLeft;
    private GameEntity healthBarRight;

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

        agentGroup = context.GetGroup(GameMatcher.Agent);
    }

    //syncing health points and names on the health bars with actors data, could be optimized in the latter case
    private void UpdateHealthBarIfApplicable(HealthBarComponent healthBar, GameEntity agentEntity)
    {
        if (healthBar.agentId.Equals(agentEntity.agent.id))
        {
            float normalizedHealth = HealthHelpers.GetNormalizedHealth(agentEntity.health);
            healthBar.listener.OnHealthChanged(normalizedHealth*100);

            var name = agentEntity.agent.name;
            healthBar.listener.OnNameChanged(name);
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
        gameOverScreen.gameOverScreen.listener.OnShow();
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

