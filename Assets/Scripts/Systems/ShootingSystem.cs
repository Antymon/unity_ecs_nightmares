using Entitas;
using System;
using System.Collections.Generic;

public class ShootingSystem : IInitializeSystem, IExecuteSystem
{
    private GameContext context;
    private IEntityDeserializer entityDeserializer;

    private IGroup<GameEntity> gunGroup;
    private List<GameEntity> gunEntities;

    public ShootingSystem(GameContext context, IEntityDeserializer entityDeserializer)
    {
        this.context = context;
        this.entityDeserializer = entityDeserializer;
    }

    public void Initialize()
    {
        gunGroup = context.GetGroup(GameMatcher.Gun);
        gunGroup.OnEntityAdded += OnGunsAdded;
        gunGroup.OnEntityRemoved += OnGunsRemoved;
        gunEntities = new List<GameEntity>(gunGroup.GetEntities());
    }

    private void OnGunsRemoved(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        gunEntities.Remove(entity);
    }

    private void OnGunsAdded(IGroup<GameEntity> group, GameEntity entity, int index, IComponent component)
    {
        gunEntities.Add(entity);
    }

    public void Execute()
    {
        GunComponent gun;
        foreach(var gunEntity in gunEntities)
        {
            gun = gunEntity.gun;
            gun.currentHeat = Math.Max(0, gun.currentHeat - 1);

            if(gun.triggerDown && gun.currentHeat==0)
            {
                gun.currentHeat = gun.cooldownTicks;

                var bullet = context.CreateEntity();
                bullet.isProjectile = true;
                gun.shootListener.OnShoot(bullet);
            }
        }
    }
}

