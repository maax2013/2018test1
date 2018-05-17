using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
	[SerializeField] GameObject unitPrefab;
	[SerializeField] Transform unitsHolder;

	Block blockCtr;
	BoardFall boardFall;

	Unit tempUnit;

	Unit[] allUnitsOnBoard;
	int tempUnitIndex;

	Unit[,] unitsTable;
	Bounds gameBoardBoundary;

	//	List<Unit> unitsToMatchingCheck;

	Unit cueUnit;

	public void init ()
	{
		blockCtr = GetComponent<Block> ();
		boardFall = GetComponent<BoardFall> ();
	}


	public void createUnits_ByRowColumn (int column, int row)
	{
//		allUnitsOnBoard = new Unit[row * column];
//		tempUnitIndex = 0;
		unitsTable = new Unit[column, row];
		gameBoardBoundary = new Bounds (Vector3.zero, new Vector3 (column, row, 0));

		/*create units table, from bottom left, to top right, column after column.*/
		for (int c = 0; c < column; c++) {
			for (int r = 0; r < row; r++) {
				GameObject tempObj = Instantiate (unitPrefab, new Vector3 (c, r, 0), Quaternion.identity) as GameObject;
				tempObj.transform.SetParent (unitsHolder, false);

				tempUnit = tempObj.GetComponent<Unit> ();
				tempUnit.initUnit (c, r);

//				allUnitsOnBoard [tempUnitIndex] = tempUnit;
//				tempUnitIndex++;
				unitsTable [c, r] = tempUnit;
			}
		}
//		tempUnitIndex = 0;
	}

	public void repositionUnitsHolder (float x, float y, float z)
	{
		unitsHolder.position = new Vector3 (x, y, z);
	}

	public void repositionBlocksHolder (float x, float y, float z)
	{
//		blockCtr = GetComponent<Block> ();
		blockCtr.repositionBlocksHolder (x, y, z);

//		blockCtr.showBlockAt (new Vector3 (0, 0, 0));
	}






	public void switch_BoardTouchable (bool on)
	{
		var inputCtr = GetComponent<InputControl> ();
		if (on) {
////			gameTouchable = true;
//			inputCtr.inputEnabled = true;
//			inputCtr.OnTouch += HandleOnTouch;
			DragDrop dragDrop = GetComponent<DragDrop> ();
			dragDrop.init (unitsTable);
			InputControl.onDragStart -= onDragStart;
			InputControl.onDragStart += onDragStart;
//			InputControl.onDrag += onDrag;
//			InputControl.onDragEnd += onDragEnd;
//			dragDrop.OnMove += switchUnit_Towards;
		} else {
////			gameTouchable = false;
//			inputCtr.inputEnabled = false;
//			inputCtr.OnTouch -= HandleOnTouch;
		}
//		GUIctr.switch_BoardTouchable (on);
	}

	void onDragEnd (GameObject obj, Vector3 pos)
	{
		dragDropDone ();
		collapseAll_matches_OnBoard ();
	}

	void dragDropDone ()
	{
		cueUnit.stopDrag ();
		DragDrop dragDrop = GetComponent<DragDrop> ();
		InputControl.onDrag -= onDrag;
		InputControl.onDragEnd -= onDragEnd;
		dragDrop.OnMove -= switchUnit_Towards;
		//++++++++++++++++++++++
	}

	void onDrag (GameObject obj, Vector3 pos)
	{
		cueUnit.transform.localPosition = pos + new Vector3 (3f, 3.5f, -1f);
		if (pointerInsideBoundary (pos, gameBoardBoundary)) {
//			print ("inside");
			GetComponent<DragDrop> ().dragMove (pos);
		} else {
//			print ("out");
			dragDropDone ();
		}
	}

	bool pointerInsideBoundary (Vector3 p, Bounds boundary)
	{
		if (boundary.Contains (p)) {
			return true;
		}
		return false;
	}

	void onDragStart (GameObject obj, Vector3 pos)
	{
		if (obj.GetComponent<Unit> () != null) {
			cueUnit = obj.GetComponent<Unit> ();
			cueUnit.startDrag ();
			DragDrop dragDrop = GetComponent<DragDrop> ();
			dragDrop.readyDrag (cueUnit);
			InputControl.onDrag -= onDrag;
			InputControl.onDrag += onDrag;
			InputControl.onDragEnd -= onDragEnd;
			InputControl.onDragEnd += onDragEnd;
			dragDrop.OnMove -= switchUnit_Towards;
			dragDrop.OnMove += switchUnit_Towards;
		}
	}

	//	void HandleOnTouch (GameObject obj)
	//	{
	////		Debug.Log (obj);
	//		if (obj.GetComponent<Unit> () != null) {
	//			cueUnit = obj.GetComponent<Unit> ();
	//			DragDrop dragDrop = GetComponent<DragDrop> ();
	//
	//			if (dragDrop.enabled) {
	//				//temprary use, before the true dragNdrop function is created
	//				dragDrop.enabled = false;//------------------------
	//				cueUnit.stopDrag ();//--------------------------
	//			} else {
	//				dragDrop.enabled = true;
	//				cueUnit.startDrag ();
	//				dragDrop.OnMove += switchUnit_Towards;
	//			}
	//
	//		}
	//	}

	void switchUnit_Towards (Vector2Int direction)
	{
		Unit targetUnit = BoardUtilities.getUnitOnTable (cueUnit.CurrentColumn + direction.x, cueUnit.CurrentRow + direction.y, unitsTable);
		if (targetUnit == null) {
			print ("out!");
			dragDropDone ();
			//+++++++++++++++++++++
		} else {
			BoardUtilities.switchUnitsCoord (cueUnit, targetUnit, unitsTable);
			/*only need to move the target unit, the cue unit is following the pointer*/
			targetUnit.moveTo (new Vector2Int (-direction.x, -direction.y));

//			tryMakeBlock (cueUnit);
//			tryMakeBlock (targetUnit);
		}
	}








	public void removeAll_match4s_OnBoard_beforeGameStart ()
	{
		List<Unit[]> candidatesGroups = new List<Unit[]> ();
		addAll_Match4sOnBoard_ToGroup (candidatesGroups);
		while (candidatesGroups.Count > 0) {
			resetUnitsInGroup (candidatesGroups);
			candidatesGroups.Clear ();
			addAll_Match4sOnBoard_ToGroup (candidatesGroups);
		}

	}

	public void collapseAll_matches_OnBoard ()
	{
//		print (boardFall.onAllFallDone);
		switch_BoardTouchable (false);

		List<Unit[]> candidatesGroups = new List<Unit[]> ();
		addAll_Match4sOnBoard_ToGroup (candidatesGroups);
//		print (candidatesGroups.Count);
		if (candidatesGroups.Count > 0) {
			collapseUnitsInGroup (candidatesGroups);
		} else {
			readyForInteraction ();
		}
	}

	void collapseUnitsInGroup (List<Unit[]> candidatesGroups)
	{
		List<Unit[]> blockGroups = new List<Unit[]> ();
		List<Unit[]> clearGroups = new List<Unit[]> ();

		for (int i = 0; i < candidatesGroups.Count; i++) {
			Unit[] match4Units = candidatesGroups [i];

			if (match4Units [0].IsUpgradable) {
				blockGroups.Add (match4Units);
			} else {
				clearGroups.Add (match4Units);
			}
		}
		StartCoroutine (makeBlocks (blockGroups));
		//TODO: handle cleargroups
	}

	IEnumerator makeBlocks (List<Unit[]> blockGroups)
	{
		int totalBlocksToMake = blockGroups.Count;
//		print (totalBlocksToMake);
		int totalCompletion = 0;
		int index = 0;
		float upgradeBlockTime = 0.8f;//~~~~~~~~~~~~~~~~~~~~~~~~
		float eachDelayTime = 0.2f;//~~~~~~~~~~~~~~~~~~~~~~

		while (index < totalBlocksToMake) {
			StartCoroutine (upgradeBlock (blockGroups [index], upgradeBlockTime, () => {
				totalCompletion++;
//				print (totalCompletion);
			}));
			index++;
			yield return new WaitForSeconds (eachDelayTime);
		}

		while (totalCompletion < totalBlocksToMake) {
			yield return new WaitForEndOfFrame ();
		}

//		System.Action<Unit[,]> handleOnAllFallDone = (Unit[,] originalTable) => {
//			unitsTable = originalTable;
//			debugBoard ();
//			print ("fire");
//			collapseAll_matches_OnBoard ();//++++++++++++++++++++
//		};
		boardFall.onAllFallDone -= handleOnAllFallDone;
		boardFall.onAllFallDone += handleOnAllFallDone;
		boardFall.fall (unitsTable);
//		boardFall.fall_1 (unitsTable);
	}

	void handleOnAllFallDone (Unit[,] originalTable)
	{
		unitsTable = originalTable;
//		debugBoard ();
//		print ("fire");
		collapseAll_matches_OnBoard ();//++++++++++++++++++++
	}

	IEnumerator upgradeBlock (Unit[] blockUnits, float duration, System.Action callback)
	{
		var rdmN = Random.Range (0, blockUnits.Length);
		Unit targetU = blockUnits [rdmN];
		List<Unit> otherUnits = new List<Unit> ();

		for (int i = 0; i < blockUnits.Length; i++) {
			if (i != rdmN) {
				otherUnits.Add (blockUnits [i]);
			}
		}

		int totalUnitsToAnimate = blockUnits.Length;
		int totalCompletions = 0;

		System.Action checkTotalCompletions = () => {
			totalCompletions++;
		};

		float mergeTime = duration * 0.5f;
		float popTime = duration * 0.5f;

		foreach (var u in otherUnits) {
			u.onMergeDone -= checkTotalCompletions;
			u.onMergeDone += checkTotalCompletions;
			u.mergeTo_overTime_thenGone (targetU.transform.localPosition, mergeTime);
		}
		/*since the first unit in blockUnits is the one at the bottom left, so the position of the block will be its x + half with, y + half height*/
		Vector3 bottomLeftU = blockUnits [0].transform.localPosition;
		Vector3 blockPos = new Vector3 (bottomLeftU.x + 0.5f, bottomLeftU.y + 0.5f, 0);
		blockCtr.showBlockAt_overTime (blockPos, mergeTime + popTime);

		yield return new WaitForSeconds (mergeTime);

		targetU.upgrade (1);
//		targetU.testMark (true);//----------------------------
		targetU.onMergeDone -= checkTotalCompletions;
		targetU.onMergeDone += checkTotalCompletions;
		targetU.popSprite_overTime (new Vector3 (1.3f, 1.3f, 1f), popTime);

//		yield return new WaitForSeconds (popTime + Time.deltaTime);
		while (totalCompletions < totalUnitsToAnimate) {
			yield return new WaitForEndOfFrame ();
		}
		callback ();
	}




	void readyForInteraction ()
	{
		print ("ready!");
		switch_BoardTouchable (true);
	}

	void addAll_Match4sOnBoard_ToGroup (List<Unit[]> candidatesGroups)
	{
		List<Unit> allUnitsList = BoardUtilities.getAllUnitsList_onTable (unitsTable);

		while (allUnitsList.Count > 0) {
			Unit tempU = allUnitsList [0];
			Unit[] match4s_TopRight = BoardUtilities.getmatch4Units_towards_onTable (tempU, new Vector2Int (1, 1), unitsTable);
			if (match4s_TopRight != null) {
				candidatesGroups.Add (match4s_TopRight);
				foreach (Unit U in match4s_TopRight) {
//					U.testMark (true);//----------------------------
					U.BelongingBlocks++;
				}
				BoardUtilities.remove_UnitsInSmallList_FromLargeList (match4s_TopRight, allUnitsList);
			} else {
				BoardUtilities.tryRemoveFromGroup (tempU, allUnitsList);
			}
		}
	}

	void resetUnitsInGroup (List<Unit[]> candidatesGroups)
	{
		for (int i = 0; i < candidatesGroups.Count; i++) {
			Unit[] match4Units = candidatesGroups [i];
			foreach (Unit u in match4Units) {
				u.reset ();
				u.testMark (false);//-----------------
			}
		}
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
