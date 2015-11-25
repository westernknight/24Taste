using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class Slot : MonoBehaviour,IDropHandler
{


    protected GameObject item;
    public int id;
    void Start()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i) == transform)
            {
                id = i;
                break;
            }
        }
    }
   
    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Slot:OnDrop");
        if (transform.childCount > 0)
        {
            transform.GetChild(0).SetParent(DragHandler.itemBeingDragged.transform.parent);
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }
        else
        {
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }
        
    }
}
