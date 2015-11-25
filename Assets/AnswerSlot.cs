using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AnswerSlot : Slot
{
    public AnswerManager manager;
    
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log("AnswerSlot:OnDrop");
        //如果是空的，可以+孩子


        if (transform.childCount > 0)
        {
            transform.GetChild(0).SetParent(DragHandler.itemBeingDragged.transform.parent);
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }
        else
        {
            DragHandler.itemBeingDragged.transform.SetParent(transform);
        }


        AnswerManager.instance.DropInItem(item, id);
    }

}
