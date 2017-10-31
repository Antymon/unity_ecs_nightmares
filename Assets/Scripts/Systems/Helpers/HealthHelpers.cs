using System;
using Entitas;

public class HealthHelpers
{
    public static float GetNormalizedHealth(HealthComponent health)
    {
        return (float)health.healthPoints / health.healthPointsCap;
    }

    public static void AddHealth(HealthComponent health, int healthPoints)
    {
        health.healthPoints = Math.Max(Math.Min(health.healthPoints + healthPoints, health.healthPointsCap),0);
    }
}

