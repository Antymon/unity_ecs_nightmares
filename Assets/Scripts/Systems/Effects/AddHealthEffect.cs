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
        
        HealthHelpers.AddHealth(entity.health, healthPoints);

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


    public bool IsExclusive()
    {
        return false;
    }
}

