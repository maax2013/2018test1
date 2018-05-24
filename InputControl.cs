using UnityEngine;
//using UnityEngine.Events;
using UnityEngine.EventSystems;

//using System.Collections;

public class InputControl : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	//	[SerializeField] Camera mainCam;
    public bool InputEnabled { get; set; }

    public event System.Action<GameObject, Vector3> onDragStart;
    public event System.Action<Vector3> onDragging;
    public event System.Action onDragEnd;

    //public delegate void MyTouchEvent(GameObject obj, Vector3 pos);

    //public event MyTouchEvent onDragStart;
    //public event MyTouchEvent onDragging;
    //public event MyTouchEvent onDragEnd;


    Bounds gameBoardBoundary;
    bool insideBoardBoundary;
    Vector3 pointerWorldPosition;

    public void passBoundary(Bounds b){
        gameBoardBoundary = b;
    }

	void Start ()
	{
		addPhysics2DRaycaster ();
	}

	void addPhysics2DRaycaster ()
	{
		Physics2DRaycaster physicsRaycaster = GameObject.FindObjectOfType<Physics2DRaycaster> ();
		if (physicsRaycaster == null) {
			Camera.main.gameObject.AddComponent<Physics2DRaycaster> ();
		}
	}

    public void OnPointerDown (PointerEventData eventData)
	{
//		Debug.Log ("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
	}

    public void OnBeginDrag (PointerEventData eventData)
	{
        if(InputEnabled){
            //Debug.Log ("Drag Begin");
            pointerWorldPosition = eventData.pointerCurrentRaycast.worldPosition;
            insideBoardBoundary = BoardUtilities.pointerInsideBoundary(pointerWorldPosition, gameBoardBoundary);
            if(insideBoardBoundary){
                if (onDragStart != null)
                {
                    onDragStart(eventData.pointerCurrentRaycast.gameObject, pointerWorldPosition);
                }
            }

        }

	}

	public void OnDrag (PointerEventData eventData)
	{
//		Debug.Log ("Dragging");
//		Debug.Log (eventData.pointerCurrentRaycast.worldPosition);
        pointerWorldPosition = eventData.pointerCurrentRaycast.worldPosition;
        insideBoardBoundary = BoardUtilities.pointerInsideBoundary(pointerWorldPosition, gameBoardBoundary);
        if (onDragging != null && InputEnabled && insideBoardBoundary) {
            onDragging (pointerWorldPosition);
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
//		Debug.Log ("Drag Ended");
        if (onDragEnd != null && InputEnabled) {
			onDragEnd ();
		}
	}

}

