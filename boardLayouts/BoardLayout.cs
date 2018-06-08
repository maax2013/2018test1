using UnityEngine;
using System.Collections.Generic;
//using System.Collections;

public class BoardLayout : MonoBehaviour
{
    [SerializeField] protected GameObject XXX;

    [SerializeField] GameObject normalUnitPrefab1;
    [SerializeField] GameObject normalUnitPrefab2;
    protected List<GameObject> normalUnitPrefabPool;

    protected GameObject ___ = null;
    protected GameObject[,] layout;

    public bool HasBluePrint { get; protected set; }

    public void initBoardLayout(int col = 6, int row = 7){
        normalUnitPrefabPool = new List<GameObject>();
        CheckPrefabs();
        InitBoardLayoutUnits(col, row);
    }
    public GameObject[,] GetBoardLayoutBg(){
        GameObject[,] targetT = new GameObject[layout.GetLength(0), layout.GetLength(1)];
        int column = targetT.GetLength(0);
        int row = targetT.GetLength(1);
        for (int c = 0; c < column; c++)
        {
            for (int r = 0; r < row; r++)
            {
                //mark empty unit as XXX, otherwise kept null as default
                if(layout[c,r] == XXX) targetT[c, r] = XXX;
            }
        }
        return FlipTableCoord(targetT);
    }

    public GameObject[,] GetBoardLayout()
    {
        return FlipTableCoord(layout);
    }
    protected virtual void CheckPrefabs(){
        if (XXX == null) throw new System.Exception("missing empty unit prefab in editor");
        normalUnitPrefabPool.Add(normalUnitPrefab1);
        normalUnitPrefabPool.Add(normalUnitPrefab2);
        for (int i = 0; i < normalUnitPrefabPool.Count; i++)
        {
            if (normalUnitPrefabPool[i] == null) throw new System.Exception("missing normal prefabs in editor");
        }
    }

    protected virtual void InitBoardLayoutUnits(int col, int row)
    {
        layout = new GameObject[row,col];
        //return layout;
        HasBluePrint = false;
        //TODO: init blueprint then validate it
    }
    protected virtual void InitBlueprint(int col, int row)
    {
        HasBluePrint = false;
        //TODO: init blueprint then validate it
    }

    //protected string[,] blueprint;

    //public string[,] getBlueprint()
    //{
    //    initBlueprint();
    //    return BoardUtilities.flipStringTableCoord(blueprint);
    //}

    //protected virtual void initBlueprint()
    //{
    //    blueprint = new string[,] {
    //        { ___, ___, ___, ___, ___, ___ },
    //        { ___, ___, ___, ___, ___, ___ },
    //        { ___, ___, ___, ___, ___, ___ },
    //        { ___, ___, ___, ___, ___, ___ },
    //        { ___, ___, ___, ___, ___, ___ },
    //        { ___, ___, ___, ___, ___, ___ },
    //        { ___, ___, ___, ___, ___, ___ }
    //    };
    //}

    protected GameObject[,] FlipTableCoord(GameObject[,] originalT)
    {
        /*flip the inner and outer array, so the table has same coord as the gameboard*/
        /*flip y axis to match unity coord*/
        GameObject[,] targetT = new GameObject[originalT.GetLength(1), originalT.GetLength(0)];
        int column = targetT.GetLength(0);
        int row = targetT.GetLength(1);
        for (int c = 0; c < column; c++)
        {
            for (int r = 0; r < row; r++)
            {
                targetT[c, r] = originalT[row - 1 - r, c];
            }
        }
        return targetT;
    }

    //protected void ValidateBoardLayout_withBlueprint(string[,] layout, string[,] bp)
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
}
