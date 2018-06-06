using UnityEngine;
//using System.Collections;

public class BoardSwapUnits : MonoBehaviour
{
    public event System.Action onAllSwapsDone;

    CountDownTimeBar cdTimer;
    InputControl_board inputCtr;

    Unit cueUnit;
    Unit[,] unitsTable;
    float offX;
    float offY;

    public void InitBoardSwap(Unit[,] table){
        unitsTable = table;
        inputCtr = GetComponent<InputControl_board>();
        Unit tempU = GetFirstValidUnit_onTable(table);
        inputCtr.createBoardBoundary(unitsTable.GetLength(0), unitsTable.GetLength(1), tempU.transform.position.z);

        Transform board = tempU.transform.parent;
        offX = board.position.x;
        offY = board.position.y;
    }
    Unit GetFirstValidUnit_onTable(Unit[,] table){
        foreach (var u in table)
        {
            if(u != null){
                return u;
            }
        }
        throw new System.Exception("can't find a single valid unit on table");
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
            DragMoveCoord.RegisterDragStartCoord(cueUnit.CurrentColumn, cueUnit.CurrentRow);

            inputCtr.onDragging -= handleOnDragging;
            inputCtr.onDragging += handleOnDragging;
            inputCtr.onDragEnd -= handleOnDragEnd;
            inputCtr.onDragEnd += handleOnDragEnd;

            //TODO: disable input/icons for items and settings
        }else{
            throw new System.Exception("dragging something else, not unit on board");
        }
    }
    void handleOnDragging(Vector3 pos){
        cueUnit.dragging(pos);

        DragMoveCoord.OnMove -= startCDTimer;
        DragMoveCoord.OnMove += startCDTimer;

        DragMoveCoord.OnMove -= switchUnit_Towards;
        DragMoveCoord.OnMove += switchUnit_Towards;

        DragMoveCoord.dragMove(pos.x - offX, pos.y - offY);
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
