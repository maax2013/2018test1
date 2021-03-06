﻿//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	[SerializeField] CountDownTimeBar cdTimer;
    int boardColumns, boardRows;
	float boardOffX, boardOffY;

	UnitsManager unitsManager;
	BoardBg boardBgCtr;
    BoardLayout boardLayout;
    GameObject[,] boardLayoutBgs;
    GameObject[,] boardLayoutUnits;
    string[,] blueprint;
    //TODO: make blueprint a interface or subclass

	// Use this for initialization
	void Start ()
	{
        boardLayout = GetComponent<BoardLayout>();
        boardLayout.initBoardLayout();
        boardLayoutBgs = boardLayout.GetBoardLayoutBg();
        boardColumns = boardLayoutBgs.GetLength(0);
        boardRows = boardLayoutBgs.GetLength(1);
		boardOffX = (boardColumns - 1) / 2f;
		boardOffY = (boardRows - 1) / 2f;

        blueprint = GetComponent<Blueprint>().getBlueprint();
        //ValidateBoardLayout_withBlueprint(boardLayout, blueprint);

		cdTimer.gameObject.transform.localPosition = new Vector3 (0f, boardOffY + 1f, 0f);
		cdTimer.gameObject.SetActive (false);

		InitBoardBG ();
		//InitUnits ();
	}
    //void ValidateBoardLayout_withBlueprint(string[,] layout, string[,] bp)
    //{
    //    if (bp.GetLength(0) != boardColumns || bp.GetLength(1) != boardRows)
    //        throw new System.Exception("blueprint has different size than the board layout");
    //    for (int c = 0; c < boardColumns; c++)
    //    {
    //        for (int r = 0; r < boardRows; r++)
    //        {
    //            if (layout[c, r] == SpecialType.Empty.ToString() && bp[c, r] != null)
    //            {
    //                throw new System.Exception("blueprint is invalid on this game board");
    //            }
    //        }
    //    }
    //}

	void InitBoardBG ()
	{
		boardBgCtr = GetComponent<BoardBg> ();
        //boardBgCtr.createBoardTiles_ByRowColumn (boardColumns, boardRows);
        boardBgCtr.CreateBoardTiles_FromLayout_andBlueprint(boardLayoutBgs, blueprint);

		boardBgCtr.RepositionBgHolder (-boardOffX, -boardOffY, 2f);

        //boardBgCtr.applyBlueprint(blueprint);
	}

	void InitUnits ()
	{
		unitsManager = GetComponent<UnitsManager> ();
		unitsManager.InitBoard ();
        //unitsManager.createUnits_ByRowColumn (boardColumns, boardRows);
        //unitsManager.createUnits__FromLayout(boardLayout);

		unitsManager.repositionBlocksHolder (-boardOffX, -boardOffY, 1f);
		unitsManager.repositionUnitsHolder (-boardOffX, -boardOffY, 0f);

//        unitsManager.initBoardSwapUnits (cdTimer);
//        unitsManager.passBlueprint(blueprint);

////		unitsManager.collapseAll_matches_OnBoard ();
//		unitsManager.removeAll_match4s_OnBoard_beforeGameStart ();
////		unitsManager.markAll_linkedUnitsGroups ();
		////		unitsManager.checkMatch4s_OnBoard ();
	}

}
