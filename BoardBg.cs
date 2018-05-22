//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class BoardBg : MonoBehaviour
{
	[SerializeField] GameObject cellBgPrefab;

	[SerializeField] Sprite[] sprites;

	[SerializeField] Transform tileBgHolder;
    [SerializeField] Transform tileSocketHolder;

	GameObject tempTileBG;
    GameObject[,] tileBgTable;
    string[,] blueprint;

	public void createBoardTiles_ByRowColumn (int column, int row)
	{
        tileBgTable = new GameObject[column, row];
		for (int c = 0; c < column; c++) {
			for (int r = 0; r < row; r++) {
				tempTileBG = Instantiate (cellBgPrefab, new Vector3 (c, r, 0), Quaternion.identity) as GameObject;
				if ((r % 2 == 0 && c % 2 == 0) || (r % 2 != 0 && c % 2 != 0)) {
					tempTileBG.GetComponent<SpriteRenderer> ().sprite = sprites [0];
				} else {
					tempTileBG.GetComponent<SpriteRenderer> ().sprite = sprites [0];
				}
				tempTileBG.transform.SetParent (tileBgHolder, false);
                tileBgTable[c, r] = tempTileBG;
			}
		}
	}

	public void repositionBoard (float x, float y, float z)
	{
		tileBgHolder.position = new Vector3 (x, y, z);
	}

    public void applyBlueprint(string[,] bp){
        blueprint = bp;
        for (int c = 0; c < bp.GetLength(0); c++)
        {
            for (int r = 0; r < bp.GetLength(1); r++)
            {
                if(bp[c,r] != null){
                    tempTileBG = Instantiate(cellBgPrefab, new Vector3(c, r, 0), Quaternion.identity) as GameObject;
                    tempTileBG.GetComponent<SpriteRenderer>().sprite = sprites[2];
                    tempTileBG.transform.SetParent(tileSocketHolder, false);
                }
            }
        }
    }

}
