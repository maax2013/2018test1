using UnityEngine;
using System.Collections;

public class InputControl : MonoBehaviour
{
	[SerializeField] Camera mainCam;
	public bool touchable = false;

	public event System.Action<GameObject> OnTouch;

	Ray ray;
	RaycastHit hit;

	// Update is called once per frame
	void Update ()
	{
		if (touchable) {
			// touch devices
			if (Input.touchSupported) { 
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) {
					ray = mainCam.ScreenPointToRay (Input.GetTouch (0).position);
					if (Physics.Raycast (ray, out hit)) {
						if (OnTouch != null) {
							OnTouch (hit.collider.gameObject);
						}
					}
				}
			} else {
				// mouse
				if (Input.GetMouseButtonDown (0)) {
//					Debug.Log (Input.mousePosition);
					ray = mainCam.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit)) {
//						Debug.Log ("hit");
						if (OnTouch != null) {
							OnTouch (hit.collider.gameObject);
						}
					}
				}			
			}
		}
	}
}

