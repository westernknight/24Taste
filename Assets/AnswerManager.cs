using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerManager : MonoBehaviour
{
    public static AnswerManager instance;

    

    void Awake()
    {
        instance = this;
    }
    
    public void DropInItem(GameObject item,int slotId)
    {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).childCount== 0)
                {
                    return;
                }
             
                if (transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text !=GameProcess.instance.lastAnswer.numbers[i].ToString())
                {
                    return;
                }
            }



            GameProcess.instance.Right();

    }
    public void RemoveOutItem(GameObject item, int slotId)
    {

    }

}
