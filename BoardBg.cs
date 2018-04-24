using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBg : MonoBehaviour
{
	[SerializeField] GameObject cellBg1Prefab;
	[SerializeField] GameObject cellBg2Prefab;

	[SerializeField] Transform tileBgHolder;

	GameObject tempTileBG;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void createBoardTilesByRowColumn (int row, int column)
	{
		for (int r = 0; r < row; r++) {
			for (int c = 0; c < column; c++) {
				if ((r % 2 == 0 && c % 2 == 0) || (r % 2 != 0 && c % 2 != 0)) {
					tempTileBG = Instantiate (cellBg1Prefab, new Vector3 (c, -r, 0), Quaternion.identity) as GameObject;
				} else {
					tempTileBG = Instantiate (cellBg2Prefab, new Vector3 (c, -r, 0), Quaternion.identity) as GameObject;
				}
				tempTileBG.transform.parent = tileBgHolder;
			}
		}
	}

	public void repositionBoard (float x, float y, float z)
	{
		tileBgHolder.position = new Vector3 (x, y, z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
