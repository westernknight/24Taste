using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameProcess2 : MonoBehaviour
{
    public int oneQuestionTime = 15;
    public class AnswerStruct
    {
        public bool has = false;
        public List<int> numbers = new List<int>();
        public List<string> operators = new List<string>();

    }
    List<GameObject> operationButtons = new List<GameObject>();
    List<GameObject> questionButtons = new List<GameObject>();

    Text typeInText;
    GameObject resetButton;

    GameObject birds;

    List<string> inputNumbers = new List<string>();
    List<string> inputOperators = new List<string>();
    /// <summary>
    /// 3个符号的所有排列，其实里面会有重复的
    /// </summary>
    List<string> allMethod = new List<string>();

    AnswerStruct answer = new AnswerStruct();
    void Start()
    {
        GameObject OperatorButtons = GameObject.Find("OperatorButtons");
        GameObject QuestionButtons = GameObject.Find("QuestionButtons");


        for (int i = 0; i < OperatorButtons.transform.childCount; i++)
        {
            operationButtons.Add(OperatorButtons.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < QuestionButtons.transform.childCount; i++)
        {
            questionButtons.Add(QuestionButtons.transform.GetChild(i).gameObject);
        }
        typeInText = GameObject.Find("typeInText").GetComponent<Text>();
        birds = GameObject.Find("birds");
        resetButton = GameObject.Find("resetButton");

        List<string> l1 = new List<string>() { "+", "-", "x", "/" };
        List<string> l2 = new List<string>() { "+", "-", "x", "/" };
        List<string> l3 = new List<string>() { "+", "-", "x", "/" };
        List<string> l4 = new List<string>() { "+", "-", "x", "/" };
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
        GetQuestion();
    }
    public void OnClick(GameObject go)
    {
        if (inputNumbers.Count==inputOperators.Count)
        {
            if (questionButtons.Contains(go))
            {
                if (go.GetComponent<CanvasGroup>().alpha == 1)
                {
                    inputNumbers.Add(go.transform.GetChild(0).GetComponent<Text>().text);
                    go.GetComponent<CanvasGroup>().alpha = 0.3f;
                }

            }
        }
        if (inputNumbers.Count==inputOperators.Count+1)
        {
            if (operationButtons.Contains(go))
            {
                inputOperators.Add(go.transform.GetChild(0).GetComponent<Text>().text);
            }
        }
        if (go == resetButton)
        {
            inputNumbers.Clear();
            inputOperators.Clear();
            typeInText.text = "";
        }
        
    }
    void PrintTypeInText()
    {

    }
    float Recount(List<string> numbers,List<string> operators)
    {
        float result = float.Parse(numbers[0]);
        for (int i = 1; i < numbers.Count; i++)
        {
            if (operators[i - 1] == "+")
            {
                result += float.Parse(numbers[i]);
            }
            if (operators[i - 1] == "x")
            {
                result *= float.Parse(numbers[i]);
            }
            if (operators[i - 1] == "-")
            {
                result -= float.Parse(numbers[i]);
            }
            if (operators[i - 1] == "/")
            {
                try
                {
                    result /= float.Parse(numbers[i]);
                }
                catch
                {
                    return 0;

                }
            }
        }
        return result;
    }
    void GetQuestion()
    {
        AnswerStruct results = new AnswerStruct();
        while (results.has == false)
        {
            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            if (StartSceneSetting.instance == null)
            {
                a = Random.Range(1, 10);
                b = Random.Range(1, 10);
                c = Random.Range(1, 10);
                d = Random.Range(1, 10);
            }
            else if (StartSceneSetting.instance.level == 0)
            {
                a = Random.Range(1, 10);
                b = Random.Range(1, 10);
                c = Random.Range(1, 10);
                d = Random.Range(1, 10);
            }
            else
            {
                a = Random.Range(1, 20);
                b = Random.Range(1, 20);
                c = Random.Range(1, 20);
                d = Random.Range(1, 20);
                while (a < 10 && b < 10 && c < 10 && d < 10)
                {
                    a = Random.Range(1, 20);
                    b = Random.Range(1, 20);
                    c = Random.Range(1, 20);
                    d = Random.Range(1, 20);
                }
            }
            results = Calc(a, b, c, d);
            if (results.has)
            {
                string q = "" + a + "    " + "" + b + "    " + "" + c + "    " + "" + d + "    ";
                Debug.Log(q);

                string answer = "(" + "(" + results.numbers[0] + results.operators[0] + results.numbers[1] + ")" + results.operators[1] + results.numbers[2] + ")" + results.operators[2] + results.numbers[3];

                Debug.Log(answer);

                for (int i = 0; i < 4; i++)
                {
                    questionButtons[i].transform.GetChild(0).GetComponent<Text>().text = results.numbers[i].ToString();
                }
                

            }
        }


    }
    AnswerStruct Calc(int a, int b, int c, int d)
    {
        AnswerStruct tmpResult = new AnswerStruct();

        List<int[]> numbers = Algorithms.PermutationAndCombination<int>.GetPermutation(new int[] { a, b, c, d });

        for (int i = 0; i < numbers.Count; i++)
        {

            for (int j = 0; j < allMethod.Count; j++)
            {

                float tmp = 0;

                if (allMethod[j][0] == '+')
                {
                    tmp = numbers[i][0] + numbers[i][1];
                }
                else if (allMethod[j][0] == '-')
                {
                    tmp = numbers[i][0] - numbers[i][1];
                }
                else if (allMethod[j][0] == 'x')
                {
                    tmp = numbers[i][0] * numbers[i][1];
                }
                else if (allMethod[j][0] == '/')
                {
                    try
                    {
                        tmp = ((float)numbers[i][0] / numbers[i][1]);
                    }
                    catch (System.Exception)
                    {

                        continue;
                    }
                }

                if (allMethod[j][1] == '+')
                {
                    tmp = tmp + numbers[i][2];
                }
                else if (allMethod[j][1] == '-')
                {
                    tmp = tmp - numbers[i][2];
                }
                else if (allMethod[j][1] == 'x')
                {
                    tmp = tmp * numbers[i][2];
                }
                else if (allMethod[j][1] == '/')
                {
                    try
                    {
                        tmp = tmp / numbers[i][2];
                    }
                    catch (System.Exception)
                    {

                        continue;
                    }
                }

                if (allMethod[j][2] == '+')
                {
                    tmp = tmp + numbers[i][3];
                }
                else if (allMethod[j][2] == '-')
                {
                    tmp = tmp - numbers[i][3];
                }
                else if (allMethod[j][2] == 'x')
                {
                    tmp = tmp * numbers[i][3];
                }
                else if (allMethod[j][2] == '/')
                {
                    try
                    {
                        tmp = tmp / numbers[i][3];
                    }
                    catch (System.Exception)
                    {

                        continue;
                    }
                }
                if (tmp == 24)
                {
                    //string theValue = "(" + "(" + numbers[i][0] + allMethod[j][0] + numbers[i][1] + ")" + allMethod[j][1] + numbers[i][2] + ")" + allMethod[j][2] + numbers[i][3];



                    if (allMethod[j][0] == allMethod[j][1] && allMethod[j][1] == allMethod[j][2])
                    {
                        return tmpResult;
                    }

                    tmpResult.has = true;
                    tmpResult.numbers.Add(numbers[i][0]);
                    tmpResult.numbers.Add(numbers[i][1]);
                    tmpResult.numbers.Add(numbers[i][2]);
                    tmpResult.numbers.Add(numbers[i][3]);



                    tmpResult.operators.Add(allMethod[j][0].ToString());
                    tmpResult.operators.Add(allMethod[j][1].ToString());
                    tmpResult.operators.Add(allMethod[j][2].ToString());
                    return tmpResult;
                }
            }
        }



        return tmpResult;

    }
}
