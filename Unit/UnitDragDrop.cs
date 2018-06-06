using UnityEngine;

public class UnitDragDrop : Component
{
    public Unit_Base thisUnit;

    const float dragZ = -0.5f;

    void Awake(){
        Debug.Log("im awake");
    }

    public void startDrag()
    {
        //testMark(true);
        //TODO: start drag state
        transform.localPosition += new Vector3(0f, 0f, dragZ);
    }
    public void dragging(Vector3 pos)
    {
        transform.position = pos + new Vector3(0f, 0f, dragZ);
    }

    public void stopDrag()
    {
        //testMark(false);
        //TODO: stop drag state
        transform.localPosition = new Vector3(thisUnit.CurrentColumn, thisUnit.CurrentRow, 0f);
    }
}