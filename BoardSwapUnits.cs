using UnityEngine;
//using System.Collections;

public class BoardSwapUnits : MonoBehaviour
{
    public event System.Action onAllSwapsDone;

    CountDownTimeBar cdTimer;
    //InputControl inputCtr;
    InputControl_board inputCtr;
    DragDrop dragDrop;

    //Bounds gameBoardBoundary;
    Unit cueUnit;
    //float totalTime = 5f;
    Unit[,] unitsTable;

    public void init(Unit[,] table){
        unitsTable = table;
        //inputCtr = GetComponent<InputControl>();
        inputCtr = GetComponent<InputControl_board>();
        inputCtr.createBoundary(unitsTable.GetLength(0), unitsTable.GetLength(1), unitsTable[0,0].transform.position.z);
        dragDrop = GetComponent<DragDrop>();
        dragDrop.init(unitsTable);
    }

    public void passCDTimer(CountDownTimeBar cdT){
        cdTimer = cdT;
    }

    public void addTempEtraDraggingTime(float t)
    {
        cdTimer.addExtraTimeToCurrentRound(t);
    }

    public void enableDragging(){
        inputCtr.InputEnabled = true;
        inputCtr.onDragStart -= handleOnDragStart;
        inputCtr.onDragStart += handleOnDragStart;
    }
    public void disableDragging(){
        inputCtr.InputEnabled = false;
    }

    void handleOnDragStart(GameObject obj)
    {
        if (obj.GetComponent<Unit>() != null)
        {
            cueUnit = obj.GetComponent<Unit>();
            cueUnit.startDrag();
            dragDrop.readyDrag(cueUnit);

            inputCtr.onDragging -= handleOnDragging;
            inputCtr.onDragging += handleOnDragging;
            inputCtr.onDragEnd -= handleOnDragEnd;
            inputCtr.onDragEnd += handleOnDragEnd;
            //dragDrop.OnMove -= switchUnit_Towards;
            //dragDrop.OnMove += switchUnit_Towards;
            //dragDrop.OnMove -= startCDTimer;
            //dragDrop.OnMove += startCDTimer;
        }else{
            throw new System.Exception("dragging something else, not unit on board");
        }
    }
    void handleOnDragging(Vector3 pos){
        cueUnit.transform.position = pos + new Vector3(0f, 0f, cueUnit.getUnitDragZ());

        dragDrop.OnMove -= startCDTimer;
        dragDrop.OnMove += startCDTimer;

        dragDrop.OnMove -= switchUnit_Towards;
        dragDrop.OnMove += switchUnit_Towards;

        dragDrop.dragMove(pos);
    }

    void handleOnDragEnd(){
        if (cueUnit)
        {
            cueUnit.stopDrag();
            cueUnit = null;
        }
        //inputCtr.onDragStart -= handleOnDragStart;
        inputCtr.onDragging -= handleOnDragging;
        //inputCtr.onDragEnd -= handleOnDragEnd;

        cdTimer.stopTimer();
        disableDragging();

        fireAllSwapsDone();
    }




    //public void switch_BoardTouchable(bool on)
    //{
    //    var inputCtr = GetComponent<InputControl>();
    //    if (on)
    //    {
    //        ////            gameTouchable = true;
    //        //          inputCtr.inputEnabled = true;
    //        //          inputCtr.OnTouch += HandleOnTouch;
    //        dragDrop.init(unitsTable);
    //        InputControl.onDragStart -= onDragStart;
    //        InputControl.onDragStart += onDragStart;
    //        //          InputControl.onDrag += onDrag;
    //        //          InputControl.onDragEnd += onDragEnd;
    //        //          dragDrop.OnMove += switchUnit_Towards;
    //    }
    //    else
    //    {
    //        //TODO: disable input
    //        dragDropDone();
    //        ////            gameTouchable = false;
    //        //          inputCtr.inputEnabled = false;
    //        //          inputCtr.OnTouch -= HandleOnTouch;
    //    }
    //    //      GUIctr.switch_BoardTouchable (on);
    //}

