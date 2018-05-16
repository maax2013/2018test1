using System.Collections.Generic;
using UnityEngine;

public class MakeBlocksWhenDraging
{
	//----------------
	Unit[,] unitsTable;
	//----------------

	void tryMakeBlock (Unit u)
	{
		List<Unit[]> surroundingMatch4s = getSurroundingMatch4s (u);

		if (surroundingMatch4s.Count > 0) {

		}
	}

	bool linkToExistingBlock (Unit[] group)
	{
		int totalLinksToOtherBlocks = 0;
		/*loop start with the second item, since the first one is the unit just switched over*/
		for (int i = 1; i < group.Length; i++) {
			if (group [i].blockGroup.Count > 0) {
				totalLinksToOtherBlocks++;
			}
		}
		if (totalLinksToOtherBlocks > 0) {
			return true;
		} else {
			return false;
		}
	}

	void makeNewBlock (Unit[] group)
	{
		List<Unit> blockGroup = new List<Unit> ();
		blockGroup.AddRange (group);
		foreach (Unit u in group) {
			u.blockGroup = blockGroup;
		}
		//+++++++++++block image
	}

	void tryMakeBiggerBlock ()
	{

	}

	List<Unit[]> getSurroundingMatch4s (Unit u)
	{
		List<Unit[]> candidatesGroups = new List<Unit[]> ();

		Unit[] topRightGroup = BoardUtilities.getmatch4Units_towards_onTable (u, new Vector2Int (1, 1), unitsTable);
		Unit[] topleftGroup = BoardUtilities.getmatch4Units_towards_onTable (u, new Vector2Int (-1, 1), unitsTable);
		Unit[] bottomRightGroup = BoardUtilities.getmatch4Units_towards_onTable (u, new Vector2Int (1, -1), unitsTable);
		Unit[] bottomLeftGroup = BoardUtilities.getmatch4Units_towards_onTable (u, new Vector2Int (-1, -1), unitsTable);

		if (topRightGroup != null)
			candidatesGroups.Add (topRightGroup);
		if (topleftGroup != null)
			candidatesGroups.Add (topleftGroup);
		if (bottomRightGroup != null)
			candidatesGroups.Add (bottomRightGroup);
		if (bottomLeftGroup != null)
			candidatesGroups.Add (bottomLeftGroup);

		return candidatesGroups;
	}

}


