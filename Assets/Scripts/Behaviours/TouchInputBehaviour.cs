using UnityEngine;
using Entitas;
using System.Linq;
using System.Collections.Generic;

public class TouchInputBehaviour : MonoBehaviour
{
    public KeyCode shootingKey = KeyCode.Space;

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
            var touches = new List<Touch>();

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

            /*
            if (Input.GetKeyDown(shootingKey))
            {
                touches.Add(FakeTouch(TouchPhase.Began, 2));
            }
            else if (Input.GetKey(shootingKey))
            {
                touches.Add(FakeTouch(TouchPhase.Stationary, 2));
            }
            else if (Input.GetKeyUp(shootingKey))
            {
                touches.Add(FakeTouch(TouchPhase.Ended, 2));
            }
             * */

            NotifyTouches(touches.ToArray());
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
        Contexts.sharedInstance.input.CreateEntity().AddTouches(touches);
    }
}

