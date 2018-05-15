using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFall : MonoBehaviour
{
	Unit[,] unitsTable;
	static int? firstEmpty;

	List<List<Unit>> readyToFallGroups = new List<List<Unit>> ();
	List<Unit> readyToFallUnits = new List<Unit> ();
	List<int> skyfallColumns = new List<int> ();
	List<Unit> reusableUnits = new List<Unit> ();

	public void fall (Unit[,] units)
	{
//		print ("fall");
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
////		foreach (var item in readyToFallGroups) {
////			print (item);
//////			print (item [0].CurrentColumn);
////		}

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
				tempU.reset ();
				skyfall_y = boardHeight + skyfallStack;
				tempU.fallFrom = new Vector2Int (fallingColumn, skyfall_y);
				tempU.fallTo = new Vector2Int (fallingColumn, skyfall_y - emptyCellsInColumn);
				readyToFallGroups [n].Add (tempU);
				skyfallStack++;
			}
		}

//		foreach (var columnUnits in readyToFallGroups) {
//			skyfallStack = 0;
//			fallingColumn = columnUnits [0].CurrentColumn;
//			print (fallingColumn);
//			emptyCellsInColumn = getEmptyCellCount_inColumn (fallingColumn, unitsTable);
////			print (emptyCellsInColumn);
//			for (int i = 0; i < emptyCellsInColumn; i++) {
////				tempU = getReusableUnit ();
////				print (tempU);
////				tempU.reset ();
////				skyfall_y = boardHeight + skyfallStack;
////				tempU.fallFrom = new Vector2Int (fallingColumn, skyfall_y);
////				tempU.fallTo = new Vector2Int (fallingColumn, skyfall_y - emptyCellsInColumn);
////				columnUnits.Add (tempU);
////				skyfallStack++;
//			}
//		}
	}

	IEnumerator unitsFall ()
	{
//		print ("==================");
//		print (readyToFallGroups.Count + "==");
//		foreach (var item in readyToFallGroups) {
//			print (item.Count);
//			//			print (item [0].CurrentColumn);
//		}

		float elapsedTime = 0;
		foreach (var readyToFallUnits in readyToFallGroups) {
			while (readyToFallUnits.Count > 0) {
				readyToFallUnits [0].gameObject.SetActive (true);
				readyToFallUnits [0].fall ();
				readyToFallUnits.RemoveAt (0);
				//			yield return new WaitForSeconds (0.05f);
				yield return new WaitForEndOfFrame ();
			}
		}

		yield return new WaitForSeconds (0.05f);
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

