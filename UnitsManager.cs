﻿//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
	[SerializeField] GameObject unitPrefab;
	[SerializeField] Transform unitsHolder;

	Block blockCtr;
	BoardFall boardFall;
	BoardMatch boardMatch;
    BoardSwapUnits boardSwapUnits;
	AllUnitTypes allTypes;

	Unit_Base tempUnit;

	Unit[] allUnitsOnBoard;
	int tempUnitIndex;

    Unit_Base[,] unitsTable;

	public void InitBoard ()
	{
		blockCtr = GetComponent<Block> ();
		boardMatch = GetComponent<BoardMatch> ();
		boardFall = GetComponent<BoardFall> ();
        boardSwapUnits = GetComponent<BoardSwapUnits>();
		allTypes = GetComponent<AllUnitTypes> ();
	}


	//public void createUnits_ByRowColumn (int column, int row)
	//{
	//	unitsTable = new Unit_Base[column, row];

	//	/*create units table, from bottom left, to top right, column after column.*/
	//	for (int c = 0; c < column; c++) {
	//		for (int r = 0; r < row; r++) {
	//			GameObject tempObj = Instantiate (unitPrefab, new Vector3 (c, r, 0), Quaternion.identity) as GameObject;
	//			tempObj.transform.SetParent (unitsHolder, false);

	//			tempUnit = tempObj.GetComponent<Unit_Base> ();
 //               tempUnit.InitNormalUnit (c, r, allTypes);

	//			unitsTable [c, r] = tempUnit;
	//		}
	//	}
	//}

    public void createUnits__FromLayout(GameObject[,] boardLayout)
    {
        int column = boardLayout.GetLength(0);
        int row = boardLayout.GetLength(1);
        unitsTable = new Unit_Base[column, row];
        GameObject tempObj;

        /*create units table, from bottom left, to top right, column after column.*/
        for (int c = 0; c < column; c++)
        {
            for (int r = 0; r < row; r++)
            {
                if (boardLayout[c, r] == null){
                    tempObj = Instantiate(unitPrefab, new Vector3(c, r, 0), Quaternion.identity) as GameObject;
                    //tempObj.transform.SetParent(unitsHolder, false);

                    tempUnit = tempObj.GetComponent<Unit_Base>();
                    //tempUnit.InitNormalUnit(c, r, allTypes);//++++++++++++++++++++++++

                    //unitsTable[c, r] = tempUnit;
                }else{
                    tempObj = Instantiate(boardLayout[c,r], new Vector3(c, r, 0), Quaternion.identity) as GameObject;
                }
                tempObj.transform.SetParent(unitsHolder, false);
                unitsTable[c, r] = tempUnit;
            }
        }
    }

	public void repositionUnitsHolder (float x, float y, float z)
	{
		unitsHolder.position = new Vector3 (x, y, z);
	}

	public void repositionBlocksHolder (float x, float y, float z)
	{
		blockCtr.repositionBlocksHolder (x, y, z);
	}

//	public void initBoardSwapUnits (CountDownTimeBar cdt)
//	{
//        boardSwapUnits.InitBoardSwap(unitsTable);
//		boardSwapUnits.passCDTimer(cdt);
//	}
//    public void passBlueprint(string[,] bp){
//        BoardCompose.SetBlueprint(bp);
//    }

//    //public void addEtraSwappingTime(float t){
//    //    boardSwapUnits.addTempEtraSwappingTime(t);
//    //}






//	public void switch_GUITouchable (bool on)
//	{
//        //TODO: GUI touchable
//	}

//    public void enableBoardDragging(){
//        boardSwapUnits.onAllSwapsDone -= handleOnAllSwapsDone;
//        boardSwapUnits.onAllSwapsDone += handleOnAllSwapsDone;
//        boardSwapUnits.enableDragging();
//    }
//    public void disableBoardDragging(){
//        boardSwapUnits.disableDragging();
//    }

//    void handleOnAllSwapsDone(){
//        collapseAll_matches_OnBoard();
//    }





//	public void removeAll_match4s_OnBoard_beforeGameStart ()
//	{
//        disableBoardDragging ();
//		boardMatch.removeAll_match4s_OnBoard_beforeGameStart (unitsTable);
//		readyForInteraction ();
//	}

//	public void collapseAll_matches_OnBoard ()
//	{
//        disableBoardDragging ();
//		boardMatch.onAllMatchDone -= handleOnAllMatchDone;
//		boardMatch.onAllMatchDone += handleOnAllMatchDone;

//		boardMatch.onNeedShowBlock -= handleOnNeedShowBlock;
//		boardMatch.onNeedShowBlock += handleOnNeedShowBlock;

//		boardMatch.onNeedBoardFall -= handleOnNeedBoardFall;
//		boardMatch.onNeedBoardFall += handleOnNeedBoardFall;

//		boardMatch.collapseAll_matches_OnBoard (unitsTable);
//	}

//	void handleOnAllMatchDone (Unit[,] updatedTable)
//	{
//		unitsTable = updatedTable;
//		//		debugBoard ();
//        if(BoardCompose.CompositionDone_onBoard(unitsTable)){
//            //TODO: level complete
//            Debug.Log("levle complete!!!!!");
//        }else{
//            readyForInteraction();
//        }
		
//	}

//	void handleOnNeedBoardFall (Unit[,] updatedTable)
//	{
//		unitsTable = updatedTable;
//		//		debugBoard ();
//		boardFall.onAllFallDone -= handleOnAllFallDone;
//		boardFall.onAllFallDone += handleOnAllFallDone;
//		boardFall.fall (unitsTable);
//	}

//	void handleOnAllFallDone (Unit[,] updatedTable)
//	{
//		unitsTable = updatedTable;
//		//		debugBoard ();
//		boardMatch.collapseAll_matches_OnBoard (unitsTable);
//	}

//	void handleOnNeedShowBlock (Vector3 targetP, float duration)
//	{
//		blockCtr.showBlockAt_overTime (targetP, duration);
//	}

//	void readyForInteraction ()
//	{
////		print ("ready!");
//        enableBoardDragging ();
//	}












//	void OnGUI ()
//	{
//		if (GUI.Button (new Rect (0, 0, 200, 55), "test")) {
//			stepCheck ();
//		}
//	}

//	void stepCheck ()
//	{
////			checkConnections_ToRightAndTop (allUnitsOnBoard [tempUnitIndex]);
////			tempUnitIndex++;

////		unitsToCheck [0].testMark (true);
////		get_LinkedUnitsGroup_of (unitsToCheck [0]);
//		//collapseAll_matches_OnBoard ();
//        if (Time.timeScale > 0f)
//        {
//            Time.timeScale = 0f;
//        }
//        else
//        {
//            Time.timeScale = 1f;
//        }
//	}

//	void debugBoard ()
//	{
//		for (int c = 0; c < unitsTable.GetLength (0); c++) {
//			for (int r = 0; r < unitsTable.GetLength (1); r++) {
////				unitsTable [c, r].debugText (c.ToString () + ":" + r.ToString ());
	//			//				unitsTable [c, r].showDebugCoord();
	//			unitsTable [c, r].debugText (unitsTable [c, r].BelongingBlocks.ToString ());
	//		}
	//	}
	//}
	


}
