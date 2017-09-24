using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class NavigationCircleManager : MonoBehaviour 
{

    [HideInInspector]
    public Vector2 lastNavigationTouchPoint; 

	float outerRadius;
	bool dragging;

	//RectTransform navigationCircleTransform;
	RectTransform innerCircleTransform;

	int fingerId;

    public void Reset()
    {
        this.gameObject.SetActive (true);
        lastNavigationTouchPoint = Vector2.zero;
        dragging = false;
        UpdateShape();
    }

	// Use this for initialization
	void Start () 
	{
        Reset();
		innerCircleTransform = GameObject.Find("ShootingCircle").GetComponent<RectTransform>();
        RectTransform rectTransform = GetComponent<RectTransform>();
		outerRadius = rectTransform.rect.width / 2f;

        /*
		if (true == GameInitializer.isSmallDevice)
		{
			navigationCircleTransform = gameObject.GetComponent<RectTransform> ();
			Vector3 offset = Vector3.zero;
			offset.x = ( (navigationCircleTransform.rect.width * GameInitializer.sizeMultiplierForSmallDevices) - navigationCircleTransform.rect.width ) * 0.5f / navigationCircleTransform.localScale.x;
			offset.y = ( (navigationCircleTransform.rect.height * GameInitializer.sizeMultiplierForSmallDevices) - navigationCircleTransform.rect.height ) * 0.5f / navigationCircleTransform.localScale.y;
			navigationCircleTransform.position = navigationCircleTransform.position + offset;
			navigationCircleTransform.localScale = navigationCircleTransform.localScale * GameInitializer.sizeMultiplierForSmallDevices;
		}*/
	}

	void Update () 
	{
        if(Application.isEditor)
        {
		    if (Input.GetMouseButtonDown(0))
		    {
			    EvaluateDifferenceBetweenTouchPointAndCircleCenter(Input.mousePosition);
                dragging = (lastNavigationTouchPoint.magnitude < outerRadius);

                Debug.Log(lastNavigationTouchPoint.magnitude+" "+outerRadius);
		    }
		    else if (Input.GetMouseButton(0))
		    {
			    EvaluateDifferenceBetweenTouchPointAndCircleCenter(Input.mousePosition);
		    }
		    else if (Input.GetMouseButtonUp(0))
		    {
			    dragging = false;
		    }

		    UpdateShape();
        }
        else
        {
		    if (Input.touchCount > 0) //multitouch interaction
		    {
			    if (!dragging) {

                    //take closest touch point to joystick center
				    Vector2 touchPosition = Input.GetTouch(0).position;
				    int closest = 0;
				    for (int i = 1; i < Input.touchCount; i++)
				    {
					    if ((Input.GetTouch(i).position - new Vector2(transform.position.x, transform.position.y)).magnitude < (touchPosition - new Vector2(transform.position.x, transform.position.y)).magnitude)
					    {
						    touchPosition = Input.GetTouch(i).position;
						    closest = i;
					    }
				    }
				
				    EvaluateDifferenceBetweenTouchPointAndCircleCenter(touchPosition);
				
				    if (lastNavigationTouchPoint.magnitude < outerRadius)
				    {
					    dragging = true;
					    fingerId = Input.GetTouch(closest).fingerId;
				    }
			    }
			    else {
				
				    dragging = false;
				    for (int i = 0; i < Input.touchCount; i++)
				    {
					    if (Input.GetTouch(i).fingerId == fingerId)
					    {
						    EvaluateDifferenceBetweenTouchPointAndCircleCenter(Input.GetTouch(i).position);
						    dragging = true;
						    break;
					    }
				    }
			    }
			
		    }
		    else {
			    dragging = false;
		    }

            UpdateShape();
        }
	}

	public bool IsPointDraggedAndInsideOfACircle()
	{
        return dragging;
	}

	void EvaluateDifferenceBetweenTouchPointAndCircleCenter(Vector2 touchPoint)
	{
        lastNavigationTouchPoint = touchPoint - (Vector2) transform.position;
	}

    float GetAngleFromDirection(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
    }

    void UpdateShape()
    {
        GameObject.Find("ShootingCircleContainer").transform.rotation = Quaternion.AngleAxis(GetAngleFromDirection(lastNavigationTouchPoint), Vector3.forward);

		if(dragging) {
			Vector3 stickpos = GameObject.Find("ShootingCircle").transform.localPosition;
			stickpos.y = Mathf.Clamp(lastNavigationTouchPoint.magnitude, 0, outerRadius);
			GameObject.Find("ShootingCircle").transform.localPosition = stickpos;
		}
		else {
			GameObject.Find("ShootingCircle").transform.localPosition = Vector3.zero;
		}

    }
}
