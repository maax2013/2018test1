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

    public void CreateBoardTiles_FromLayout_andBlueprint(GameObject[,] boardLayout, string[,] blueprint)
    {
        for (int c = 0; c < boardLayout.GetLength(0); c++)
        {
            for (int r = 0; r < boardLayout.GetLength(1); r++)
            {
                if(boardLayout[c,r]== null){
                    tempTileBG = Instantiate(cellBgPrefab, new Vector3(c, r, 0), Quaternion.identity) as GameObject;
                    if ((r % 2 == 0 && c % 2 == 0) || (r % 2 != 0 && c % 2 != 0))
                    {
                        tempTileBG.GetComponent<SpriteRenderer>().sprite = sprites[0];
                    }
                    else
                    {
                        tempTileBG.GetComponent<SpriteRenderer>().sprite = sprites[1];
                    }
                    tempTileBG.transform.SetParent(tileBgHolder, false);
                }
                //if(blueprint[c,r]!= null){
                //    tempTileBG = Instantiate(cellBgPrefab, new Vector3(c, r, 0), Quaternion.identity) as GameObject;
                //    tempTileBG.GetComponent<SpriteRenderer>().sprite = sprites[2];
                //    tempTileBG.transform.SetParent(tileSocketHolder, false);
                //}
            }
        }
    }

	public void RepositionBgHolder (float x, float y, float z)
	{
		tileBgHolder.position = new Vector3 (x, y, z);
	}

}
