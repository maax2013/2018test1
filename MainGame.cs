using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
	int boardColumns = 7;
	int boardRows = 8;
	float boardOffX, boardOffY;

	UnitsManager unitsManager;
	BoardBg boardBgCtr;

	// Use this for initialization
	void Start ()
	{
		boardOffX = (boardColumns - 1) / 2f;
		boardOffY = (boardRows - 1) / 2f;

		initBoardBG ();
		initUnits ();
	}

	void initBoardBG ()
	{
		boardBgCtr = GetComponent<BoardBg> ();
		boardBgCtr.createBoardTiles_ByRowColumn (boardRows, boardColumns);
		boardBgCtr.repositionBoard (-boardOffX, -boardOffY, 1f);
	}

	void initUnits ()
	{
		unitsManager = GetComponent<UnitsManager> ();
		unitsManager.createUnits_ByRowColumn (boardRows, boardColumns);
		unitsManager.repositionUnitsHolder (-boardOffX, -boardOffY, 0f);
		unitsManager.group_ConnectedUnits_OnBoard ();
		//		unitsManager.checkMatch4s_OnBoard ();
		unitsManager.switch_BoardTouchable (true);
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
