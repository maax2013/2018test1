using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
	public event System.Action<Vector2Int> OnMove;

	Unit[,] unitsTable;
	Unit cueUnit;
	float offX;
	float offY;
	Vector2Int lastTileCoord;
	Vector2Int nextTileCoord;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void init (Unit[,] table)
	{
		unitsTable = table;
		Transform board = table [0, 0].transform.parent;
		offX = board.position.x;
		offY = board.position.y;
	}

	public void readyDrag (Unit u)
	{
		cueUnit = u;
		lastTileCoord = new Vector2Int (u.CurrentRow, u.CurrentColumn);
	}

	public void dragMove (Vector3 pos)
	{
		nextTileCoord = convertPosToTableCoor (pos);
//		print (nextTileCoord - lastTileCoord);
		Vector2Int distance = nextTileCoord - lastTileCoord;
		if (Mathf.Abs (distance.x) > 0 || Mathf.Abs (distance.y) > 0) {
			if (OnMove != null) {
				OnMove (new Vector2Int (distance.y, distance.x));
			}
		}
		lastTileCoord = nextTileCoord;
		cueUnit.setUnitCoord (nextTileCoord.y, nextTileCoord.x);
//		cueUnit.CurrentRow = nextTileCoord.x;
//		cueUnit.CurrentColumn = nextTileCoord.y;
	}

	Vector2Int convertPosToTableCoor (Vector3 pos)
	{
		float tempX = pos.x - offX;
		float tempY = pos.y - offY;
		int tempColumn = Mathf.RoundToInt (tempX);
		int tempRow = Mathf.RoundToInt (tempY);
		Vector2Int tempCoord = new Vector2Int (tempRow, tempColumn);
		return tempCoord;
	}
	
	//	// Update is called once per frame
	//	void Update ()
	//	{
	//		if (Input.GetKeyDown (KeyCode.S)) {
	//			if (OnMove != null) {
	//				OnMove (new Vector2Int (0, -1));
	//			}
	//		}
	//		if (Input.GetKeyDown (KeyCode.W)) {
	//			if (OnMove != null) {
	//				OnMove (new Vector2Int (0, 1));
	//			}
	//		}
	//		if (Input.GetKeyDown (KeyCode.A)) {
	//			if (OnMove != null) {
	//				OnMove (new Vector2Int (-1, 0));
	//			}
	//		}
	//		if (Input.GetKeyDown (KeyCode.D)) {
	//			if (OnMove != null) {
	//				OnMove (new Vector2Int (1, 0));
	//			}
	//		}
	//
	//		if (Input.GetKeyDown (KeyCode.Q)) {
	//			if (OnMove != null) {
	//				OnMove (new Vector2Int (-1, 1));
	//			}
	//		}
	//		if (Input.GetKeyDown (KeyCode.E)) {
	//			if (OnMove != null) {
	//				OnMove (new Vector2Int (1, 1));
	//			}
	//		}
	//		if (Input.GetKeyDown (KeyCode.Z)) {
	//			if (OnMove != null) {
	//				OnMove (new Vector2Int (-1, -1));
	//			}
	//		}
	//		if (Input.GetKeyDown (KeyCode.C)) {
	//			if (OnMove != null) {
	//				OnMove (new Vector2Int (1, -1));
	//			}
	//		}
	//	}
}
