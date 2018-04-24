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
	Unit[,] unitsTable;
	int tempUnitIndex;

	List<Unit> unitsToMatchingCheck;
	List<Unit> tempConnectedUnitsGroup;


	// Use this for initialization
	void Start ()
	{
		
	}

	public void createUnitsByRowColumn (int row, int column)
	{
		allUnitsOnBoard = new Unit[row * column];
		tempUnitIndex = 0;
		unitsTable = new Unit[row, column];

		for (int r = 0; r < row; r++) {
			for (int c = 0; c < column; c++) {
				tempObj = Instantiate (unitPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				tempUnit = tempObj.GetComponent<Unit> ();
				tempUnit.initRandomUnit (c, r);

				tempObj.transform.parent = unitsHolder;
				tempObj.transform.position = new Vector3 (c, -r, tempObj.transform.position.z);

				allUnitsOnBoard [tempUnitIndex] = tempUnit;
				tempUnitIndex++;
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

//		foreach (var u in unitsTable) {
////			Debug.Log (u.CurrentRow + ", " + u.CurrentColumn);
////			u.updateCountText (u.TotalConnectedUnits);
//		}


//		for (int i = 0; i < allUnitsOnBoard.Length; i++) {
//			unitsToMatchingCheck.Add (allUnitsOnBoard [i]);
//		}
	}

	void stepCheck ()
	{
		checkConnectionsToRightAndBottom (allUnitsOnBoard [tempUnitIndex]);
		tempUnitIndex++;
	}

	void checkConnectionsToRightAndBottom (Unit u)
	{
		tempConnectedUnitsGroup = new List<Unit> ();
//		Debug.Log (u.TotalConnectedUnits);
		if (u.TotalConnectedUnits == 1) {
//			tempConnectedUnitsGroup.Add (u);
			tryAddToGroup (tempConnectedUnitsGroup, u);
		} else {
			tempConnectedUnitsGroup = joinSecondGroupToFirstGroup (tempConnectedUnitsGroup, u.BelongingGroup);
		}

		var connectedUnitRight = connectedUnitTowards (u, new Vector2 (1, 0));
		var connectedUnitbottom = connectedUnitTowards (u, new Vector2 (0, 1));
		if (connectedUnitRight || connectedUnitbottom) {
			if (connectedUnitRight) {
				if (connectedUnitRight.TotalConnectedUnits == 1) {
//					tempConnectedUnitsGroup.Add (connectedUnitRight);
					tryAddToGroup (tempConnectedUnitsGroup, connectedUnitRight);
				} else {
					tempConnectedUnitsGroup = joinSecondGroupToFirstGroup (tempConnectedUnitsGroup, connectedUnitRight.BelongingGroup);
//					for (int i = 0; i < connectedUnitRight.BelongingGroup.Count; i++) {
//						tempConnectedUnitsGroup.Add (connectedUnitRight.BelongingGroup [i]);
//					}
//					tempConnectedUnitsGroup.Add (connectedUnitRight);
				}

			}
			if (connectedUnitbottom) {
				tryAddToGroup (tempConnectedUnitsGroup, connectedUnitbottom);
			}

			foreach (var unit in tempConnectedUnitsGroup) {
				unit.TotalConnectedUnits = tempConnectedUnitsGroup.Count;
				unit.BelongingGroup = tempConnectedUnitsGroup;
				unit.updateCountText ();
			}

		}

	}

	List<Unit> joinSecondGroupToFirstGroup (List<Unit> g1, List<Unit> g2)
	{
		Unit[] tempGroup = g2.ToArray ();

		foreach (var tempU in tempGroup) {
//			g1.Add (tempU);
			tryAddToGroup (g1, tempU);
		}
		return g1;
	}

	void tryAddToGroup (List<Unit> group, Unit u)
	{
		if (!group.Contains (u)) {
			group.Add (u);
		}
	}

	Unit connectedUnitTowards (Unit u1, Vector2 direction)
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


	//	void OnGUI ()
	//	{
	//		if (GUI.Button (new Rect (0, 0, 100, 35), "skip")) {
	//			stepCheck ();
	//		}
	//
	//	}




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
