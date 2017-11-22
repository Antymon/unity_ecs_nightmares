using Entitas;
using System.Collections.Generic;
using UnityEngine;

public interface ITouchDrivenStateManager
{
    void ManageState(List<InputEntity> entities);
    void Disable();
    bool IsEnabled();
}

//template method manager that enables or disables injected item based on:
//touch input
//api calls
public abstract class TouchDrivenStateManager : ITouchDrivenStateManager
{
    public void ManageState(List<InputEntity> touchEntities)
    {
        foreach (var entity in touchEntities)
        {
            int touchId = GetTouchId();

            var touches = entity.touches.touches;

            bool touchFound = false;

            if (IsEnabled())
            {
                foreach (var touch in touches)
                {
                    if (touch.fingerId == touchId)
                    {
                        touchFound = true;

                        OnTouchFound(touch);

                        break;
                    }
                }

                if (!touchFound)
                {
                    Disable();
                }
            }
            else //disabled
            {
                foreach (var touch in touches)
                {
                    if (ShouldEnable(touch))
                    {
                        Enable(touch);
                        break;
                    }
                }
            }
        }
    }

    public abstract void Disable();


    protected abstract void Enable(Touch touch);

    public abstract bool IsEnabled();

    protected abstract bool ShouldEnable(Touch touch);


    protected abstract int GetTouchId();

    protected abstract void OnTouchFound(Touch touch);
}
