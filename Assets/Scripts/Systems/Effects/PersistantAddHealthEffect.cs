
using Entitas;

public class PersistantAddHealthEffect : IEffect
{
    public int healthPoints;
    public int intervalTicks; //how many updates needed to reapply

    private int ticksSinceApply = 0;

    public void Apply(GameEntity entity)
    {
        if(!CanApply(entity))
        {
            return;
        }

        //ToDo: health points capping logic shouldn't be here
        entity.health.healthPoints = System.Math.Min(entity.health.healthPoints+healthPoints, entity.health.healthPointsCap);
        
        ticksSinceApply = 0;
    }

    public bool IsUsed()
    {
        return false;
    }

    public bool CanApply(GameEntity entity)
    {
        return ticksSinceApply>=intervalTicks;
    }

    public void Update()
    {
        ticksSinceApply++;
    }

    public bool IsCollectible()
    {
        return false;
    }
}

