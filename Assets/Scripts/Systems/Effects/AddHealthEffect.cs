using Entitas;

public class AddHealthEffect : IEffect
{
    public int healthPoints;

    public void Apply(GameEntity entity)
    {
        entity.health.healthPoints=System.Math.Min(entity.health.healthPoints+healthPoints, entity.health.healthPointsCap);
    }

    public int GetRepeatInterval()
    {
        return 0;
    }

    public bool IsContintous()
    {
        return false;
    }
}

