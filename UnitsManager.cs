﻿using System.Collections;
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
////			Debug.Log (u.TotalConnectedUnits);
//			u.updateCountText (u.TotalConnectedUnits);
//		}


//		for (int i = 0; i < allUnitsOnBoard.Length; i++) {
//			unitsToMatchingCheck.Add (allUnitsOnBoard [i]);
//		}
	}

	void checkConnectionsToRightAndBottom (Unit u)
	{
		if (u.TotalConnectedUnits == 1) {
			tempConnectedUnitsGroup.Clear ();
			tempConnectedUnitsGroup.Add (u);
		} else {
			tempConnectedUnitsGroup = u.BelongingGroup;
		}

		var connectedUnitRight = connectedUnitTowards (u, new Vector2 (1, 0));
		var connectedUnitbottom = connectedUnitTowards (u, new Vector2 (0, 1));
		if (connectedUnitRight || connectedUnitbottom) {
			if (connectedUnitRight) {
				if (connectedUnitRight.TotalConnectedUnits == 1) {
					tempConnectedUnitsGroup.Add (connectedUnitRight);
				} else {
//					foreach (var tempU in connectedUnitRight.BelongingGroup) {
//						tempConnectedUnitsGroup.Add (tempU);
//					}
//					for (int i = 0; i < connectedUnitRight.BelongingGroup.Count; i++) {
//						tempConnectedUnitsGroup.Add (connectedUnitRight.BelongingGroup [i]);
//					}
					tempConnectedUnitsGroup.Add (connectedUnitRight);
				}

			}
			if (connectedUnitbottom) {
				tempConnectedUnitsGroup.Add (connectedUnitbottom);
			}

			foreach (var unit in tempConnectedUnitsGroup) {
				unit.TotalConnectedUnits = tempConnectedUnitsGroup.Count;
				unit.BelongingGroup = tempConnectedUnitsGroup;
				unit.updateCountText (u.TotalConnectedUnits);
			}

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
