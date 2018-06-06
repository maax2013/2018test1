using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BoardMatch : MonoBehaviour
{
	public event System.Action<Unit[,]> onAllMatchDone;
	public event System.Action<Unit[,]> onNeedBoardFall;
	public event System.Action<Vector3, float> onNeedShowBlock;

	Unit[,] unitsTable;
	int remainingBlockTasks = 0;

	public void removeAll_match4s_OnBoard_beforeGameStart (Unit[,] table)
	{
		unitsTable = table;
		List<Unit[]> candidatesGroups = new List<Unit[]> ();
		addAllMatch4s_OnBoard_ToGroup (unitsTable, candidatesGroups);
		while (candidatesGroups.Count > 0) {
			resetUnitsInGroup (candidatesGroups);
			candidatesGroups.Clear ();
			addAllMatch4s_OnBoard_ToGroup (unitsTable, candidatesGroups);
		}
		fireAllMatchDone ();
	}

	public void collapseAll_matches_OnBoard (Unit[,] table)
	{
		//		print (boardFall.onAllFallDone);
		unitsTable = table;
		List<Unit[]> candidatesGroups = new List<Unit[]> ();
		addAllMatch4s_OnBoard_ToGroup (unitsTable, candidatesGroups);
		//		print (candidatesGroups.Count);
		if (candidatesGroups.Count > 0) {
			collapseUnitsInGroup (candidatesGroups);
		} else {
			fireAllMatchDone ();
		}
	}

	void collapseUnitsInGroup (List<Unit[]> candidatesGroups)
	{
		List<Unit[]> blockGroups = new List<Unit[]> ();
		List<Unit[]> clearGroups = new List<Unit[]> ();

		for (int i = 0; i < candidatesGroups.Count; i++) {
			Unit[] match4Units = candidatesGroups [i];

			if (match4Units [0].CurrentUnitType.isUpgradable ()) {
				blockGroups.Add (match4Units);
			} else {
				if (match4Units [0].CurrentUnitType.isNoTier ()) {
					clearGroups.Add (match4Units);
				}
				if (match4Units [0].CurrentUnitType.isMaxTier ()) {
					//TODO: what to do with block of max tiered units
				}
			}
		}
		remainingBlockTasks = 0;
		System.Action updateremainingBlockTasks = () => {
			remainingBlockTasks--;
		};
		if (blockGroups.Count > 0) {
			remainingBlockTasks++;
			StartCoroutine (makeBlocks (blockGroups, updateremainingBlockTasks));
		}
		if (clearGroups.Count > 0) {
			remainingBlockTasks++;
			StartCoroutine (clearNoTierBlocks (clearGroups, updateremainingBlockTasks));
		}

		StartCoroutine (waitForBlockTasksDone ());

	}

	IEnumerator waitForBlockTasksDone ()
	{
		while (remainingBlockTasks > 0) {
			yield return new WaitForEndOfFrame ();
		}
		fireNeedBoardFall ();
	}

	IEnumerator clearNoTierBlocks (List<Unit[]> clearGroups, System.Action callback)
	{
		int totalBlocksToMake = clearGroups.Count;
		//		print (totalBlocksToMake);
		int totalCompletion = 0;
		int index = 0;
		float clearBlockTime = 0.3f;//~~~~~~~~~~~~~~~~~~~~~~~~
		float eachDelayTime = 0.2f;//~~~~~~~~~~~~~~~~~~~~~~

		while (index < totalBlocksToMake) {
			StartCoroutine (clearBlock (clearGroups [index], clearBlockTime, () => {
				totalCompletion++;
				//				print (totalCompletion);
			}));
			index++;
			yield return new WaitForSeconds (eachDelayTime);
		}

		while (totalCompletion < totalBlocksToMake) {
			yield return new WaitForEndOfFrame ();
		}
		//TODO: add points to total cash earned
		callback ();
	}

	IEnumerator clearBlock (Unit[] blockUnits, float duration, System.Action callback)
	{
		int totalUnitsToAnimate = blockUnits.Length;
		int totalCompletions = 0;

		System.Action checkTotalCompletions = () => {
			totalCompletions++;
		};

		foreach (var u in blockUnits) {
			u.onJumpDone -= checkTotalCompletions;
			u.onJumpDone += checkTotalCompletions;
			u.jump_overTime_thenGone (duration);
		}
		while (totalCompletions < totalUnitsToAnimate) {
			yield return new WaitForEndOfFrame ();
		}
		callback ();
	}

	IEnumerator makeBlocks (List<Unit[]> blockGroups, System.Action callback)
	{
		int totalBlocksToMake = blockGroups.Count;
		//		print (totalBlocksToMake);
		int totalCompletion = 0;
		int index = 0;
		float upgradeBlockTime = 0.8f;//~~~~~~~~~~~~~~~~~~~~~~~~
		float eachDelayTime = 0.2f;//~~~~~~~~~~~~~~~~~~~~~~

		while (index < totalBlocksToMake) {
			StartCoroutine (upgradeBlock (blockGroups [index], upgradeBlockTime, () => {
				totalCompletion++;
				//				print (totalCompletion);
			}));
			index++;
			yield return new WaitForSeconds (eachDelayTime);
		}

		while (totalCompletion < totalBlocksToMake) {
			yield return new WaitForEndOfFrame ();
		}

		callback ();
	}



	IEnumerator upgradeBlock (Unit[] blockUnits, float duration, System.Action callback)
	{
		var rdmN = Random.Range (0, blockUnits.Length);
		Unit targetU = blockUnits [rdmN];
		List<Unit> otherUnits = new List<Unit> ();

		for (int i = 0; i < blockUnits.Length; i++) {
			if (i != rdmN) {
				otherUnits.Add (blockUnits [i]);
			}
		}

		int totalUnitsToAnimate = blockUnits.Length;
		int totalCompletions = 0;

		System.Action checkTotalCompletions = () => {
			totalCompletions++;
		};

		float mergeTime = duration * 0.5f;
		float popTime = duration * 0.5f;

		foreach (var u in otherUnits) {
            if(u.UMerge){
                //u.UMerge.OnMergeDone -= checkTotalCompletions;
                u.UMerge.OnMergeDone += checkTotalCompletions;
                //u.UMerge.OnMergeDone -= u.HandleOnMergeDone;
                u.UMerge.OnMergeDone += u.HandleOnMergeDone;
                u.UMerge.mergeTo_overTime(targetU.transform.localPosition, mergeTime);
            }else
            {
                throw new System.Exception("can't find UnitMerge component");
            }
			//u.onMergeDone -= checkTotalCompletions;
			//u.onMergeDone += checkTotalCompletions;
			//u.mergeTo_overTime_thenGone (targetU.transform.localPosition, mergeTime);
		}
		/*since the first unit in blockUnits is the one at the bottom left, so the position of the block will be its x + half with, y + half height*/
		Vector3 bottomLeftU = blockUnits [0].transform.localPosition;
		Vector3 blockPos = new Vector3 (bottomLeftU.x + 0.5f, bottomLeftU.y + 0.5f, 0);
		fireNeedShowBlock (blockPos, mergeTime + popTime);
//		blockCtr.showBlockAt_overTime (blockPos, mergeTime + popTime);

		yield return new WaitForSeconds (mergeTime);

		targetU.upgrade (1);
		//		targetU.testMark (true);//----------------------------
        if(targetU.UMerge){
            //targetU.UMerge.OnPopDone -= checkTotalCompletions;
            targetU.UMerge.OnPopDone += checkTotalCompletions;
            //targetU.UMerge.OnPopDone -= targetU.HandleOnPopDone;
            targetU.UMerge.OnPopDone += targetU.HandleOnPopDone;
            targetU.UMerge.popSprite_overTime(popTime);
        }else{
            throw new System.Exception("can't find UnitMerge component");
        }
		//targetU.onMergeDone -= checkTotalCompletions;
		//targetU.onMergeDone += checkTotalCompletions;
		//targetU.popSprite_overTime (popTime);

		//		yield return new WaitForSeconds (popTime + Time.deltaTime);
		while (totalCompletions < totalUnitsToAnimate) {
			yield return new WaitForEndOfFrame ();
		}
		callback ();
	}


	void addAllMatch4s_OnBoard_ToGroup (Unit[,] uTable, List<Unit[]> candidatesGroups)
	{
		List<Unit> allUnitsList = BoardUtilities.getAllUnitsList_onTable (uTable);

		while (allUnitsList.Count > 0) {
			Unit tempU = allUnitsList [0];
			Unit[] match4s_TopRight = BoardUtilities.getmatch4Units_towards_onTable (tempU, new Vector2Int (1, 1), uTable);
			if (match4s_TopRight != null) {
				candidatesGroups.Add (match4s_TopRight);
				foreach (Unit U in match4s_TopRight) {
					//					U.testMark (true);//----------------------------
					U.BelongingBlocks++;
				}
				BoardUtilities.remove_UnitsInSmallList_FromLargeList (match4s_TopRight, allUnitsList);
			} else {
				BoardUtilities.tryRemoveFromGroup (tempU, allUnitsList);
			}
		}
	}

	void resetUnitsInGroup (List<Unit[]> candidatesGroups)
	{
		for (int i = 0; i < candidatesGroups.Count; i++) {
			Unit[] match4Units = candidatesGroups [i];
			foreach (Unit u in match4Units) {
				u.reset ();
//				u.testMark (false);//-----------------
			}
		}
	}

	void fireAllMatchDone ()
	{
		if (onAllMatchDone != null) {
			onAllMatchDone (unitsTable);
		}
	}

	void fireNeedBoardFall ()
	{
		if (onNeedBoardFall != null) {
			onNeedBoardFall (unitsTable);
		}
	}

	void fireNeedShowBlock (Vector3 pos, float duration)
	{
		if (onNeedShowBlock != null) {
			onNeedShowBlock (pos, duration);
		}
	}
}

