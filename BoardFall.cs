using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFall : MonoBehaviour
{
	public event System.Action<Unit[,]> onAllFallDone;

	Unit[,] originalTable;
	Unit[,] unitsTable;
	static int? firstEmpty;
	
	List<List<Unit>> readyToFallColumnGroups = new List<List<Unit>> ();
	List<Unit> readyToFallUnitsInColumn = new List<Unit> ();
	List<int> skyfallColumnIndexes = new List<int> ();
	List<Unit> reusableUnits = new List<Unit> ();
	
	int totalFallingUnits = 0;
	int totalCompletions = 0;
    const float fallDelay = 0.01f;

	public void fall (Unit[,] units)
	{
		//		print ("fall");
		originalTable = units;

		/*List of groups of units to fall, separate by columns*/
		readyToFallColumnGroups.Clear ();
		/*list of column indexes, should be the same length as readyToFallColumnGroups*/
		skyfallColumnIndexes.Clear ();

		reusableUnits.Clear ();
	
		unitsTable = new Unit[originalTable.GetLength (0), originalTable.GetLength (1)];
		unitsTable = convertTable (originalTable, unitsTable);
	
		groupFallingUnitsByColumn_onBoard (unitsTable);
		addSkyfallUnits_toFallingColumns (readyToFallColumnGroups);
		StartCoroutine (unitsFall ());
	}

	void groupFallingUnitsByColumn_onBoard (Unit[,] unitsTable)
	{
		for (int x = 0; x < unitsTable.GetLength (0); x++) {
			firstEmpty = null;
			readyToFallUnitsInColumn = new List<Unit> ();
	
			for (int y = 0; y < unitsTable.GetLength (1); y++) {
				if (unitsTable [x, y] == null && !firstEmpty.HasValue) {
					firstEmpty = y;
				} else if (firstEmpty.HasValue && unitsTable [x, y] != null) {
					Unit fallingU = unitsTable [x, y];
					/*mark Falling Unit Start and End position Coords, for animation later*/
					fallingU.fallFrom = new Vector2Int (x, y);
					fallingU.fallTo = new Vector2Int (x, firstEmpty.Value);

					readyToFallUnitsInColumn.Add (fallingU);
	
					/*shift table coords*/
					unitsTable [x, firstEmpty.Value] = unitsTable [x, y];
					unitsTable [x, y] = null;
					firstEmpty++;
				}
			}
			/*as long as firstEmpty has value, meaning there a empty cell in the column, 
			if the empty cells on the very top, still add a empty list to the group, for add skyfall later*/
			if (firstEmpty.HasValue) {
				//				print (readyToFallUnitsInColumn.Count);
				readyToFallColumnGroups.Add (readyToFallUnitsInColumn);
				//				print (readyToFallColumnGroups.Count + "==");
				skyfallColumnIndexes.Add (x);
			}
		}
	}

	void addSkyfallUnits_toFallingColumns (List<List<Unit>> columnGroups)
	{
		//		print ("==================");
		//		print (readyToFallColumnGroups.Count + "==");
		//		foreach (var item in readyToFallColumnGroups) {
		//			print (item.Count);
		//		}
	
		int skyfallStack;
		int fallingColumn;
		int emptyCellsInColumn;
		int skyfall_y;
		int boardHeight = unitsTable.GetLength (1);
		Unit tempU;
	
        for (int n = 0; n < columnGroups.Count; n++) {
			//			print (readyToFallColumnGroups [n].Count);
			skyfallStack = 0;
			fallingColumn = skyfallColumnIndexes [n];
			//			print (fallingColumn);
			emptyCellsInColumn = countEmptyCell_inColumn_onBoard (fallingColumn, unitsTable);
			//			print (emptyCellsInColumn);
			for (int i = 0; i < emptyCellsInColumn; i++) {
				tempU = getReusableUnit ();
				//				print (tempU);
				/*new skyfall unit start from the very top of the board, then up*/
				skyfall_y = boardHeight + skyfallStack;
				/*mark Falling Unit Start and End position Coords, for animation later*/
				tempU.fallFrom = new Vector2Int (fallingColumn, skyfall_y);
				tempU.fallTo = new Vector2Int (fallingColumn, skyfall_y - emptyCellsInColumn);

                columnGroups [n].Add (tempU);
				skyfallStack++;
			}
		}
	}

	IEnumerator unitsFall ()
	{
		//		print ("==================");
		//		print (readyToFallColumnGroups.Count + "==");
		//		foreach (var item in readyToFallColumnGroups) {
		//			print (item.Count);
		//			//			print (item [0].CurrentColumn);
		//		}

		totalFallingUnits = 0;
		totalCompletions = 0;
		totalFallingUnits = countAllUnitsInNestedList (readyToFallColumnGroups);
		//		print (totalFallingUnits);

		//TODO: different falling animation, all lowest units from each column fall at once
		foreach (List<Unit> readyToFallUnitsInColumn in readyToFallColumnGroups) {
			while (readyToFallUnitsInColumn.Count > 0) {
				readyToFallUnitsInColumn [0].gameObject.SetActive (true);

				readyToFallUnitsInColumn [0].onFallDone -= updateTableAndTotalCompletions;
				readyToFallUnitsInColumn [0].onFallDone += updateTableAndTotalCompletions;
				readyToFallUnitsInColumn [0].fall ();

				readyToFallUnitsInColumn.RemoveAt (0);

				yield return new WaitForSeconds (fallDelay);
				//yield return new WaitForEndOfFrame ();
			}
		}
	
		while (totalCompletions < totalFallingUnits) {
			yield return new WaitForEndOfFrame ();
		}
	
//		Debug.Log ("falling done!");
		if (onAllFallDone != null) {
			onAllFallDone (originalTable);
		}
	}

	int countAllUnitsInNestedList (List<List<Unit>> nestedList)
	{
		int total = 0;
		foreach (var l in nestedList) {
			foreach (var u in l) {
				total++;
			}
		}
		return total;
	}

	void updateTableAndTotalCompletions (Unit u)
	{
		//		print (u);
		BoardUtilities.update_OneUnitCoord_onTable (u, originalTable);
//		updateCoord_onOriginalTable (u, originalTable);
		totalCompletions++;
		//		print (totalCompletions);
	}

	/*mark inactive units on table as null, then add it to reusableUnits for later use as skyfall units*/
	Unit[,] convertTable (Unit[,] fromT, Unit[,] toT)
	{
		for (int y = 0; y < fromT.GetLength (1); y++) {
			for (int x = 0; x < fromT.GetLength (0); x++) {
				if (fromT [x, y].gameObject.activeSelf) {
					toT [x, y] = fromT [x, y];
				} else {
					toT [x, y] = null;
					reusableUnits.Add (fromT [x, y]);
				}
			}
		}
		return toT;
	}

	int countEmptyCell_inColumn_onBoard (int c, Unit[,] unitsTable)
	{
		int tempN = 0;
		for (int y = 0; y < unitsTable.GetLength (1); y++) {
			if (unitsTable [c, y] == null) {
				tempN++;
			}
		}
		return tempN;
	}

	Unit getReusableUnit ()
	{
		if (reusableUnits.Count > 0) {
			Unit u = reusableUnits [0];
			reusableUnits.RemoveAt (0);
			return u;
		} else {
			Debug.Log ("no more reusable unit available");
			return null;
			//TODO: create a new unit?
		}
	}
}

