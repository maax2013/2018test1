using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBg : MonoBehaviour
{
	[SerializeField] GameObject cellBg;
	[SerializeField] Sprite[] sprites;

	[SerializeField] Transform tileBgHolder;

	GameObject tempTileBG;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void createBoardTiles_ByRowColumn (int row, int column)
	{
		for (int r = 0; r < row; r++) {
			for (int c = 0; c < column; c++) {
				tempTileBG = Instantiate (cellBg, new Vector3 (c, r, 0), Quaternion.identity) as GameObject;
				if ((r % 2 == 0 && c % 2 == 0) || (r % 2 != 0 && c % 2 != 0)) {
					cellBg.GetComponent<SpriteRenderer> ().sprite = sprites [0];
				} else {
					cellBg.GetComponent<SpriteRenderer> ().sprite = sprites [1];
				}
				tempTileBG.transform.SetParent (tileBgHolder, false);
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
