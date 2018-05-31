using UnityEngine;
using UnityEngine.EventSystems;

//using System.Collections;

public class BoardSettingBtn : MonoBehaviour, IPointerDownHandler
{
    public event System.Action OnSettingClicked;

    void Start()
    {
        addPhysics2DRaycaster();
    }

    void addPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }
    public void EnableBtn()
    {

    }
    public void DisableBtn(){
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //      Debug.Log ("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

}
