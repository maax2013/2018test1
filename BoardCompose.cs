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
                bool isSocket = bp[c, r] != null;
                bool isNotMatch_WithBlueprint = table[c, r].GetUnitID() != bp[c, r];
                if(isSocket && isNotMatch_WithBlueprint){
                    return false;
                }
                //otherwise means the unit on socket matches with blueprint, pass
            }
        }
        //if all passes, then success
        return true;
    }
}
