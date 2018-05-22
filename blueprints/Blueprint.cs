using UnityEngine;

public class Blueprint : MonoBehaviour
{
    protected string __ = null;
    protected string[,] blueprint;



    public string[,] getBlueprint()
    {
        init();
        return BoardUtilities.flipBlueprintTableCoord(blueprint);
    }

    protected virtual void init()
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
