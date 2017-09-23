
using Entitas;

public class BackgroundSystem : ReactiveSystem<BackgroundEntity>, IInitializeSystem
{
    private IEntityDeserializer entityDeserializer;

    public BackgroundSystem(IEntityDeserializer deserializer):base(Contexts.sharedInstance.background)
    {
        this.entityDeserializer = deserializer;
    }

    protected override void Execute(System.Collections.Generic.List<BackgroundEntity> entities)
    {
        foreach(var entity in entities)
        {
            var otherEntity = entity.trigger.other;

            if(otherEntity is BallEntity)
            {
                ((BallEntity)otherEntity).isBallOut = true;
            }

            entity.RemoveTrigger();
        }
    }


    protected override bool Filter(BackgroundEntity entity)
    {
        return true;
    }

    protected override ICollector<BackgroundEntity> GetTrigger(IContext<BackgroundEntity> context)
    {
        return context.CreateCollector<BackgroundEntity>(BackgroundMatcher.Trigger.Added());
    }

    public void Initialize()
    {
        var background = Contexts.sharedInstance.background.CreateEntity();

        entityDeserializer.DeserializeEnitity(background);
    }
}

