using Entitas;

public class AddHealthEffect : IEffect
{
    public int healthPoints;

    private bool used = false;

    public bool Apply(GameEntity entity)
    {
        if (IsUsed())
        {
            return false;
        }

        //ToDo: health points capping logic shouldn't be here
        entity.health.healthPoints = System.Math.Min(entity.health.healthPoints+healthPoints, entity.health.healthPointsCap);
        used = true;

        return true;
    }

    public bool IsUsed()
    {
        return used;
    }

    public bool IsApplicable(GameEntity entity)
    {
        return entity.hasHealth;
    }

    public void Update(ulong tick)
    {
    }


    public bool IsCollectible()
    {
        return true;
    }
}

