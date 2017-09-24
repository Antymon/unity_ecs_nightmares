using UnityEngine;
using Entitas;


public class TouchInputBehaviour : MonoBehaviour
{
    public void Update()
    {
        if(Application.isEditor)
        {
            if(Input.GetMouseButtonDown(0))
            {
                NotifySimulatedTouch(TouchPhase.Began);
            }
            else if (Input.GetMouseButton(0))
            {
                NotifySimulatedTouch(TouchPhase.Moved);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                NotifySimulatedTouch(TouchPhase.Ended);
            }
        }
        else
        {
            if(Input.touchCount>0)
            {
                NotifyTouches(Input.touches);
            }
        }
    }

    private void NotifySimulatedTouch(TouchPhase phase)
    {
        Touch touch = new Touch();
        touch.phase = phase;
        touch.position = Input.mousePosition;
        NotifyTouches(new Touch[] {touch});
    }

    private void NotifyTouches(Touch[] touches)
    {
        Contexts.sharedInstance.input.CreateEntity().AddTouches(touches);
    }
}

