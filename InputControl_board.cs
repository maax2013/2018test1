using UnityEngine;
//using UnityEngine.Events;
using UnityEngine.EventSystems;

//using System.Collections;

public class InputControl_board : MonoBehaviour
{
    //  [SerializeField] Camera mainCam;
    public bool InputEnabled { get; set; }

    public event System.Action<GameObject> onDragStart;
    public event System.Action<Vector3> onDragging;
    public event System.Action onDragEnd;

    bool dragEnabled;
    bool itemEnabled;
    bool menuEnabled;

    Bounds gameBoardBoundary;
    float tableUnitZ;
    bool insideBoardBoundary;
    Vector3 pointerWorldPosition;

    Ray screenRay;
    RaycastHit2D[] hitResults = new RaycastHit2D[1];

    public void createBoardBoundary(int column, int row, float unitZ)
    {
        /*make sure the boundary is on the same z index as the units on board*/
        tableUnitZ = unitZ;
        gameBoardBoundary = new Bounds(Vector3.zero, new Vector3(column, row, tableUnitZ));
        //Debug.Log(hitResults[0].transform);
    }

    public void EnableAll(){
        
    }

    public void EnableDrag(){
        
    }
    public void DisableDrag(){
        
    }
    public void EnableItem()
    {

    }
    public void DisableItem()
    {

    }



    void Update()
    {
        if (InputEnabled && Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        doDragStart(Input.GetTouch(0).position);
                        break;
                    case TouchPhase.Moved:
                        doDragging(Input.GetTouch(0).position);
                        break;
                    case TouchPhase.Ended:
                        doDragEnd();
                        break;
                    case TouchPhase.Canceled:
                        doDragEnd();
                        break;
                    default:
                        break;
                }
            }
        }
        else if(InputEnabled && Input.mousePresent)
        {
            if(Input.GetMouseButtonDown(0))
            {
                doDragStart(Input.mousePosition);
            }
            else if(Input.GetMouseButton(0))
            {
                doDragging(Input.mousePosition);
            }
            else if(Input.GetMouseButtonUp(0))
            {
                //Debug.Log("end");
                doDragEnd();
            }
        }
    }

    void doDragStart(Vector3 v)
    {
        screenRay = Camera.main.ScreenPointToRay(v);
        Physics2D.GetRayIntersectionNonAlloc(screenRay, hitResults);
        //Debug.Log(hitResults[0].collider);
        if (hitResults[0].collider != null)
        {
            pointerWorldPosition = hitResults[0].point;
            insideBoardBoundary = BoardUtilities.pointerInsideBoundary(pointerWorldPosition, gameBoardBoundary);
            if (insideBoardBoundary)
            {
                //Debug.Log("fire");
                fireOnDragStart(hitResults[0].collider.gameObject);
            }
        }
    }
    void doDragging(Vector3 v)
    {
        if (hitResults[0].collider != null)
        {
            //Debug.Log(hitResults[0].collider);
            pointerWorldPosition = Camera.main.ScreenToWorldPoint(v);
            /*update z, so it is the same as the boundary and the units on board*/
            pointerWorldPosition.z = tableUnitZ;
            insideBoardBoundary = BoardUtilities.pointerInsideBoundary(pointerWorldPosition, gameBoardBoundary);
            if (insideBoardBoundary)
            {
                fireOnDragging(pointerWorldPosition);
            }
            else
            {
                //Debug.Log("out of boundary");
                doDragEnd();
            }
        }
    }
    void doDragEnd()
    {
        if (hitResults[0].collider != null)
        {
            hitResults = new RaycastHit2D[1];
            //Debug.Log(hitResults[0].collider);
            //InputEnabled = false;
            fireOnDragEnd();
        }
    }

    void fireOnDragStart(GameObject o)
    {
        if (onDragStart != null)
            onDragStart(o);
    }
    void fireOnDragging(Vector3 v)
    {
        if (onDragging != null)
            onDragging(v);
    }
    void fireOnDragEnd()
    {
        if (onDragEnd != null)
            onDragEnd();
    }

}

