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
    string[,] blueprint;

	// Use this for initialization
	void Start ()
	{
		boardOffX = (boardColumns - 1) / 2f;
		boardOffY = (boardRows - 1) / 2f;

		cdTimer.gameObject.transform.localPosition = new Vector3 (0f, boardOffY + 1f, 0f);
		cdTimer.gameObject.SetActive (false);

        blueprint = GetComponent<Blueprint>().getBlueprint();

		initBoardBG ();
		initUnits ();
	}

	void initBoardBG ()
	{
		boardBgCtr = GetComponent<BoardBg> ();
		boardBgCtr.createBoardTiles_ByRowColumn (boardColumns, boardRows);

		boardBgCtr.repositionBoard (-boardOffX, -boardOffY, 2f);

        boardBgCtr.applyBlueprint(blueprint);
	}

	void initUnits ()
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
