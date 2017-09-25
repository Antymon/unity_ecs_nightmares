using Entitas;
using UnityEngine;

public class SystemsConfigBehaviour : AbstractGameControllerBehaviour
{
    //in general systems ordering is significant

    protected override void AddSystems(Contexts contexts, Systems systems)
    {
        systems.Add(new PlayerInitSystem(contexts.game, entityDeserializer));
        systems.Add(new JoypadSystem(contexts.input, contexts.game, entityDeserializer));
        systems.Add(new PlayerMovementSystem(contexts.game));
        systems.Add(new TriggerBulletSystem(contexts.input, contexts.game));
        systems.Add(new ShootingSystem(contexts.game));
        systems.Add(new CollisionSystem(contexts.input,contexts.game));
        systems.Add(new EnemyInitSystem(contexts.game, entityDeserializer));
        systems.Add(new DestroySystem(contexts.game));
        systems.Add(new RoundSystem(contexts.game, entityDeserializer));
    }
}

