using UnityEngine;
using System.Collections;

public class Unit_Base : MonoBehaviour
{
    [SerializeField] GameObject textObj;
    [SerializeField] GameObject glowObj;

    public int CurrentRow { get; protected set; }
    public int CurrentColumn { get; protected set; }

    //public bool CanUpgrade { get; protected set; }
    //public bool CanFall { get; protected set; }
    //public bool CanDragDrop { get; protected set; }
    //public bool CanJump { get; protected set; }
    //public bool CanMatch { get; protected set; }
    //public bool CanMerge { get; protected set; }
    //public bool CanSwap { get; protected set; }

    public UnitUpgrade UUpgrade { get; protected set; }
    public UnitMerge UMerge { get; protected set; }
    public UnitDragDrop UDragDrop { get; protected set; }
 //   void Awake()
 //   {
 //       Debug.Log("im awake");
 //   }

	void Start()
	{
        UUpgrade = GetComponent<UnitUpgrade>();
        UMerge = GetComponent<UnitMerge>();
        UDragDrop = GetComponent<UnitDragDrop>();
	}

	public void SetUnitCoord (int column, int row)
    {
        CurrentColumn = column;
        CurrentRow = row;
    }






    public void testMark(bool on)
    {
        glowObj.SetActive(on);
    }

    public void debugText(string t)
    {
        textObj.SetActive(true);
        textObj.GetComponent<TextMesh>().text = t;
    }

    public void showDebugCoord()
    {
        debugText(CurrentColumn.ToString() + ":" + CurrentRow.ToString());
    }
}