    //void onDragEnd(GameObject obj, Vector3 pos)
    //{
    //    dragDropDone();
    //    collapseAll_matches_OnBoard();
    //}

    //void dragDropDone()
    //{
    //    if (cueUnit)
    //    {
    //        cueUnit.stopDrag();
    //        cueUnit = null;
    //    }
    //    InputControl.onDrag -= onDrag;
    //    InputControl.onDragEnd -= onDragEnd;
    //    dragDrop.OnMove -= switchUnit_Towards;
    //    dragDrop.OnMove -= startCDTimer;
    //    cdTimer.stopTimer();
    //    //++++++++++++++++++++++
    //}

    //void onDrag(GameObject obj, Vector3 pos)
    //{
    //    cueUnit.transform.localPosition = pos + new Vector3(3f, 3.5f, -1f);
    //    if (pointerInsideBoundary(pos, gameBoardBoundary))
    //    {
    //        //          print ("inside");
    //        GetComponent<DragDrop>().dragMove(pos);
    //    }
    //    else
    //    {
    //        //          print ("out");
    //        dragDropDone();
    //    }
    //}

    //bool pointerInsideBoundary(Vector3 p, Bounds boundary)
    //{
    //    if (boundary.Contains(p))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //void onDragStart(GameObject obj, Vector3 pos)
    //{
    //    if (obj.GetComponent<Unit>() != null)
    //    {
    //        cueUnit = obj.GetComponent<Unit>();
    //        cueUnit.startDrag();
    //        dragDrop.readyDrag(cueUnit);
    //        InputControl.onDrag -= onDrag;
    //        InputControl.onDrag += onDrag;
    //        InputControl.onDragEnd -= onDragEnd;
    //        InputControl.onDragEnd += onDragEnd;
    //        dragDrop.OnMove -= switchUnit_Towards;
    //        dragDrop.OnMove += switchUnit_Towards;
    //        dragDrop.OnMove -= startCDTimer;
    //        dragDrop.OnMove += startCDTimer;
    //    }
    //}

    ////  void HandleOnTouch (GameObject obj)
    ////  {
    //////        Debug.Log (obj);
    ////      if (obj.GetComponent<Unit> () != null) {
    ////          cueUnit = obj.GetComponent<Unit> ();
    ////          DragDrop dragDrop = GetComponent<DragDrop> ();
    ////
    ////          if (dragDrop.enabled) {
    ////              //temprary use, before the true dragNdrop function is created
    ////              dragDrop.enabled = false;//------------------------
    ////              cueUnit.stopDrag ();//--------------------------
    ////          } else {
    ////              dragDrop.enabled = true;
    ////              cueUnit.startDrag ();
    ////              dragDrop.OnMove += switchUnit_Towards;
    ////          }
    ////
    ////      }
    ////  }

    void startCDTimer(Vector2Int direction)
    {
        if (!cdTimer.IsRuning())
        {
            cdTimer.onTimesUp -= handleOnTimesUp;
            cdTimer.onTimesUp += handleOnTimesUp;
            cdTimer.startCountDown();
        }
    }
    void handleOnTimesUp()
    {
        handleOnDragEnd();
    }





    void switchUnit_Towards(Vector2Int direction)
    {
        Unit targetUnit = BoardUtilities.getUnitOnTable(cueUnit.CurrentColumn + direction.x, cueUnit.CurrentRow + direction.y, unitsTable);
        if (targetUnit == null)
        {
            //Debug.Log("out!");
            throw new System.Exception("invalid unit to switch towards");
        }
        else
        {
            //          BoardUtilities.switchUnitsCoord (cueUnit, targetUnit, unitsTable);
            /*only need to move the target unit, the cue unit is following the pointer*/
            targetUnit.moveTo(new Vector2Int(-direction.x, -direction.y));
            BoardUtilities.switchUnitsCoord(cueUnit, targetUnit, unitsTable);

            //          tryMakeBlock (cueUnit);
            //          tryMakeBlock (targetUnit);
        }
    }

    void fireAllSwapsDone()
    {
        if (onAllSwapsDone != null)
        {
            onAllSwapsDone();
        }
    }
}
