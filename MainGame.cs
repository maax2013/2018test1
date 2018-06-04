//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
	[SerializeField] CountDownTimeBar cdTimer;
	[SerializeField] int boardColumns = 7;
	[SerializeField] int boardRows = 8;
	float boardOffX, boardOffY;

	UnitsManager unitsManager;
	BoardBg boardBgCtr;
    string[,] boardLayout;
    string[,] blueprint;

	// Use this for initialization
	void Start ()
	{
        boardLayout = GetComponent<BoardLayout>().GetBoardLayout();
        boardColumns = boardLayout.GetLength(0);
        boardRows = boardLayout.GetLength(1);
		boardOffX = (boardColumns - 1) / 2f;
		boardOffY = (boardRows - 1) / 2f;

        blueprint = GetComponent<Blueprint>().getBlueprint();
        ValidateBoardLayout_withBlueprint(boardLayout, blueprint);

		cdTimer.gameObject.transform.localPosition = new Vector3 (0f, boardOffY + 1f, 0f);
		cdTimer.gameObject.SetActive (false);

		InitBoardBG ();
		InitUnits ();
	}
    void ValidateBoardLayout_withBlueprint(string[,] layout, string[,] bp)
    {
        if (bp.GetLength(0) != boardColumns || bp.GetLength(1) != boardRows)
            throw new System.Exception("blueprint has different size than the board layout");
        for (int c = 0; c < boardColumns; c++)
        {
            for (int r = 0; r < boardRows; r++)
            {
                if (layout[c, r] == SpecialType.Empty.ToString() && bp[c, r] != null)
                {
                    throw new System.Exception("blueprint is invalid on this game board");
                }
            }
        }
    }

	void InitBoardBG ()
	{
		boardBgCtr = GetComponent<BoardBg> ();
        //boardBgCtr.createBoardTiles_ByRowColumn (boardColumns, boardRows);
        boardBgCtr.createBoardTiles_FromLayout_andBlueprint(boardLayout, blueprint);

		boardBgCtr.repositionBoard (-boardOffX, -boardOffY, 2f);

        //boardBgCtr.applyBlueprint(blueprint);
	}

	void InitUnits ()
	{
		unitsManager = GetComponent<UnitsManager> ();
		unitsManager.init ();
		unitsManager.createUnits_ByRowColumn (boardColumns, boardRows);

		unitsManager.repositionBlocksHolder (-boardOffX, -boardOffY, 1f);
		unitsManager.repositionUnitsHolder (-boardOffX, -boardOffY, 0f);
        unitsManager.initBoardSwapUnits (cdTimer);
        unitsManager.passBlueprint(blueprint);

//		unitsManager.collapseAll_matches_OnBoard ();
		unitsManager.removeAll_match4s_OnBoard_beforeGameStart ();
//		unitsManager.markAll_linkedUnitsGroups ();
		//		unitsManager.checkMatch4s_OnBoard ();
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
