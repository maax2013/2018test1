using UnityEngine;

public class Blueprint : MonoBehaviour
{
    protected string __ = null;
    protected string[,] blueprint;



    public string[,] getBlueprint()
    {
        initBlueprint();
        return BoardUtilities.flipStringTableCoord(blueprint);
    }

    protected virtual void initBlueprint()
    {
        blueprint = new string[,] {
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
