using UnityEngine;

public class BoardCompose : MonoBehaviour
{
    string[,] blueprint = null;

    public void setBlueprint(string[,] bp)
    {
        blueprint = bp;
    }

    public bool compositionDone_onBoard(Unit[,] table)
    {
        if(blueprint != null){
            return compareBoard_withBlueprint(table, blueprint);
        }else{
            Debug.Log("no blueprint found");
            throw new System.Exception("no blueprint found");
        }

    }

    bool compareBoard_withBlueprint(Unit[,] table, string[,] bp)
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
