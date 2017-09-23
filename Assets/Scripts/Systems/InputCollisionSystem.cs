using Entitas;

public class InputCollisionSystem : ReactiveSystem<InputEntity>
{
    public InputCollisionSystem() : base(Contexts.sharedInstance.input)
    {

    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        foreach(var entity in entities)
        {
            var sourceEntity = entity.collision.self;

            if (sourceEntity is PadEntity)
            {
                ((PadEntity)sourceEntity).ReplaceCollision(entity.collision.collision,entity.collision.velocity,sourceEntity,entity.collision.other);
            }
            else if(sourceEntity is BlockEntity)
            {
                ((BlockEntity)sourceEntity).ReplaceCollision(entity.collision.collision, entity.collision.velocity, sourceEntity, entity.collision.other);
            }

            entity.Destroy();
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

