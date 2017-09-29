using Entitas;
using System.Collections.Generic;

public class AgentsFactory
{
    private GameContext gameContext;
    private InputContext inputContext;
    private IEntityDeserializer entityDeserializer;

    public AgentsFactory(GameContext gameContext, InputContext inputContext, IEntityDeserializer entityDeserializer)
    {
        this.gameContext = gameContext;
        this.inputContext = inputContext;
        this.entityDeserializer = entityDeserializer;
    }

    public void CreateAgents(out GameEntity player, out GameEntity enemy)
    {
        player = RequestActorCreation(player: true, target: null);
        enemy = RequestActorCreation(player: false, target: player);

        player.agent.target = enemy;

        //setting following flags will make them picked up synchronously
        //so they are set when everything is ready
        player.isPlayer = true;
        enemy.isEnemy = true;
    }

    private GameEntity RequestActorCreation(bool player, GameEntity target)
    {
        var binding = player ? EntityPrefabNameBinding.PLAYER_BINDING : EntityPrefabNameBinding.ENEMY_BINDING;
        var agentEntity = gameContext.CreateEntity();
        agentEntity.AddEntityBinding(binding);

        //agent placeholder values
        agentEntity.AddAgent(
            newId: 0,
            newName: string.Empty,
            newScore: 0,
            newEffects: new List<IEffect>(),
            newTarget: target
            );

        entityDeserializer.DeserializeEnitity(agentEntity);

        InitializeScoreForAgent(agentEntity);

        agentEntity.AddPositionChanged(inputContext.tick.currentTick, 0, false, agentEntity.position.position);

        return agentEntity;
    }

    private void InitializeScoreForAgent(GameEntity agentEntity)
    {
        var scores = gameContext.scores.agentIdToScoreMapping;
        var agentId = agentEntity.agent.id;

        if (!scores.ContainsKey(agentId))
        {
            scores[agentId] = 0;
        }
    }
}

