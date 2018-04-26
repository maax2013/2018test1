using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//using System.Collections;

public class InputControl : MonoBehaviour,  IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	//	[SerializeField] Camera mainCam;
	public bool inputEnabled = false;

	public event System.Action<GameObject> OnTouch;

	public delegate void MyTouchEvent (GameObject obj, Vector3 pos);

	public static event MyTouchEvent onDragStart;
	public static event MyTouchEvent onDrag;
	public static event MyTouchEvent onDragEnd;

	//	Ray ray;
	//	RaycastHit hit;

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
//		Debug.Log ("Drag Begin");
		if (onDragStart != null) {
			onDragStart (eventData.pointerCurrentRaycast.gameObject, eventData.pointerCurrentRaycast.worldPosition);
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
//		Debug.Log ("Dragging");
//		Debug.Log (eventData.pointerCurrentRaycast.worldPosition);
		if (onDrag != null) {
			onDrag (null, eventData.pointerCurrentRaycast.worldPosition);
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
//		Debug.Log ("Drag Ended");
		if (onDragEnd != null) {
			onDragEnd (null, eventData.pointerCurrentRaycast.worldPosition);
		}
	}

	//	// Update is called once per frame
	//	void Update ()
	//	{
	//		if (inputEnabled) {
	//			// touch devices
	//			if (Input.touchSupported) {
	//				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
	//					ray = mainCam.ScreenPointToRay (Input.GetTouch (0).position);
	//					if (Physics.Raycast (ray, out hit)) {
	//						if (OnTouch != null) {
	//							OnTouch (hit.collider.gameObject);
	//						}
	//					}
	//				}
	//			} else {
	//				// mouse
	//				if (Input.GetMouseButtonDown (0)) {
	////					Debug.Log (Input.mousePosition);
	//					ray = mainCam.ScreenPointToRay (Input.mousePosition);
	//					if (Physics.Raycast (ray, out hit)) {
	////						Debug.Log ("hit");
	//						if (OnTouch != null) {
	//							OnTouch (hit.collider.gameObject);
	//						}
	//					}
	//				}
	//			}
	//		}
	//	}
}

