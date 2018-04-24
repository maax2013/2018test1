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
		boardOffX = Mathf.Floor (boardColumns / 2);
		boardOffY = Mathf.Floor (boardRows / 2);

		initBoardBG ();
		initUnits ();
	}

	void initUnits ()
	{
		unitsManager = GetComponent<UnitsManager> ();
		unitsManager.createUnitsByRowColumn (boardRows, boardColumns);
		unitsManager.repositionUnitsHolder (-boardOffX, boardOffY, 0f);
	}

	void initBoardBG ()
	{
		boardBgCtr = GetComponent<BoardBg> ();
		boardBgCtr.createBoardTilesByRowColumn (boardRows, boardColumns);
		boardBgCtr.repositionBoard (-boardOffX, boardOffY, 1f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
