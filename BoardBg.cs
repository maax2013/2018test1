using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBg : MonoBehaviour
{
	[SerializeField] GameObject cellBgPrefab;
	[SerializeField] Sprite[] sprites;

	[SerializeField] Transform tileBgHolder;

	GameObject tempTileBG;

	public void createBoardTiles_ByRowColumn (int column, int row)
	{
		for (int c = 0; c < column; c++) {
			for (int r = 0; r < row; r++) {
				tempTileBG = Instantiate (cellBgPrefab, new Vector3 (c, r, 0), Quaternion.identity) as GameObject;
				if ((r % 2 == 0 && c % 2 == 0) || (r % 2 != 0 && c % 2 != 0)) {
					tempTileBG.GetComponent<SpriteRenderer> ().sprite = sprites [0];
				} else {
					tempTileBG.GetComponent<SpriteRenderer> ().sprite = sprites [1];
				}
				tempTileBG.transform.SetParent (tileBgHolder, false);
			}
		}
	}

	public void repositionBoard (float x, float y, float z)
	{
		tileBgHolder.position = new Vector3 (x, y, z);
	}

}
