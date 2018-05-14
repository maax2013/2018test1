using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFall : MonoBehaviour
{
	Unit[,] unitsTable;
	static int? firstEmpty;
	List<Unit> readyToFallUnits = new List<Unit> ();

	public void fall (Unit[,] units)
	{
//		print ("fall");
		readyToFallUnits.Clear ();
		unitsTable = new Unit[units.GetLength (0), units.GetLength (1)];
		unitsTable = copyTable (units, unitsTable);
		fallUnits_onBoard (unitsTable);
	}

	void fallUnits_onBoard (Unit[,] unitsTable)
	{
		for (int x = 0; x < unitsTable.GetLength (0); x++) {
			firstEmpty = null;
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
		}
		foreach (var u in readyToFallUnits) {
			u.fall ();
		}
	}

	Unit[,] copyTable (Unit[,] fromT, Unit[,] toT)
	{
		for (int y = 0; y < fromT.GetLength (1); y++) {
			for (int x = 0; x < fromT.GetLength (0); x++) {
				if (fromT [x, y].gameObject.activeSelf) {
					toT [x, y] = fromT [x, y];
				} else {
					toT [x, y] = null;
				}
			}
		}
		return toT;
	}
}

