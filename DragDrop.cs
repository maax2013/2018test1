using UnityEngine;

public static class DragDrop
{
	public static event System.Action<Vector2Int> OnMove;

	//Unit[,] unitsTable;
	//Unit cueUnit;
    static float offX;
    static float offY;
    static Vector2Int lastTileCoord;
    static Vector2Int nextTileCoord;


    public static void ApplyOffset (Unit[,] table)
	{
		//unitsTable = table;
		Transform board = table [0, 0].transform.parent;
		offX = board.position.x;
		offY = board.position.y;
	}

    public static void ReadyForDrag (Unit u)
	{
		//cueUnit = u;
		lastTileCoord = new Vector2Int (u.CurrentColumn, u.CurrentRow);
	}

    public static void dragMove (Vector3 pos)
	{
		nextTileCoord = convertPosToTableCoord (pos);
//		print (nextTileCoord - lastTileCoord);
		Vector2Int distance = nextTileCoord - lastTileCoord;
		if (Mathf.Abs (distance.x) > 0 || Mathf.Abs (distance.y) > 0) {
            lastTileCoord = nextTileCoord;
			if (OnMove != null) {
				OnMove (new Vector2Int (distance.x, distance.y));
			}
		}
	}

    static Vector2Int convertPosToTableCoord (Vector3 pos)
	{
		float tempX = pos.x - offX;
		float tempY = pos.y - offY;
		int tempColumn = Mathf.RoundToInt (tempX);
		int tempRow = Mathf.RoundToInt (tempY);
		Vector2Int tempCoord = new Vector2Int (tempColumn, tempRow);
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
