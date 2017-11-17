using UnityEngine;
using Entitas;
using System.Linq;
using System.Collections.Generic;

public class TouchInputBehaviour : MonoBehaviour
{
    public KeyCode alternativeTouchKey = KeyCode.Space;

    List<Touch> touches = new List<Touch>();

    public void Update()
    {
        if(!Application.isEditor)
        {
            if (Input.touchCount > 0)
            {
                NotifyTouches(Input.touches);
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                touches.Add(FakeTouch(TouchPhase.Began,1));
            }
            else if (Input.GetMouseButton(0))
            {
                touches.Add(FakeTouch(TouchPhase.Moved, 1));
            }
            else if(Input.GetMouseButtonUp(0))
            {
                touches.Add(FakeTouch(TouchPhase.Ended, 1));
            }

            
            if (Input.GetKeyDown(alternativeTouchKey))
            {
                touches.Add(FakeTouch(TouchPhase.Began, 2));
            }
            else if (Input.GetKey(alternativeTouchKey))
            {
                touches.Add(FakeTouch(TouchPhase.Stationary, 2));
            }
            else if (Input.GetKeyUp(alternativeTouchKey))
            {
                touches.Add(FakeTouch(TouchPhase.Ended, 2));
            }

            if (touches.Count > 0)
            {
                NotifyTouches(touches.ToArray());
                touches.Clear();
            }

        }
    }

    private Touch FakeTouch(TouchPhase phase, int id)
    {
        Touch touch = new Touch();
        touch.phase = phase;
        touch.position = Input.mousePosition;
        touch.fingerId = id;

        return touch;
    }

    private void NotifyTouches(Touch[] touches)
    {
        if (!Contexts.sharedInstance.game.isPause)
        {
            Contexts.sharedInstance.input.CreateEntity().AddTouches(touches);
        }
    }
}

