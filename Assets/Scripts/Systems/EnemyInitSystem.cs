using Entitas;
using System.Collections.Generic;

public class EnemyInitSystem  : IInitializeSystem
{
    private GameContext gameContext;
    private InputContext inputContext;
    private IEntityDeserializer entityDeserializer;


    public EnemyInitSystem(GameContext gameContext, InputContext inputContext, IEntityDeserializer entityDeserializer)
    {
        this.gameContext = gameContext;
        this.inputContext = inputContext;
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {
        var entity = gameContext.CreateEntity();
        entity.AddEntityBinding(EntityPrefabNameBinding.ENEMY_BINDING);
        
        var playerGroup = gameContext.GetGroup(GameMatcher.Player);
        var playerEntity = playerGroup.GetSingleEntity();

        entity.AddAgent(0, string.Empty, new List<IEffect>(), playerEntity);
        entity.isEnemy = true;

        //ToDo: temp hack, to avoid initialization isssues
        playerEntity.agent.target = entity;

        entityDeserializer.DeserializeEnitity(entity);

        entity.AddPositionChanged(inputContext.tick.currentTick, 0, false, entity.position.position);
    }

}

