using UnityEngine;

public static class DragMoveCoord
{
	public static event System.Action<Vector2Int> OnMove;

	//Unit[,] unitsTable;
	//Unit cueUnit;
    static Vector2Int lastTileCoord;
    static Vector2Int nextTileCoord;


    public static void RegisterDragStartCoord (int column, int row)
	{
		//cueUnit = u;
		lastTileCoord = new Vector2Int (column, row);
	}

    public static void dragMove (float posX, float posY)
	{
		nextTileCoord = convertPosToTableCoord (posX, posY);
//		print (nextTileCoord - lastTileCoord);
		Vector2Int distance = nextTileCoord - lastTileCoord;
		if (Mathf.Abs (distance.x) > 0 || Mathf.Abs (distance.y) > 0) {
            lastTileCoord = nextTileCoord;
			if (OnMove != null) {
				OnMove (new Vector2Int (distance.x, distance.y));
			}
		}
	}

    static Vector2Int convertPosToTableCoord (float tempX, float tempY)
	{
		int tempColumn = Mathf.RoundToInt (tempX);
		int tempRow = Mathf.RoundToInt (tempY);
		Vector2Int tempCoord = new Vector2Int (tempColumn, tempRow);
		return tempCoord;
	}
	
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
