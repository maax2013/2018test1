using UnityEngine;
//using UnityEngine.EventSystems;

public class UsefulItem : MonoBehaviour
{
    [SerializeField] protected int coinPrice;
    [SerializeField] protected int currencyPrice;

    [SerializeField] protected Collider2D purchaseC;
    [SerializeField] protected Collider2D useC;


    public void enableItem(){
        useC.enabled = true;
        purchaseC.enabled = true;
    }
    public void disableItem()
    {
        useC.enabled = false;
        purchaseC.enabled = false;
    }

    protected virtual void useItem(){
        
    }

}
