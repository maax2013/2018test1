using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFall : MonoBehaviour
{
	public event System.Action onAllFallDone;

	Unit[,] originalTable;
	Unit[,] unitsTable;
	static int? firstEmpty;

	List<List<Unit>> readyToFallGroups = new List<List<Unit>> ();
	List<Unit> readyToFallUnits = new List<Unit> ();
	List<int> skyfallColumns = new List<int> ();
	List<Unit> reusableUnits = new List<Unit> ();

	int totalFallingUnits = 0;
	int totalCompletions = 0;

	public void fall (Unit[,] units)
	{
//		print ("fall");
		originalTable = units;

		readyToFallGroups.Clear ();
		reusableUnits.Clear ();
		skyfallColumns.Clear ();

		unitsTable = new Unit[units.GetLength (0), units.GetLength (1)];
		unitsTable = copyTable (units, unitsTable);

		markFallingUnits_onBoard (unitsTable);
		addSkyfallUnits (readyToFallGroups);
		StartCoroutine (unitsFall ());
	}

	void markFallingUnits_onBoard (Unit[,] unitsTable)
	{
		for (int x = 0; x < unitsTable.GetLength (0); x++) {
			firstEmpty = null;
			readyToFallUnits = new List<Unit> ();

			for (int y = 0; y < unitsTable.GetLength (1); y++) {
				if (unitsTable [x, y] == null && !firstEmpty.HasValue) {
					firstEmpty = y;
				} else if (firstEmpty.HasValue && unitsTable [x, y] != null) {
					Unit fallingU = unitsTable [x, y];
					fallingU.fallFrom = new Vector2Int (x, y);
					fallingU.fallTo = new Vector2Int (x, firstEmpty.Value);
					readyToFallUnits.Add (fallingU);

					unitsTable [x, firstEmpty.Value] = unitsTable [x, y];
					unitsTable [x, y] = null;
					firstEmpty++;
				}
			}
			if (firstEmpty.HasValue) {
//				print (readyToFallUnits.Count);
				readyToFallGroups.Add (readyToFallUnits);
//				print (readyToFallGroups.Count + "==");
				skyfallColumns.Add (x);
			}
		}
	}

	void addSkyfallUnits (List<List<Unit>> readyToFallGroups)
	{
//		print ("==================");
//		print (readyToFallGroups.Count + "==");
//		foreach (var item in readyToFallGroups) {
//			print (item.Count);
//		}

		int skyfallStack;
		int fallingColumn;
		int emptyCellsInColumn;
		int skyfall_y;
		int boardHeight = unitsTable.GetLength (1);
		Unit tempU;

		for (int n = 0; n < readyToFallGroups.Count; n++) {
//			print (readyToFallGroups [n].Count);
			skyfallStack = 0;
			fallingColumn = skyfallColumns [n];
//			print (fallingColumn);
			emptyCellsInColumn = getEmptyCellCount_inColumn (fallingColumn, unitsTable);
//			print (emptyCellsInColumn);
			for (int i = 0; i < emptyCellsInColumn; i++) {
				tempU = getReusableUnit ();
//				print (tempU);
//				tempU.reset ();
				skyfall_y = boardHeight + skyfallStack;
				tempU.fallFrom = new Vector2Int (fallingColumn, skyfall_y);
				tempU.fallTo = new Vector2Int (fallingColumn, skyfall_y - emptyCellsInColumn);
				readyToFallGroups [n].Add (tempU);
				skyfallStack++;
			}
		}
	}

	IEnumerator unitsFall ()
	{
//		print ("==================");
//		print (readyToFallGroups.Count + "==");
//		foreach (var item in readyToFallGroups) {
//			print (item.Count);
//			//			print (item [0].CurrentColumn);
//		}
		totalFallingUnits = 0;
		totalCompletions = 0;
		totalFallingUnits = countAllUnitsInNestedList (readyToFallGroups);
//		print (totalFallingUnits);
		foreach (var readyToFallUnits in readyToFallGroups) {
			while (readyToFallUnits.Count > 0) {
				readyToFallUnits [0].gameObject.SetActive (true);
				readyToFallUnits [0].onFallDone += checkTotalCompletions;
				readyToFallUnits [0].fall ();
				readyToFallUnits.RemoveAt (0);
				//			yield return new WaitForSeconds (0.05f);
				yield return new WaitForEndOfFrame ();
			}
		}

		while (totalCompletions < totalFallingUnits) {
			yield return new WaitForEndOfFrame ();
		}

		print ("falling done!");
		if (onAllFallDone != null) {
			onAllFallDone ();
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

	void checkTotalCompletions (Unit u)
	{
//		print (u);
		updateCoord_onOriginalTable (u, originalTable);
		totalCompletions++;
//		print (totalCompletions);
	}

	void updateCoord_onOriginalTable (Unit u, Unit[,] originalTable)
	{
		int coordX = u.CurrentColumn;
		int coordY = u.CurrentRow;
		originalTable [coordX, coordY] = u;
	}

	Unit[,] copyTable (Unit[,] fromT, Unit[,] toT)
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

	int getEmptyCellCount_inColumn (int c, Unit[,] unitsTable)
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
			return null;
			//TODO: create a new unit?
		}
	}
}

