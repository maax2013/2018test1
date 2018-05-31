//using UnityEngine;

public static class BoardCompose
{
    static string[,] blueprint = null;

    static public void SetBlueprint(string[,] bp)
    {
        blueprint = bp;
    }

    static public bool CompositionDone_onBoard(Unit[,] table)
    {
        if(blueprint != null){
            return CompareBoard_withBlueprint(table, blueprint);
        }else{
            //Debug.Log("no blueprint found");
            throw new System.Exception("no blueprint found");
        }

    }

    static bool CompareBoard_withBlueprint(Unit[,] table, string[,] bp)
    {
        int column = table.GetLength(0);
        int row = table.GetLength(1);
        for (int c = 0; c < column; c++)
        {
            for (int r = 0; r < row; r++)
            {
                if(table[c,r].UnitID != bp[c,r] && bp[c,r] != null){
                    return false;
                }
            }
        }
        return true;
    }
}
