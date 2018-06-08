using UnityEngine;

public class BoardLayout001 : BoardLayout
{
    //[SerializeField] protected GameObject XXX;
    //string S1 = SpecialType.Unmovable.ToString();

    protected override void CheckPrefabs()
    {
        base.CheckPrefabs();
        string errorMsg = "missing special prefabs in editor";
        if (XXX == null) throw new System.Exception(errorMsg);
    }

    protected override void InitBoardLayoutUnits(int col, int row)
    {
        layout = new GameObject[,] {
            { ___, XXX, ___, ___, ___, XXX,___ },
            { XXX, XXX, ___, ___, ___, XXX,XXX },
            { XXX, XXX, ___, ___, ___, XXX,XXX },
            { ___, ___, ___, ___, ___, ___,___ },
            { ___, ___, ___, ___, ___, ___,___ },
            { ___, ___, ___, ___, ___, ___,___ },
            { ___, ___, ___, ___, ___, ___,___ },
            { XXX, ___, ___, ___, ___, ___,XXX }
        };
    }
}
