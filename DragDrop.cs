using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
	public event System.Action<Vector2Int> OnMove;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (OnMove != null) {
				OnMove (new Vector2Int (0, 1));
			}
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (OnMove != null) {
				OnMove (new Vector2Int (0, -1));
			}
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			if (OnMove != null) {
				OnMove (new Vector2Int (-1, 0));
			}
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			if (OnMove != null) {
				OnMove (new Vector2Int (1, 0));
			}
		}

		
	}
}
