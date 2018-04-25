using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
	[SerializeField] GameObject unitPrefab;

	[SerializeField] Transform unitsHolder;

	GameObject tempObj;
	Unit tempUnit;

	Unit[] allUnitsOnBoard;
	int tempUnitIndex;

	Unit[,] unitsTable;

	List<Unit> unitsToMatchingCheck;
	List<Unit> tempConnectedUnitsGroup;


	// Use this for initialization
	void Start ()
	{
		
	}

	public void createUnitsByRowColumn (int row, int column)
	{
//		allUnitsOnBoard = new Unit[row * column];
//		tempUnitIndex = 0;
		unitsTable = new Unit[row, column];

		for (int r = 0; r < row; r++) {
			for (int c = 0; c < column; c++) {
				tempObj = Instantiate (unitPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				tempUnit = tempObj.GetComponent<Unit> ();
				tempUnit.initRandomUnit (c, r);

				tempObj.transform.parent = unitsHolder;
				tempObj.transform.position = new Vector3 (c, -r, tempObj.transform.position.z);

//				allUnitsOnBoard [tempUnitIndex] = tempUnit;
//				tempUnitIndex++;
				unitsTable [r, c] = tempUnit;
			}
		}
		tempUnitIndex = 0;
	}

	public void repositionUnitsHolder (float x, float y, float z)
	{
		unitsHolder.position = new Vector3 (x, y, z);
	}

	public void groupConnectedUnitsOnBoard ()
	{
		//Check every unit for connections, from top left, to bottom right.
		//only check the unit to the right and the unit to the bottom.
		//Mark all connected units with the number of connections then put them into a group.
		tempConnectedUnitsGroup = new List<Unit> ();
		foreach (var u in unitsTable) {
			checkConnectionsToRightAndBottom (u);
		}

		foreach (var u in unitsTable) {
//			Debug.Log (u.CurrentRow + ", " + u.CurrentColumn);
			u.updateCountText ();
		}
	}

	public void switchGameTouchable (bool on)
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
//		GUIctr.switchGameTouchable (on);
	}

	void HandleOnTouch (GameObject obj)
	{
//		Debug.Log (obj);
		if (obj.GetComponent<Unit> () != null) {
			Unit unit = obj.GetComponent<Unit> ();
			if (GetComponent<DragDrop> ().enabled) {
				//temprary use, before the true dragNdrop function is created
				GetComponent<DragDrop> ().enabled = false;//------------------------
				unit.stopDrag ();//--------------------------
			} else {
				GetComponent<DragDrop> ().enabled = true;
				unit.startDrag ();
			}

		}
	}


	void checkConnectionsToRightAndBottom (Unit u)
	{
		//		Debug.Log (u.TotalConnectedUnits);
		tempConnectedUnitsGroup = new List<Unit> ();
		if (u.TotalConnectedUnits < 2) {
			tryAddToGroup (u, tempConnectedUnitsGroup);
		} else {
			tempConnectedUnitsGroup = joinFirstGroupToSecondGroup (u.BelongingGroup, tempConnectedUnitsGroup);
		}

		Unit connectedUnitRight = getConnectedUnitTowards (u, new Vector2 (1, 0));
		Unit connectedUnitBottom = getConnectedUnitTowards (u, new Vector2 (0, 1));
		if (connectedUnitRight || connectedUnitBottom) {
			if (connectedUnitRight) {
				if (connectedUnitRight.TotalConnectedUnits < 2) {
					tryAddToGroup (connectedUnitRight, tempConnectedUnitsGroup);
				} else {
					tempConnectedUnitsGroup = joinFirstGroupToSecondGroup (connectedUnitRight.BelongingGroup, tempConnectedUnitsGroup);
				}
			}
			if (connectedUnitBottom) {
				tryAddToGroup (connectedUnitBottom, tempConnectedUnitsGroup);
			}

			foreach (var unit in tempConnectedUnitsGroup) {
				unit.TotalConnectedUnits = tempConnectedUnitsGroup.Count;
				unit.BelongingGroup = tempConnectedUnitsGroup;
//				unit.updateCountText ();
			}
		}
	}

	List<Unit> joinFirstGroupToSecondGroup (List<Unit> g1, List<Unit> g2)
	{
		Unit[] tempGroup = g1.ToArray ();
		foreach (var tempU in tempGroup) {
			tryAddToGroup (tempU, g2);
		}
		return g2;
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

	Unit getConnectedUnitTowards (Unit u1, Vector2 direction)
	{
		tempUnit = getUnitOnTable (u1.CurrentRow + (int)direction.y, u1.CurrentColumn + (int)direction.x);
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




	public void checkMatch4sOnBoard ()
	{
		List<List<Unit>> groupsToCheck = getAllConnectedUnitsGroupsMinimumOf (4);
//		foreach (var l in groupsToCheck) {
//			foreach (var u in l) {
//				u.debugText ("=");
//			}
//		}
	}

	List<List<Unit>> getAllConnectedUnitsGroupsMinimumOf (int n)
	{
		List<Unit> allUnitsList = new List<Unit> ();
		foreach (var u in unitsTable) {
			allUnitsList.Add (u);
		}
		List<List<Unit>> validGroups = new List<List<Unit>> ();

		while (allUnitsList.Count > 0) {
			Unit tempU = allUnitsList [0];
			if (doesUnitConnectedToMinimumOf (tempU, n)) {
				validGroups.Add (tempU.BelongingGroup);
				removeUnitsInSmallListFromLargeList (tempU.BelongingGroup, allUnitsList);
			} else {
				tryRemoveFromGroup (tempU, allUnitsList);
			}
		}

		return validGroups;
	}

	bool doesUnitConnectedToMinimumOf (Unit u, int n)
	{
		if (u.TotalConnectedUnits >= n) {
			return true;
		}
		return false;
	}

	void removeUnitsInSmallListFromLargeList (List<Unit> smallL, List<Unit> largeL)
	{
		Unit[] tempGroup = smallL.ToArray ();
		foreach (var tempU in tempGroup) {
			tryRemoveFromGroup (tempU, largeL);
		}
	}

	List<List<Unit>> getMatch4sInGroup (List<Unit> group)
	{
		return null;
	}

	bool isMatch4 (Unit u)
	{
		Unit connectedUnitRight = getConnectedUnitTowards (u, new Vector2 (1, 0));
		Unit connectedUnitBottom = getConnectedUnitTowards (u, new Vector2 (0, 1));//~~~~~~~~~~~~~~~~~~~~~~~~~~~~ -1?
		Unit connectedUnitBottomRight = getConnectedUnitTowards (u, new Vector2 (1, 1));

		if (connectedUnitRight && connectedUnitBottom && connectedUnitBottomRight) {
			return true;
		}
		return false;
	}







	//	void OnGUI ()
	//	{
	//		if (GUI.Button (new Rect (0, 0, 100, 35), "skip")) {
	//			stepCheck ();
	//		}
	//
	//	}

	//	void stepCheck ()
	//	{
	//		checkConnectionsToRightAndBottom (allUnitsOnBoard [tempUnitIndex]);
	//		tempUnitIndex++;
	//	}
	//



	//	public void checkMatchesOnBoard ()
	//	{
	//		unitsToMatchingCheck = new List<Unit> ();
	//		for (int i = 0; i < allUnitsOnBoard.Length; i++) {
	//			unitsToMatchingCheck.Add (allUnitsOnBoard [i]);
	//		}
	//		//Check every unit for connections, from top left, to bottom right.
	//		//only check the unit to the right and the unit to the bottom.
	//		//Mark all connected units with the number of connections then put them into a group.
	//		while (unitsToMatchingCheck.Count > 0) {
	//			checkConnections (unitsToMatchingCheck [0]);
	//		}
	//	}
	//
	//	bool hasRectangleConnections (Unit u)
	//	{
	//		tempConnectedUnitsGroup.Add (u);
	//		var connectedUnitRight = connectedUnitTowards (u, new Vector2 (1, 0));
	//		var connectedUnitbottom = connectedUnitTowards (u, new Vector2 (1, 0));
	//		if (connectedUnitRight && connectedUnitbottom) {
	//			if (connectedUnitTowards (u, new Vector2 (1, 1))) {
	//
	//
	//				//+++++
	//				foreach (var unit in tempConnectedUnitsGroup) {
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
