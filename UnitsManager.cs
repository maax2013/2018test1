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

	//	List<Unit> unitsToMatchingCheck;

	Unit cueUnit;


	// Use this for initialization
	void Start ()
	{
		
	}

	public void createUnits_ByRowColumn (int row, int column)
	{
//		allUnitsOnBoard = new Unit[row * column];
//		tempUnitIndex = 0;
		unitsTable = new Unit[row, column];

		for (int r = 0; r < row; r++) {
			for (int c = 0; c < column; c++) {
				GameObject tempObj = Instantiate (unitPrefab, new Vector3 (c, r, 0), Quaternion.identity) as GameObject;
				tempObj.transform.SetParent (unitsHolder, false);

				tempUnit = tempObj.GetComponent<Unit> ();
				tempUnit.initRandomUnit (c, r);

//				allUnitsOnBoard [tempUnitIndex] = tempUnit;
//				tempUnitIndex++;
				unitsTable [r, c] = tempUnit;
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
		//check every unit for links. put all linked units into a group, and count total members in that group.
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
//			gameTouchable = true;
			inputCtr.touchable = true;
			inputCtr.OnTouch += HandleOnTouch;
		} else {
//			gameTouchable = false;
			inputCtr.touchable = false;
			inputCtr.OnTouch -= HandleOnTouch;
		}
//		GUIctr.switch_BoardTouchable (on);
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
		Unit targetUnit = getUnitOnTable (cueUnit.CurrentRow + direction.y, cueUnit.CurrentColumn + direction.x);
		targetUnit.testMark (true);
	}

	void moveUnit_Towards (Unit u, Vector2Int direction)
	{
		
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
		tempUnit = getUnitOnTable (u1.CurrentRow + direction.y, u1.CurrentColumn + direction.x);
		if (tempUnit) {
			if (hasSameID (u1, tempUnit)) {
				return tempUnit;
			}
		}
		return null;
	}

	Unit getUnitOnTable (int row, int column)
	{
		if (-1 < row && row < unitsTable.GetLength (0)
		    && -1 < column && column < unitsTable.GetLength (1)) {
			return unitsTable [row, column];
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




	public void checkMatch4s_OnBoard ()
	{
//		List<List<Unit>> groupsToCheck = getAll_ConnectedUnitsGroups_MinimumOf (4);

//		foreach (var l in groupsToCheck) {
//			foreach (var u in l) {
//				u.debugText ("=");
//			}
//		}
	}

	List<List<Unit>> getAll_ConnectedUnitsGroups_MinimumOf (int n)
	{
		List<Unit> allUnitsList = new List<Unit> ();
		foreach (var u in unitsTable) {
			allUnitsList.Add (u);
		}
		List<List<Unit>> validGroups = new List<List<Unit>> ();

		while (allUnitsList.Count > 0) {
			Unit tempU = allUnitsList [0];
			if (unit_ConnectToAtLeast (tempU, n)) {
				validGroups.Add (tempU.BelongingGroup);
				remove_UnitsInSmallList_FromLargeList (tempU.BelongingGroup, allUnitsList);
			} else {
				tryRemoveFromGroup (tempU, allUnitsList);
			}
		}

		return validGroups;
	}

	bool unit_ConnectToAtLeast (Unit u, int n)
	{
		if (u.TotalConnectedUnits >= n) {
			return true;
		}
		return false;
	}

	void remove_UnitsInSmallList_FromLargeList (List<Unit> smallL, List<Unit> largeL)
	{
		Unit[] tempGroup = smallL.ToArray ();
		foreach (var tempU in tempGroup) {
			tryRemoveFromGroup (tempU, largeL);
		}
	}

	List<List<Unit>> getMatch4s_InGroup (List<Unit> group)
	{
		return null;
	}

	bool isMatch4 (Unit u)
	{
		Unit connectedUnitRight = getSameIdUnit_Towards (u, new Vector2Int (1, 0));
		Unit connectedUnitTop = getSameIdUnit_Towards (u, new Vector2Int (0, 1));
		Unit connectedUnitTopRight = getSameIdUnit_Towards (u, new Vector2Int (1, 1));

		if (connectedUnitRight && connectedUnitTop && connectedUnitTopRight) {
			return true;
		}
		return false;
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
