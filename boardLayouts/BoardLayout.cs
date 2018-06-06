using UnityEngine;
//using System.Collections;

public class BoardLayout : MonoBehaviour
{
    protected string __ = null;
    protected string XX = GemType.Special.ToString() + SpecialType.Empty.ToString();
    protected string[,] layout;



    public string[,] GetBoardLayout()
    {
        InitBoardLayout();
        return BoardUtilities.flipStringTableCoord(layout);
    }

    protected virtual void InitBoardLayout()
    {
        layout = new string[,] {
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ },
            { __, __, __, __, __, __ }
        };
    }
}
