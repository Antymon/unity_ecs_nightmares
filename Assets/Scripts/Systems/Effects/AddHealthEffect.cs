﻿using Entitas;

public class AddHealthEffect : IEffect
{
    public int healthPoints;

    private bool used = false;

    public void Apply(GameEntity entity)
    {
        if(!CanApply(entity))
        {
            return;
        }

        //ToDo: health points capping logic shouldn't be here
        entity.health.healthPoints = System.Math.Min(entity.health.healthPoints+healthPoints, entity.health.healthPointsCap);
        used = true;
    }

    public bool IsUsed()
    {
        return used;
    }

    public bool CanApply(GameEntity entity)
    {
        return !IsUsed();
    }

    public void Update()
    {
    }


    public bool IsCollectible()
    {
        return true;
    }
}

