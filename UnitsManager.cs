using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
	[SerializeField] GameObject unitPrefab;

	[SerializeField] Transform unitsHolder;

	GameObject tempUnit;
	Unit tempUnitCtr;

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
		unitsTable = new Unit[row, column];
		tempUnitIndex = 0;

		for (int r = 0; r < row; r++) {
			for (int c = 0; c < column; c++) {
				tempUnit = Instantiate (unitPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				tempUnitCtr = tempUnit.GetComponent<Unit> ();
				tempUnitCtr.initRandomUnit (c, r);

				tempUnit.transform.parent = unitsHolder;
				tempUnit.transform.position = new Vector3 (c, -r, tempUnit.transform.position.z);

				allUnitsOnBoard [tempUnitIndex] = tempUnitCtr;
				tempUnitIndex++;
				unitsTable [r, c] = tempUnitCtr;
			}
		}
	}

	public void repositionUnitsHolder (float x, float y, float z)
	{
		unitsHolder.position = new Vector3 (x, y, z);
	}

	public void checkMatchesOnBoard ()
	{
		unitsToMatchingCheck = new List<Unit> ();
		for (int i = 0; i < allUnitsOnBoard.Length; i++) {
			unitsToMatchingCheck.Add (allUnitsOnBoard [i]);
		}
		while (unitsToMatchingCheck.Count > 0) {
			checkConnections (unitsToMatchingCheck [0], new Vector2 (1, 0));
		}
	}

	void matchingCheck ()
	{
		
	}

	void checkConnections (Unit u, Vector2 moveTo)
	{
		if (tempConnectedUnitsGroup) {
			tempConnectedUnitsGroup.Clear ();
		} else {
			tempConnectedUnitsGroup = new List<Unit> ();
		}
		if (hasRectangleConnections (u)) {
			
		} else {
			unitsToMatchingCheck.Remove (u);
		}


	}

	bool hasRectangleConnections (Unit u)
	{
		tempConnectedUnitsGroup.Add (u);
		var connectedUnitRight = connectedUnitTowards (u, new Vector2 (1, 0));
		var connectedUnitbottom = connectedUnitTowards (u, new Vector2 (1, 0));
		if (connectedUnitRight && connectedUnitbottom) {
			if (connectedUnitTowards (u, new Vector2 (1, 1))) {


				//+++++
				foreach (var unit in tempConnectedUnitsGroup) {
					unitsToMatchingCheck.Remove (unit);
				}
				return true;
			}
		}
		return false;
	}

	void checkForBiggerRectangle (Unit u1, Unit u2)
	{
		
	}

	Unit connectedUnitTowards (Unit u1, Vector2 direction)
	{
		tempUnit = getUnitOnTable (u1.CurrentRow + direction.y, u1.CurrentColumn + direction.x);
		if (tempUnit) {
			if (hasSameID (u1, tempUnit)) {
				updateConnectedUnitsGroup (tempUnit, tempConnectedUnitsGroup);
				return tempUnit;
			}
		}
		return null;
	}

	Unit getUnitOnTable (int row, int column)
	{
		if (-1 < row < unitsTable.GetLength (0) && -1 < column < unitsTable.GetLength (0)) {
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

	void updateConnectedUnitsGroup (Unit u, List<Unit> group)
	{
		if (!group.Contains (u)) {
			group.Add (u);
			foreach (var unit in group) {
				unit.TotalUnitsThisConnectsTo += 1;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
