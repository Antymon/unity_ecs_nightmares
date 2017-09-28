using Entitas;
using UnityEngine;

public class SystemsConfigBehaviour : AbstractGameControllerBehaviour
{
    //in general systems ordering is significant

    protected override void AddSystems(Contexts contexts, Systems systems)
    {
        var gameContext = contexts.game;
        var inputContext = contexts.input;

        systems.Add(new TickSystem(inputContext));
        systems.Add(new PlayerInitSystem(gameContext, inputContext, entityDeserializer));
        systems.Add(new JoypadSystem(inputContext, gameContext, entityDeserializer));
        systems.Add(new MovementSystem(gameContext, inputContext));
        systems.Add(new TriggerBulletSystem(inputContext, gameContext));
        systems.Add(new ShootingSystem(gameContext));
        systems.Add(new CollisionSystem(inputContext,gameContext));
        systems.Add(new EnemyInitSystem(gameContext, inputContext, entityDeserializer));
        systems.Add(new DestroySystem(gameContext));
        
        systems.Add(new EnemyAISystem(gameContext));
        systems.Add(new TriggerSystem(inputContext));
        systems.Add(new EffectSystem(gameContext, inputContext, entityDeserializer));
        systems.Add(new RoundSystem(gameContext, entityDeserializer));
        
    }
}

