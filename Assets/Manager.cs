using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Manager : MonoBehaviour
{
    
    List<string> l1 = new List<string>() { "+", "-", "x", "/" };
    List<string> l2 = new List<string>() { "+", "-", "x", "/" };
    List<string> l3 = new List<string>() { "+", "-", "x", "/" };
    List<string> l4 = new List<string>() { "+", "-", "x", "/" };
    List<string> allMethod = new List<string>();
    bool getNext = false;

    List<string> lastAnswer = null;
    void Start()
    {
        for (int i = 0; i < l1.Count; i++)
        {
            string tmp = l1[i];
            for (int j = 0; j < l2.Count; j++)
            {
                tmp += l2[j];
                for (int k = 0; k < l3.Count; k++)
                {
                    tmp += l3[k];
                    for (int l = 0; l < l4.Count; l++)
                    {
                        tmp += l4[l];
                        //Debug.Log(tmp);
                        allMethod.Add(tmp);
                        tmp = tmp.Remove(tmp.Length - 1);
                    }

                    tmp = tmp.Remove(tmp.Length - 1);
                }
                tmp = tmp.Remove(tmp.Length - 1);
            }
        }

        for (int i = 0; i < allMethod.Count; i++)
        {
            allMethod[i] = allMethod[i].Remove(allMethod[i].Length - 1);
        }
        if (StartSceneSetting.instance)
        {
            
                StartSceneSetting.instance.PlayBGM(1);
                Vector3 vec = GameObject.Find("birds").transform.position;
                vec.y = StartSceneSetting.instance.audio.volume*Screen.height;
                GameObject.Find("birds").transform.position = vec;
            
            
        }
        
        GetQuestion();
    }
    public void OnClick()
    {
        Application.LoadLevel("startScene");
    }
    public void OnDrag()
    {
        Vector3 vec = GameObject.Find("birds").transform.position;
        vec.y = Input.mousePosition.y;
        GameObject.Find("birds").transform.position = vec;

        StartSceneSetting.instance.SetBGMVolumn(vec.y / Screen.height);
        //Debug.Log("here" + Input.mousePosition);
    }
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            Application.LoadLevel("startScene");
        }
    }
    void GetQuestion()
    {
        List<string> results = new List<string>();
        while (results.Count == 0)
        {
            int a = 0;//Random.Range(0, 10);
            int b = 0;//Random.Range(0, 10);
            int c = 0;//Random.Range(0, 10);
            int d = 0;// Random.Range(0, 10);
            if (StartSceneSetting.instance==null)
            {
                a = Random.Range(0, 10);
                b = Random.Range(0, 10);
                c = Random.Range(0, 10);
                d = Random.Range(0, 10);
            }
            else if (StartSceneSetting.instance.level == 0)
            {
                a=Random.Range(0,10);
                b=Random.Range(0,10);
                c=Random.Range(0,10);
                d = Random.Range(0, 10);
            }
            else
            {
                a = Random.Range(0, 20);
                b = Random.Range(0, 20);
                c = Random.Range(0, 20);
                d = Random.Range(0, 20);
            }
            results = Calc(a, b, c, d);
            if (results.Count != 0)
            {
                string q = "" + a + "    " + "" + b + "    " + "" + c + "    " + "" + d + "    ";
                Debug.Log(q);
                GameObject.Find("questionText").GetComponent<Text>().text = q;
                GameObject.Find("answerText0").GetComponent<Text>().text = "";
                GameObject.Find("answerText1").GetComponent<Text>().text = "";
                GameObject.Find("answerText2").GetComponent<Text>().text = "";
            }
        }
        lastAnswer = results;

    }
    public void PressButton()
    {
        if (getNext)
        {
            GetQuestion();
            getNext = false;
            GameObject.Find("answerText0").GetComponent<Text>().text = "";
            GameObject.Find("answerText1").GetComponent<Text>().text = "";
            GameObject.Find("answerText2").GetComponent<Text>().text = "";
            GameObject.Find("ButtonText").GetComponent<Text>().text = "GetAnswer";
        }
        else
        {
            getNext = true;
            GameObject.Find("ButtonText").GetComponent<Text>().text = "Next";
            GameObject.Find("answerText0").GetComponent<Text>().text = lastAnswer[0];
//             if (lastAnswer.Count==2)
//             {
//                 GameObject.Find("answerText1").GetComponent<Text>().text = lastAnswer[1];
//             }
//             else if (lastAnswer.Count==3)
//             {
//                 GameObject.Find("answerText2").GetComponent<Text>().text = lastAnswer[2];
//             }
//             

           
        }
    }


    List<string> Calc(int a, int b, int c, int d)
    {
        List<string> tmpResult = new List<string>();
        
        //GetTheNumberList(new List<int>() { a, b, c, d });
        DuplicatePerm.test(new int[] { a, b, c, d });
        List<List<float>> numberList = new List<List<float>>();
        numberList = DuplicatePerm.numList;
        for (int i = 0; i < numberList.Count; i++)
        {


            for (int j = 0; j < allMethod.Count; j++)
            {

                float tmp = 0;

                if (allMethod[j][0] == '+')
                {
                    tmp = numberList[i][0] + numberList[i][1];
                }
                else if (allMethod[j][0] == '-')
                {
                    tmp = numberList[i][0] - numberList[i][1];
                }
                else if (allMethod[j][0] == 'x')
                {
                    tmp = numberList[i][0] * numberList[i][1];
                }
                else if (allMethod[j][0] == '/')
                {
                    try
                    {
                        tmp = numberList[i][0] / numberList[i][1];
                    }
                    catch (System.Exception)
                    {

                        continue;
                    }

                }

                if (allMethod[j][1] == '+')
                {
                    tmp = tmp + numberList[i][2];
                }
                else if (allMethod[j][1] == '-')
                {
                    tmp = tmp - numberList[i][2];
                }
                else if (allMethod[j][1] == 'x')
                {
                    tmp = tmp * numberList[i][2];
                }
                else if (allMethod[j][1] == '/')
                {
                    try
                    {
                        tmp = tmp / numberList[i][2];
                    }
                    catch (System.Exception)
                    {

                        continue;
                    }
                }

                if (allMethod[j][2] == '+')
                {
                    tmp = tmp + numberList[i][3];
                }
                else if (allMethod[j][2] == '-')
                {
                    tmp = tmp - numberList[i][3];
                }
                else if (allMethod[j][2] == 'x')
                {
                    tmp = tmp * numberList[i][3];
                }
                else if (allMethod[j][2] == '/')
                {
                    try
                    {
                        tmp = tmp / numberList[i][3];
                    }
                    catch (System.Exception)
                    {

                        continue;
                    }
                }
                if (tmp == 24)
                {
                    string theValue = "(" + "(" + numberList[i][0] + allMethod[j][0] + numberList[i][1] + ")" + allMethod[j][1] + numberList[i][2] + ")" + allMethod[j][2] + numberList[i][3];

                    tmpResult.Add(theValue);
                    if (tmpResult.Count>2)
                    {
                        return tmpResult;
                    }
                }


                //tmp = 0;

                //if (allMethod[j][0] == '+')
                //{
                //    tmp = numberList[i][0] + numberList[i][1];
                //}
                //else if (allMethod[j][0] == '-')
                //{
                //    tmp = numberList[i][0] - numberList[i][1];
                //}
                //else if (allMethod[j][0] == 'x')
                //{
                //    tmp = numberList[i][0] * numberList[i][1];
                //}
                //else if (allMethod[j][0] == '/')
                //{
                //    try
                //    {
                //        tmp = numberList[i][0] / numberList[i][1];
                //    }
                //    catch (System.Exception)
                //    {

                //        continue;
                //    }

                //}

            }

        }   
        return tmpResult;

    }
}
