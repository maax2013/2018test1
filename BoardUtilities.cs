using System.Collections.Generic;
using UnityEngine;

public static class BoardUtilities
{
	public static void switchUnitsCoord (Unit u1, Unit u2, Unit[,] table)
	{
		table [u1.CurrentColumn, u1.CurrentRow] = u2;
		table [u2.CurrentColumn, u2.CurrentRow] = u1;

		int tempColumn = u1.CurrentColumn;
		int tempRow = u1.CurrentRow;
		u1.setUnitCoord (u2.CurrentColumn, u2.CurrentRow);
		u2.setUnitCoord (tempColumn, tempRow);
	}

	public static void remove_UnitsInSmallList_FromLargeList (List<Unit> smallL, List<Unit> largeL)
	{
		Unit[] tempGroup = smallL.ToArray ();
		foreach (var tempU in tempGroup) {
			tryRemoveFromGroup (tempU, largeL);
		}
	}

	public static void remove_UnitsInSmallList_FromLargeList (Unit[] smallL, List<Unit> largeL)
	{
		foreach (var tempU in smallL) {
			tryRemoveFromGroup (tempU, largeL);
		}
	}

	public static void tryAddToGroup (Unit u, List<Unit> group)
	{
		if (!group.Contains (u)) {
			group.Add (u);
		}
	}

	public static void tryRemoveFromGroup (Unit u, List<Unit> group)
	{
		if (group.Contains (u)) {
			group.Remove (u);
		}
	}

	public static Unit[] getmatch4Units_towards_onTable (Unit u, Vector2Int diagonal, Unit[,]unitsTable)
	{
		Unit linkedUnit_corner = BoardUtilities.getSameIdUnit_Towards_onTable (u, diagonal, unitsTable);
		Unit linkedUnit_side1 = BoardUtilities.getSameIdUnit_Towards_onTable (u, new Vector2Int (diagonal.x, 0), unitsTable);
		Unit linkedUnit_side2 = BoardUtilities.getSameIdUnit_Towards_onTable (u, new Vector2Int (0, diagonal.y), unitsTable);


		if (linkedUnit_corner && linkedUnit_side1 && linkedUnit_side2) {
			/*make sure no overlaps*/
			if (linkedUnit_corner.BelongingBlocks < 1 &&
			    linkedUnit_side1.BelongingBlocks < 1 &&
			    linkedUnit_side2.BelongingBlocks < 1) {
				/*make sure to put self in the first slot*/
				Unit[] match4Group = new Unit[] { u, linkedUnit_corner, linkedUnit_side1, linkedUnit_side2 };
				return match4Group;
			}
		}
		return null;
	}

	public static Unit getSameIdUnit_Towards_onTable (Unit u1, Vector2Int direction, Unit[,]table)
	{
		Unit tempUnit = BoardUtilities.getUnitOnTable (u1.CurrentColumn + direction.x, u1.CurrentRow + direction.y, table);
		if (tempUnit) {
			if (BoardUtilities.hasSameID (u1, tempUnit)) {
				return tempUnit;
			}
		}
		return null;
	}

	public static Unit getUnitOnTable (int column, int row, Unit[,]unitsTable)
	{
		if (-1 < column && column < unitsTable.GetLength (0)
		    && -1 < row && row < unitsTable.GetLength (1)) {
			return unitsTable [column, row];
		} else {
			return null;
		}
	}

	public static bool hasSameID (Unit u1, Unit u2)
	{
        if (u1.GetUnitID() == u2.GetUnitID()) {
			return true;
		} else {
			return false;
		}
	}

	public static List<Unit> getAllUnitsList_onTable (Unit[,]unitsTable)
	{
		List<Unit> allUnitsList = new List<Unit> ();
		foreach (var u in unitsTable) {
			allUnitsList.Add (u);
		}
		return allUnitsList;
	}

	public static void update_OneUnitCoord_onTable (Unit u, Unit[,] table)
	{
		int coordX = u.CurrentColumn;
		int coordY = u.CurrentRow;
		table [coordX, coordY] = u;
	}

	public static string[,] flipStringTableCoord (string[,] originalT)
	{
		/*flip the inner and outer array, so the table has same coord as the gameboard*/
        /*flip y axis to match unity coord*/
		string[,] targetT = new string[originalT.GetLength (1), originalT.GetLength (0)];
		int column = targetT.GetLength (0);
		int row = targetT.GetLength (1);
		for (int c = 0; c < column; c++) {
			for (int r = 0; r < row; r++) {
				targetT [c, r] = originalT [row-1-r, c];
			}
		}
		return targetT;
	}

    public static bool pointerInsideBoundary(Vector3 p, Bounds boundary)
    {
        if (boundary.Contains(p))
        {
            return true;
        }
        return false;
    }
}
