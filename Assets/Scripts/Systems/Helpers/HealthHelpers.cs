using Entitas;

public class HealthHelpers
{
    public static float GetNormalizedHealth(HealthComponent health)
    {
        return (float)health.healthPoints / health.healthPointsCap;
    }
}

