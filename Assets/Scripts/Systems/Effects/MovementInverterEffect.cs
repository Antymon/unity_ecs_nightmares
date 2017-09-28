using Entitas;
using UnityEngine;

public class MovementInverterEffect : IEffect, IMovementDestinationChangedListener, IMovementDirectionChangedListener
{
    public ulong lastingTicks; //how long will the effect last

    private bool canApply = true; //one time toggle
    private bool applied = false; //helps with tracking if effect was eventually unapplied

    private IMovementDestinationChangedListener interceptedDestinationChangedListener;
    private IMovementDirectionChangedListener interceptedDirectionChangedListener;

    private GameEntity targetEntity; //effect will affect oponent rather than collector entity

    private ulong currentTick = 0;
    private ulong applicationTick = 0;

    public bool Apply(GameEntity entity)
    {
        if(!CanApply(entity))
        {
            return false;
        }

        targetEntity = entity.agent.target;

        if (targetEntity.hasMovementDestinationChangedListener)
        {
            interceptedDestinationChangedListener = targetEntity.movementDestinationChangedListener.listener;
            targetEntity.movementDestinationChangedListener.listener = this;
        }
        else if (targetEntity.hasMovementDirectionChangedListener)
        {
            interceptedDirectionChangedListener = targetEntity.movementDirectionChangedListener.listener;
            targetEntity.movementDirectionChangedListener.listener = this;
        }

        canApply = false;
        applied = true;

        applicationTick = currentTick;

        return true;
    }

    private void Unapply()
    {
        applied = false;
        if (targetEntity.hasMovementDestinationChangedListener)
        {
            targetEntity.movementDestinationChangedListener.listener = interceptedDestinationChangedListener;
        }
        else if (targetEntity.hasMovementDirectionChangedListener)
        {
            targetEntity.movementDirectionChangedListener.listener = interceptedDirectionChangedListener;
        }
    }

    public bool IsUsed()
    {
        return !canApply && currentTick-applicationTick >= lastingTicks;
    }

    private bool CanApply(GameEntity entity)
    {
        return canApply && IsApplicable(entity);
    }

    
    public bool IsApplicable(GameEntity entity)
    {        
        var target = entity.agent.target;

        return target.isEnabled && //opponent has to be alive
            (target.hasMovementDestinationChangedListener || target.hasMovementDirectionChangedListener);
    }

    public void Update(ulong tick)
    {
        currentTick = tick;

        if(applied && IsUsed())
        {
            Unapply();
        }
    }

    public void OnMovementDestinationChanged(Vector3 destination)
    {
        interceptedDestinationChangedListener.OnMovementDestinationChanged(InvertXZVector(destination));
    }

    public void OnOrientationDestinationChanged(Vector3 destination)
    {
        interceptedDestinationChangedListener.OnOrientationDestinationChanged(InvertXZVector(destination));
    }

    private Vector3 InvertXZVector(Vector3 destination)
    {
        return new Vector3(destination.z, destination.y, destination.x);
    }

    public void OnMovementDirectionChanged(Vector3 direction)
    {
        interceptedDirectionChangedListener.OnMovementDirectionChanged(InvertXYVector(direction));
    }

    public void OnOrientationChanged(Vector3 direction)
    {
        interceptedDirectionChangedListener.OnOrientationChanged(InvertXYVector(direction));
    }

    private Vector3 InvertXYVector(Vector3 destination)
    {
        return new Vector3(destination.y, destination.x, destination.z);
    }


    public bool IsCollectible()
    {
        return true;
    }

    public bool IsExclusive()
    {
        return true;
    }
}

