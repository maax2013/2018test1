//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
	[SerializeField] GameObject unitPrefab;
	[SerializeField] Transform unitsHolder;

	Block blockCtr;
	BoardFall boardFall;
	BoardMatch boardMatch;
    BoardCompose boardCompose;
    BoardSwapUnits boardSwapUnits;
	AllUnitTypes allTypes;

	Unit tempUnit;

	Unit[] allUnitsOnBoard;
	int tempUnitIndex;

	Unit[,] unitsTable;

	public void init ()
	{
		blockCtr = GetComponent<Block> ();
		boardMatch = GetComponent<BoardMatch> ();
		boardFall = GetComponent<BoardFall> ();
        boardCompose = GetComponent<BoardCompose>();
        boardSwapUnits = GetComponent<BoardSwapUnits>();
		allTypes = GetComponent<AllUnitTypes> ();
	}


	public void createUnits_ByRowColumn (int column, int row)
	{
		unitsTable = new Unit[column, row];

		/*create units table, from bottom left, to top right, column after column.*/
		for (int c = 0; c < column; c++) {
			for (int r = 0; r < row; r++) {
				GameObject tempObj = Instantiate (unitPrefab, new Vector3 (c, r, 0), Quaternion.identity) as GameObject;
				tempObj.transform.SetParent (unitsHolder, false);

				tempUnit = tempObj.GetComponent<Unit> ();
				tempUnit.initUnit (c, r, allTypes);

				unitsTable [c, r] = tempUnit;
			}
		}
	}

	public void repositionUnitsHolder (float x, float y, float z)
	{
		unitsHolder.position = new Vector3 (x, y, z);
	}

	public void repositionBlocksHolder (float x, float y, float z)
	{
		blockCtr.repositionBlocksHolder (x, y, z);
	}

	public void initBoardSwapUnits (CountDownTimeBar cdt)
	{
        boardSwapUnits.init(unitsTable);
		boardSwapUnits.passCDTimer(cdt);
	}
    public void passBlueprint(string[,] bp){
        boardCompose.setBlueprint(bp);
    }

    //public void addEtraSwappingTime(float t){
    //    boardSwapUnits.addTempEtraSwappingTime(t);
    //}






	public void switch_GUITouchable (bool on)
	{
        //TODO: GUI touchable
	}

    public void enableBoardDragging(){
        boardSwapUnits.onAllSwapsDone -= handleOnAllSwapsDone;
        boardSwapUnits.onAllSwapsDone += handleOnAllSwapsDone;
        boardSwapUnits.enableDragging();
    }
    public void disableBoardDragging(){
        boardSwapUnits.disableDragging();
    }

    void handleOnAllSwapsDone(){
        collapseAll_matches_OnBoard();
    }



//	void onDragEnd (GameObject obj, Vector3 pos)
//	{
//		dragDropDone ();
//		collapseAll_matches_OnBoard ();
//	}

//	void dragDropDone ()
//	{
//		if (cueUnit) {
//			cueUnit.stopDrag ();
//			cueUnit = null;
//		}
//		InputControl.onDrag -= onDrag;
//		InputControl.onDragEnd -= onDragEnd;
//		dragDrop.OnMove -= switchUnit_Towards;
//		dragDrop.OnMove -= startCDTimer;
//		cdTimer.stopTimer ();
//		//++++++++++++++++++++++
//	}

//	void onDrag (GameObject obj, Vector3 pos)
//	{
//		cueUnit.transform.localPosition = pos + new Vector3 (3f, 3.5f, -1f);
//		if (pointerInsideBoundary (pos, gameBoardBoundary)) {
////			print ("inside");
//			GetComponent<DragDrop> ().dragMove (pos);
//		} else {
////			print ("out");
//			dragDropDone ();
//		}
//	}

//	bool pointerInsideBoundary (Vector3 p, Bounds boundary)
//	{
//		if (boundary.Contains (p)) {
//			return true;
//		}
//		return false;
//	}

//	void onDragStart (GameObject obj, Vector3 pos)
//	{
//		if (obj.GetComponent<Unit> () != null) {
//			cueUnit = obj.GetComponent<Unit> ();
//			cueUnit.startDrag ();
//			dragDrop.readyDrag (cueUnit);
//			InputControl.onDrag -= onDrag;
//			InputControl.onDrag += onDrag;
//			InputControl.onDragEnd -= onDragEnd;
//			InputControl.onDragEnd += onDragEnd;
//			dragDrop.OnMove -= switchUnit_Towards;
//			dragDrop.OnMove += switchUnit_Towards;
//			dragDrop.OnMove -= startCDTimer;
//			dragDrop.OnMove += startCDTimer;
//		}
//	}

