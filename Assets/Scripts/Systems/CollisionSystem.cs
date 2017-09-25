using Entitas;
using System;
using UnityEngine;

public class CollisionSystem : ReactiveSystem<InputEntity>
{
    public CollisionSystem(InputContext context)
        : base(context)
    {
    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        GameEntity self;
        GameEntity other;

        foreach (var entity in entities)
        {
            self = entity.collision.self;
            other = entity.collision.other;

            AlterHealth(self, other);
            AlterHealth(other, self);

            entity.Destroy();
        }
    }

    private void AlterHealth(GameEntity self, GameEntity other)
    {
        if(!self.hasHealth || !other.hasDamage)
        {
            Debug.LogWarning("Objects in collision are missing expected components");
            return;
        }

        self.health.healthPoints -= other.damage.healthPointsDamaged;
        self.health.healthPoints = Math.Min(self.health.healthPoints, self.health.healthPointsCap);
        self.health.healthPoints = Math.Max(self.health.healthPoints, 0);

        if(self.hasHealthChangedListener)
        {
            self.healthChangedListener.listener.HealthChanged(self);
        }
        
        if(self.health.healthPoints == 0)
        {
            self.Destroy();
        }
    }

    protected override bool Filter(InputEntity entity)
    {
        return true;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector<InputEntity>(InputMatcher.Collision.Added());
    }
}

