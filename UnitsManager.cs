using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
	[SerializeField] GameObject unitPrefab;

	[SerializeField] Transform unitsHolder;

	Unit tempUnit;

	Unit[] allUnitsOnBoard;
	int tempUnitIndex;

	Unit[,] unitsTable;
	Bounds gameBoardBoundary;

	//	List<Unit> unitsToMatchingCheck;

	Unit cueUnit;


	// Use this for initialization
	void Start ()
	{
		
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


	public void markAll_linkedUnitsGroups ()
	{
		/*check every unit for links. put all linked units into a group, and count total members in that group.*/
		List<Unit> unitsToCheck = new List<Unit> ();

		foreach (var u in unitsTable) {
			unitsToCheck.Add (u);
		}
//		print (unitsToCheck.Count);
		while (unitsToCheck.Count > 0) {
			List<Unit> linkedUnits = get_LinkedUnitsGroup_of (unitsToCheck [0]);
			remove_UnitsInSmallList_FromLargeList (linkedUnits, unitsToCheck);
		}
	}

	List<Unit> get_LinkedUnitsGroup_of (Unit u)
	{
		List<Unit> unitsForFurtherCheck = new List<Unit> ();
		List<Unit> linkedUnits = new List<Unit> ();

		tryAddToGroup (u, linkedUnits);

		tryLinkAdjacents (u, unitsForFurtherCheck, linkedUnits);

		if (unitsForFurtherCheck.Count > 0) {
			extendLinks (unitsForFurtherCheck, linkedUnits);
		}
		if (linkedUnits.Count > 0) {
			foreach (var unit in linkedUnits) {
				unit.TotalConnectedUnits = linkedUnits.Count;
				unit.BelongingGroup = linkedUnits;
				unit.updateCountText ();
			}
		}
		return linkedUnits;
	}

	void tryLinkAdjacents (Unit u, List<Unit> moreToCheck, List<Unit> linkedUnits)
	{
		tryMakeLinkTowards (u, new Vector2Int (0, 1), moreToCheck, linkedUnits);
		tryMakeLinkTowards (u, new Vector2Int (0, -1), moreToCheck, linkedUnits);
		tryMakeLinkTowards (u, new Vector2Int (-1, 0), moreToCheck, linkedUnits);
		tryMakeLinkTowards (u, new Vector2Int (1, 0), moreToCheck, linkedUnits);

		tryRemoveFromGroup (u, moreToCheck);
	}

	void tryMakeLinkTowards (Unit u, Vector2Int dir, List<Unit> moreToCheck, List<Unit> linkedUnits)
	{
		Unit tryLinkUnit = getSameIdUnit_Towards (u, dir);
		if (tryLinkUnit == null || tryLinkUnit.TotalConnectedUnits > 1) {
			return;
		} else {
			u.TotalConnectedUnits += 1;
			tryLinkUnit.TotalConnectedUnits += 1;
			tryAddToGroup (tryLinkUnit, linkedUnits);
			tryAddToGroup (tryLinkUnit, moreToCheck);
		}
	}

	void extendLinks (List<Unit> moreToCheck, List<Unit> linkedUnits)
	{
		while (moreToCheck.Count > 0) {
			tryLinkAdjacents (moreToCheck [0], moreToCheck, linkedUnits);
		}
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
			InputControl.onDrag += onDrag;
			InputControl.onDragEnd += onDragEnd;
			dragDrop.OnMove += switchUnit_Towards;
		}
	}

	void HandleOnTouch (GameObject obj)
	{
//		Debug.Log (obj);
		if (obj.GetComponent<Unit> () != null) {
			cueUnit = obj.GetComponent<Unit> ();
			DragDrop dragDrop = GetComponent<DragDrop> ();

			if (dragDrop.enabled) {
				//temprary use, before the true dragNdrop function is created
				dragDrop.enabled = false;//------------------------
				cueUnit.stopDrag ();//--------------------------
			} else {
				dragDrop.enabled = true;
				cueUnit.startDrag ();
				dragDrop.OnMove += switchUnit_Towards;
			}

		}
	}

	void switchUnit_Towards (Vector2Int direction)
	{
		Unit targetUnit = getUnitOnTable (cueUnit.CurrentColumn + direction.x, cueUnit.CurrentRow + direction.y);
		if (targetUnit == null) {
			print ("out!");
			dragDropDone ();
			//+++++++++++++++++++++
		} else {
//			//		targetUnit.testMark (true);
//			bool moveToSame = hasSameID (cueUnit, targetUnit);
//			if (moveToSame) {
////				swapSameIdUnits (cueUnit, targetUnit);
//
//				switchUnitsCoord (cueUnit, targetUnit, unitsTable);
//				moveUnit_Towards (cueUnit, direction);
//				moveUnit_Towards (targetUnit, new Vector2Int (-direction.x, -direction.y));
//			} else {
////				List<Unit> groupToCheckLater1 = getUnitGroup (cueUnit);
////				List<Unit> groupToCheckLater2 = getUnitGroup (targetUnit);
//
//				switchUnitsCoord (cueUnit, targetUnit, unitsTable);
//				moveUnit_Towards (cueUnit, direction);
//				moveUnit_Towards (targetUnit, new Vector2Int (-direction.x, -direction.y));
//
////				if (groupToCheckLater1 != null) {
////					
////				}
//			}
			switchUnitsCoord (cueUnit, targetUnit, unitsTable);
			/*only need to move the target unit, the cue unit is following the pointer*/
			moveUnit_Towards (targetUnit, new Vector2Int (-direction.x, -direction.y));

			tryMakeBlock (cueUnit);
			tryMakeBlock (targetUnit);
		}
	}

	void tryMakeBlock (Unit u)
	{
		List<Unit[]> surroundingMatch4s = getSurroundingMatch4s (u);

		if (surroundingMatch4s.Count > 0) {
			
		}
	}

	bool linkToExistingBlock (Unit[] group)
	{
		int totalLinksToOtherBlocks = 0;
		/*loop start with the second item, since the first one is the unit just switched over*/
		for (int i = 1; i < group.Length; i++) {
			if (group [i].blockGroup.Count > 0) {
				totalLinksToOtherBlocks++;
			}
		}
		if (totalLinksToOtherBlocks > 0) {
			return true;
		} else {
			return false;
		}
	}

	void makeNewBlock (Unit[] group)
	{
		List<Unit> blockGroup = new List<Unit> ();
		blockGroup.AddRange (group);
		foreach (Unit u in group) {
			u.blockGroup = blockGroup;
		}
		//+++++++++++block image
	}

	void tryMakeBiggerBlock ()
	{
		
	}

	List<Unit[]> getSurroundingMatch4s (Unit u)
	{
		List<Unit[]> candidatesGroups = new List<Unit[]> ();

		Unit[] topRightGroup = getmatch4Units_towards (u, new Vector2Int (1, 1));
		Unit[] topleftGroup = getmatch4Units_towards (u, new Vector2Int (-1, 1));
		Unit[] bottomRightGroup = getmatch4Units_towards (u, new Vector2Int (1, -1));
		Unit[] bottomLeftGroup = getmatch4Units_towards (u, new Vector2Int (-1, -1));

		if (topRightGroup != null)
			candidatesGroups.Add (topRightGroup);
		if (topleftGroup != null)
			candidatesGroups.Add (topleftGroup);
		if (bottomRightGroup != null)
			candidatesGroups.Add (bottomRightGroup);
		if (bottomLeftGroup != null)
			candidatesGroups.Add (bottomLeftGroup);

		return candidatesGroups;
	}


	//	void swapSameIdUnits (Unit u1, Unit u2)
	//	{
	//		var temp_TotalConnectedUnits = u1.TotalConnectedUnits;
	//		var temp_BelongingGroup = u1.BelongingGroup;
	//
	//		tryRemoveFromGroup (u1, u1.BelongingGroup);
	//		tryRemoveFromGroup (u2, u2.BelongingGroup);
	//
	//		u1.TotalConnectedUnits = u2.TotalConnectedUnits;
	//		u1.BelongingGroup = u2.BelongingGroup;
	//		tryAddToGroup (u1, u1.BelongingGroup);
	//		u1.updateCountText ();
	//
	//		u2.TotalConnectedUnits = temp_TotalConnectedUnits;
	//		u2.BelongingGroup = temp_BelongingGroup;
	//		tryAddToGroup (u2, u2.BelongingGroup);
	//		u2.updateCountText ();
	//	}

	//	List<Unit> getUnitGroup (Unit u)
	//	{
	//		if (u.TotalConnectedUnits > 1) {
	//			tryRemoveFromGroup (u, u.BelongingGroup);
	//			if (u.BelongingGroup.Count > 1) {
	//				return u.BelongingGroup;
	//			} else {
	//				Unit onlyLeftUnit = u.BelongingGroup [0];
	//				onlyLeftUnit.TotalConnectedUnits = 1;
	//				onlyLeftUnit.updateCountText ();
	//			}
	//		}
	//		return null;
	//	}

	void switchUnitsCoord (Unit u1, Unit u2, Unit[,] table)
	{
		table [u1.CurrentColumn, u1.CurrentRow] = u2;
		table [u2.CurrentColumn, u2.CurrentRow] = u1;

		int tempColumn = u1.CurrentColumn;
		int tempRow = u1.CurrentRow;
		u1.setUnitCoord (u2.CurrentColumn, u2.CurrentRow);
		u2.setUnitCoord (tempColumn, tempRow);
	}

	void moveUnit_Towards (Unit u, Vector2Int direction)
	{
		u.moveTo (direction);
	}






	void tryAddToGroup (Unit u, List<Unit> group)
	{
		if (!group.Contains (u)) {
			group.Add (u);
		}
	}

	void tryRemoveFromGroup (Unit u, List<Unit> group)
	{
		if (group.Contains (u)) {
			group.Remove (u);
		}
	}

	Unit getSameIdUnit_Towards (Unit u1, Vector2Int direction)
	{
		tempUnit = getUnitOnTable (u1.CurrentColumn + direction.x, u1.CurrentRow + direction.y);
		if (tempUnit) {
			if (hasSameID (u1, tempUnit)) {
				return tempUnit;
			}
		}
		return null;
	}

	Unit getUnitOnTable (int column, int row)
	{
		if (-1 < column && column < unitsTable.GetLength (0)
		    && -1 < row && row < unitsTable.GetLength (1)) {
			return unitsTable [column, row];
		} else {
			return null;
		}

	}

	bool hasSameID (Unit u1, Unit u2)
	{
		if (u1.UnitID == u2.UnitID) {
			return true;
		} else {
			return false;
		}
	}




	//	public void checkMatch4s_OnBoard ()
	//	{
	////		List<List<Unit>> groupsToCheck = getAll_ConnectedUnitsGroups_MinimumOf (4);
	//
	////		foreach (var l in groupsToCheck) {
	////			foreach (var u in l) {
	////				u.debugText ("=");
	////			}
	////		}
	//	}
	//
	//	List<List<Unit>> getAll_ConnectedUnitsGroups_MinimumOf (int n)
	//	{
	//		List<Unit> allUnitsList = new List<Unit> ();
	//		foreach (var u in unitsTable) {
	//			allUnitsList.Add (u);
	//		}
	//		List<List<Unit>> validGroups = new List<List<Unit>> ();
	//
	//		while (allUnitsList.Count > 0) {
	//			Unit tempU = allUnitsList [0];
	//			if (unit_ConnectToAtLeast (tempU, n)) {
	//				validGroups.Add (tempU.BelongingGroup);
	//				remove_UnitsInSmallList_FromLargeList (tempU.BelongingGroup, allUnitsList);
	//			} else {
	//				tryRemoveFromGroup (tempU, allUnitsList);
	//			}
	//		}
	//
	//		return validGroups;
	//	}
	//
	//	bool unit_ConnectToAtLeast (Unit u, int n)
	//	{
	//		if (u.TotalConnectedUnits >= n) {
	//			return true;
	//		}
	//		return false;
	//	}

	void remove_UnitsInSmallList_FromLargeList (List<Unit> smallL, List<Unit> largeL)
	{
		Unit[] tempGroup = smallL.ToArray ();
		foreach (var tempU in tempGroup) {
			tryRemoveFromGroup (tempU, largeL);
		}
	}

	void remove_UnitsInSmallList_FromLargeList (Unit[] smallL, List<Unit> largeL)
	{
		foreach (var tempU in smallL) {
			tryRemoveFromGroup (tempU, largeL);
		}
	}

	//	List<List<Unit>> getMatch4s_InGroup (List<Unit> group)
	//	{
	//		return null;
	//	}





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

	public void collapseAll_match4s_OnBoard_beforeGameStart ()
	{
		List<Unit[]> candidatesGroups = new List<Unit[]> ();
		addAll_Match4sOnBoard_ToGroup (candidatesGroups);
		if (candidatesGroups.Count > 0) {
			upgradeUnitsInGroup (candidatesGroups);
		}
	}

	void upgradeUnitsInGroup (List<Unit[]> candidatesGroups)
	{
		for (int i = 0; i < candidatesGroups.Count; i++) {
			Unit[] match4Units = candidatesGroups [i];
			foreach (Unit u in match4Units) {
				u.upgrade (1);
//				u.testMark (false);//-----------------
			}
		}
	}

	void addAll_Match4sOnBoard_ToGroup (List<Unit[]> candidatesGroups)
	{
		List<Unit> allUnitsList = getAllUnitsList ();

		while (allUnitsList.Count > 0) {
			Unit tempU = allUnitsList [0];
			Unit[] match4s_TopRight = getmatch4Units_towards (tempU, new Vector2Int (1, 1));
			if (match4s_TopRight != null) {
				candidatesGroups.Add (match4s_TopRight);
				foreach (Unit U in match4s_TopRight) {
					markMatch4Unit (U);//----------------------------
				}
				remove_UnitsInSmallList_FromLargeList (match4s_TopRight, allUnitsList);
			} else {
				tryRemoveFromGroup (tempU, allUnitsList);
			}
		}
	}

	void resetUnitsInGroup (List<Unit[]> candidatesGroups)
	{
		for (int i = 0; i < candidatesGroups.Count; i++) {
			Unit[] match4Units = candidatesGroups [i];
			foreach (Unit u in match4Units) {
				u.randomId ();
				u.testMark (false);//-----------------
			}
		}
	}

	List<Unit> getAllUnitsList ()
	{
		List<Unit> allUnitsList = new List<Unit> ();
		foreach (var u in unitsTable) {
			allUnitsList.Add (u);
		}
		return allUnitsList;
	}


	Unit[] getmatch4Units_towards (Unit u, Vector2Int diagonal)
	{
		Unit linkedUnit_corner = getSameIdUnit_Towards (u, diagonal);
		Unit linkedUnit_side1 = getSameIdUnit_Towards (u, new Vector2Int (diagonal.x, 0));
		Unit linkedUnit_side2 = getSameIdUnit_Towards (u, new Vector2Int (0, diagonal.y));


		if (linkedUnit_corner && linkedUnit_side1 && linkedUnit_side2) {
			/*make sure no overlaps*/
			if (linkedUnit_corner.childOfBlocks < 1 &&
			    linkedUnit_side1.childOfBlocks < 1 &&
			    linkedUnit_side2.childOfBlocks < 1) {
				/*make sure to put self in the first slot*/
				Unit[] match4Group = new Unit[] { u, linkedUnit_corner, linkedUnit_side1, linkedUnit_side2 };
				return match4Group;
			}
		}
		return null;
	}

	void markMatch4Unit (Unit u)
	{
		u.testMark (true);//----------------------------
		u.childOfBlocks++;
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
	}
	


	//	List<Unit> joinFirstGroup_ToSecondGroup (List<Unit> g1, List<Unit> g2)
	//	{
	//		Unit[] tempGroup = g1.ToArray ();
	//		foreach (var tempU in tempGroup) {
	//			tryAddToGroup (tempU, g2);
	//		}
	//		return g2;
	//	}
	//	public void checkMatchesOnBoard ()
	//	{
	//		unitsToMatchingCheck = new List<Unit> ();
	//		for (int i = 0; i < allUnitsOnBoard.Length; i++) {
	//			unitsToMatchingCheck.Add (allUnitsOnBoard [i]);
	//		}
	//		//Check every unit for connections, from top left, to Top right.
	//		//only check the unit to the right and the unit to the Top.
	//		//Mark all connected units with the number of connections then put them into a group.
	//		while (unitsToMatchingCheck.Count > 0) {
	//			checkConnections (unitsToMatchingCheck [0]);
	//		}
	//	}
	//
	//	bool hasRectangleConnections (Unit u)
	//	{
	//		temp_ConnectedUnits_Group.Add (u);
	//		var connectedUnitRight = connectedUnitTowards (u, new Vector2Int (1, 0));
	//		var connectedUnitTop = connectedUnitTowards (u, new Vector2Int (1, 0));
	//		if (connectedUnitRight && connectedUnitTop) {
	//			if (connectedUnitTowards (u, new Vector2Int (1, 1))) {
	//
	//
	//				//+++++
	//				foreach (var unit in temp_ConnectedUnits_Group) {
	//					unitsToMatchingCheck.Remove (unit);
	//				}
	//				return true;
	//			}
	//		}
	//		return false;
	//	}
	//
	//	void checkForBiggerRectangle (Unit u1, Unit u2)
	//	{
	//
	//	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