//	//	void HandleOnTouch (GameObject obj)
//	//	{
//	////		Debug.Log (obj);
//	//		if (obj.GetComponent<Unit> () != null) {
//	//			cueUnit = obj.GetComponent<Unit> ();
//	//			DragDrop dragDrop = GetComponent<DragDrop> ();
//	//
//	//			if (dragDrop.enabled) {
//	//				//temprary use, before the true dragNdrop function is created
//	//				dragDrop.enabled = false;//------------------------
//	//				cueUnit.stopDrag ();//--------------------------
//	//			} else {
//	//				dragDrop.enabled = true;
//	//				cueUnit.startDrag ();
//	//				dragDrop.OnMove += switchUnit_Towards;
//	//			}
//	//
//	//		}
//	//	}

//	void startCDTimer (Vector2Int direction)
//	{
//		if (!cdTimer.IsRuning ()) {
//			cdTimer.onTimesUp -= handleOnTimesUp;
//			cdTimer.onTimesUp += handleOnTimesUp;
//			cdTimer.startCountDown (5f);
//		}
//	}

//	void handleOnTimesUp ()
//	{
//		dragDropDone ();
//		collapseAll_matches_OnBoard ();
//	}

//	void switchUnit_Towards (Vector2Int direction)
//	{
//		Unit targetUnit = BoardUtilities.getUnitOnTable (cueUnit.CurrentColumn + direction.x, cueUnit.CurrentRow + direction.y, unitsTable);
//		if (targetUnit == null) {
//			print ("out!");
//			dragDropDone ();
//			//+++++++++++++++++++++
//		} else {
////			BoardUtilities.switchUnitsCoord (cueUnit, targetUnit, unitsTable);
//			/*only need to move the target unit, the cue unit is following the pointer*/
//			targetUnit.moveTo (new Vector2Int (-direction.x, -direction.y));
//			BoardUtilities.switchUnitsCoord (cueUnit, targetUnit, unitsTable);

////			tryMakeBlock (cueUnit);
////			tryMakeBlock (targetUnit);
	//	}
	//}






	public void removeAll_match4s_OnBoard_beforeGameStart ()
	{
        disableBoardDragging ();
		boardMatch.removeAll_match4s_OnBoard_beforeGameStart (unitsTable);
		readyForInteraction ();
	}

	public void collapseAll_matches_OnBoard ()
	{
        disableBoardDragging ();
		boardMatch.onAllMatchDone -= handleOnAllMatchDone;
		boardMatch.onAllMatchDone += handleOnAllMatchDone;

		boardMatch.onNeedShowBlock -= handleOnNeedShowBlock;
		boardMatch.onNeedShowBlock += handleOnNeedShowBlock;

		boardMatch.onNeedBoardFall -= handleOnNeedBoardFall;
		boardMatch.onNeedBoardFall += handleOnNeedBoardFall;

		boardMatch.collapseAll_matches_OnBoard (unitsTable);
	}

	void handleOnAllMatchDone (Unit[,] updatedTable)
	{
		unitsTable = updatedTable;
		//		debugBoard ();
        if(boardCompose.compositionDone_onBoard(unitsTable)){
            //TODO: level complete
            Debug.Log("levle complete!!!!!");
        }else{
            readyForInteraction();
        }
		
	}

	void handleOnNeedBoardFall (Unit[,] updatedTable)
	{
		unitsTable = updatedTable;
		//		debugBoard ();
		boardFall.onAllFallDone -= handleOnAllFallDone;
		boardFall.onAllFallDone += handleOnAllFallDone;
		boardFall.fall (unitsTable);
	}

	void handleOnAllFallDone (Unit[,] updatedTable)
	{
		unitsTable = updatedTable;
		//		debugBoard ();
		boardMatch.collapseAll_matches_OnBoard (unitsTable);
	}

	void handleOnNeedShowBlock (Vector3 targetP, float duration)
	{
		blockCtr.showBlockAt_overTime (targetP, duration);
	}

	void readyForInteraction ()
	{
//		print ("ready!");
        enableBoardDragging ();
	}












	void OnGUI ()
	{
		if (GUI.Button (new Rect (0, 0, 200, 55), "test")) {
			stepCheck ();
		}
	}

	void stepCheck ()
	{
//			checkConnections_ToRightAndTop (allUnitsOnBoard [tempUnitIndex]);
//			tempUnitIndex++;

//		unitsToCheck [0].testMark (true);
//		get_LinkedUnitsGroup_of (unitsToCheck [0]);
		collapseAll_matches_OnBoard ();
	}

	void debugBoard ()
	{
		for (int c = 0; c < unitsTable.GetLength (0); c++) {
			for (int r = 0; r < unitsTable.GetLength (1); r++) {
//				unitsTable [c, r].debugText (c.ToString () + ":" + r.ToString ());
				//				unitsTable [c, r].showDebugCoord();
				unitsTable [c, r].debugText (unitsTable [c, r].BelongingBlocks.ToString ());
			}
		}
	}
	



	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
