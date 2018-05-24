using UnityEngine;
//using System.Collections;

public class BoardSwapUnits : MonoBehaviour
{
    public event System.Action onAllSwapsDone;

    CountDownTimeBar cdTimer;
    InputControl_board inputCtr;
    DragDrop dragDrop;

    Unit cueUnit;
    Unit[,] unitsTable;

    public void init(Unit[,] table){
        unitsTable = table;
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
