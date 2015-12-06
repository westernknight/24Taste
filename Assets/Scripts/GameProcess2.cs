using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameProcess2 : MonoBehaviour
{
    enum RecountType
    {
        firstFirst,
        twoSide,
        middleLeft,
        middleRight,
        lastFirst,
    }
    public Sprite simpleLevelBk;
    public Sprite hardLevelBk;
    public bool isDebugRandom = false;
    public int debugNum = 0;
    public static GameProcess2 instance;
    public int oneQuestionTime = 18;
    public class AnswerStruct
    {
        public bool has = false;
        public List<int> numbers = new List<int>();
        public List<string> operators = new List<string>();

    }
    List<GameObject> operationButtons = new List<GameObject>();
    List<GameObject> questionButtons = new List<GameObject>();

    Text typeInText;
    Text answerText;
    GameObject resetButton;


    GameObject score;

    List<string> inputNumbers = new List<string>();
    List<string> inputOperators = new List<string>();
    /// <summary>
    /// 3个符号的所有排列，其实里面会有重复的
    /// </summary>
    List<string> allMethod = new List<string>();

    AnswerStruct answer = new AnswerStruct();

    TimeCounter timeCounter;
    public AudioSource clickSound;
    void Awake()
    {
        instance = this;
    }
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
        answerText = GameObject.Find("answerText").GetComponent<Text>();
        answerText.text = "";
        //birds = GameObject.Find("birds");
        resetButton = GameObject.Find("resetButton");
        typeInText.text = "";
        score = GameObject.Find("score");
        score.GetComponent<Text>().text = "0";

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

        timeCounter = GameObject.FindObjectOfType<TimeCounter>();
        timeCounter.timeOutEvent += () =>
        {
            string sz = "(" + "(" + answer.numbers[0] + answer.operators[0] + answer.numbers[1] + ")" + answer.operators[1] + answer.numbers[2] + ")" + answer.operators[2] + answer.numbers[3];
            sz += " = 24";
            answerText.text = sz;
            answerText.color = Color.red;
            SetButtonToNext();

        };
        
        

        ///setting
        if (StartSceneSetting.instance)
        {
            if (StartSceneSetting.instance.level == 0)
            {
                StartSceneSetting.instance.PlayBGM(1);
                GameObject.Find("bk").GetComponent<Image>().overrideSprite = simpleLevelBk;
            }
            else
            {
                StartSceneSetting.instance.PlayBGM(Random.Range(2, 6));
                GameObject.Find("bk").GetComponent<Image>().overrideSprite =  hardLevelBk;
            }
            StartSceneSetting.instance.InitSoundBirds();
            oneQuestionTime = oneQuestionTime * (StartSceneSetting.instance.level + 1);

            
        }

        GetQuestion();
        SetButtonToReset();
    }
    public void OnDrag()
    {
        if (StartSceneSetting.instance)
        {
            StartSceneSetting.instance.OnDrag();
        }
    }
    public void OnClickBackToStartScene()
    {

        StartCoroutine(LoadLevelDelay("startScene"));

    }
    public void OnDownReturnButton()
    {

        GameObject go = GameObject.Find("ReturnImage");
        LeanTween.value(go, Vector3.one, new Vector3(1.1f, 1.1f, 1.1f), 0.5f).setOnUpdate((Vector3 vec) =>
        {
            go.transform.localScale = vec;
        });
    }
    public void OnUpReturnButton()
    {
        LeanTween.cancelAll();
        GameObject go = GameObject.Find("ReturnImage");
        LeanTween.value(go, go.transform.localScale, Vector3.one, 0.5f).setOnUpdate((Vector3 vec) =>
        {
            go.transform.localScale = vec;
        });
    }
//     public void OnDrag()
//     {
//         Vector3 vec = GameObject.Find("birds").transform.position;
//         vec.y = Input.mousePosition.y;
//         if (vec.y > Screen.height / 2)
//         {
//             vec.y = Screen.height / 2;
//         }
//         GameObject.Find("birds").transform.position = vec;
// 
//         if (StartSceneSetting.instance)
//         {
//             StartSceneSetting.instance.SetBGMVolumn(vec.y / (Screen.height / 2));
//         }
// 
//         //Debug.Log("here" + Input.mousePosition);
//     }
    IEnumerator LoadLevelDelay(string name)
    {

        clickSound.Play();
        while (clickSound.isPlaying)
        {
            yield return null;
        }
        Application.LoadLevel(name);
    }
    public void OnClick(GameObject go)
    {
        if (go == resetButton)
        {
            if (resetButton.transform.GetChild(0).GetComponent<Text>().text == "下 一 题")
            {
                if (answerText.text != "")
                {
                    score.GetComponent<Text>().text = "0";
                }
                answerText.text = "";
                GetQuestion();
                SetButtonToReset();
            }
            inputNumbers.Clear();
            inputOperators.Clear();
            for (int i = 0; i < questionButtons.Count; i++)
            {
                questionButtons[i].GetComponent<CanvasGroup>().alpha = 1;
            }
            typeInText.text = "";

        }
        else if (inputNumbers.Count == inputOperators.Count)
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
        else if (inputNumbers.Count == inputOperators.Count + 1)
        {
            if (operationButtons.Contains(go))
            {
                inputOperators.Add(go.transform.GetChild(0).GetComponent<Text>().text);
            }
            if (inputOperators.Count == 3 && inputNumbers.Count == 3)
            {
                GameObject last = null;
                for (int i = 0; i < questionButtons.Count; i++)
                {
                    if (questionButtons[i].GetComponent<CanvasGroup>().alpha == 1)
                    {
                        last = questionButtons[i];
                        break;
                    }
                }
                last.GetComponent<CanvasGroup>().alpha = 0.3f;
                inputNumbers.Add(last.transform.GetChild(0).GetComponent<Text>().text);
            }
        }

        PrintTypeInText();
    }

    void PrintTypeInText()
    {
        string sz = "";

        if (inputNumbers.Count == 1)
        {
            sz += inputNumbers[0];
            if (inputOperators.Count == 1)
            {
                sz += inputOperators[0];
            }
        }
        else if (inputNumbers.Count == 2)
        {
            if (inputOperators.Count == 2)
            {
          
                sz += inputNumbers[0];
                sz += inputOperators[0];
                sz += inputNumbers[1];
        
                sz += inputOperators[1];
            }
            else//1
            {
                sz += inputNumbers[0];
                sz += inputOperators[0];
                sz += inputNumbers[1];
            }
        }
        else if (inputNumbers.Count == 3)
        {
            if (inputOperators.Count == 3)
            {
     
                sz += inputNumbers[0];
                sz += inputOperators[0];
                sz += inputNumbers[1];
          
                sz += inputOperators[1];
                sz += inputNumbers[2];
            
                sz += inputOperators[2];
            }
            else//2
            {
        
                sz += inputNumbers[0];
                sz += inputOperators[0];
                sz += inputNumbers[1];
          
                sz += inputOperators[1];
                sz += inputNumbers[2];
            }

        }
        else if (inputNumbers.Count == 4)
        {
            RecountType rType;
            string recount = Recount(inputNumbers, inputOperators, out rType).ToString();
            switch (rType)
            {
                case RecountType.firstFirst:
                    {
                        sz += "((";
                        sz += inputNumbers[0];
                        sz += inputOperators[0];
                        sz += inputNumbers[1];
                        sz += ")";
                        sz += inputOperators[1];
                        sz += inputNumbers[2];
                        sz += ")";
                        sz += inputOperators[2];
                        sz += inputNumbers[3];
                        sz += " = ";
                        sz += recount;
                    }
                    break;
                case RecountType.twoSide:
                    {
                        sz += "(";
                        sz += inputNumbers[0];
                        sz += inputOperators[0];
                        sz += inputNumbers[1];
                        sz += ")";
                        sz += inputOperators[1];
                        sz += "(";
                        sz += inputNumbers[2];              
                        sz += inputOperators[2];
                        sz += inputNumbers[3];
                        sz += ")";
                        sz += " = ";
                        sz += recount;
                    }
                    break;
                case RecountType.middleLeft:
                    {
                        sz += "(";
                        sz += inputNumbers[0];
                        sz += inputOperators[0];
                        sz += "(";
                        sz += inputNumbers[1];                       
                        sz += inputOperators[1];                        
                        sz += inputNumbers[2];
                        sz += "))";
                        sz += inputOperators[2];
                        sz += inputNumbers[3];              
                        sz += " = ";
                        sz += recount;
                    }
                    break;
                case RecountType.middleRight:
                    {
                       
                        sz += inputNumbers[0];
                        sz += inputOperators[0];
                        sz += "((";
                        sz += inputNumbers[1];                      
                        sz += inputOperators[1];                    
                        sz += inputNumbers[2];
                        sz += ")";
                        sz += inputOperators[2];
                        sz += inputNumbers[3];
                        sz += ")";
                        sz += " = ";
                        sz += recount;
                    }
                    break;
                case RecountType.lastFirst:
                    {
                
                        sz += inputNumbers[0];
                        sz += inputOperators[0];
                        sz += "(";
                        sz += inputNumbers[1];              
                        sz += inputOperators[1];
                        sz += "(";
                        sz += inputNumbers[2];            
                        sz += inputOperators[2];
                        sz += inputNumbers[3];
                        sz += "))";
                        sz += " = ";
                        sz += recount;
                    }
                    break;
                default:
                    break;
            }





           

            if (recount == "24")
            {
                FinishOneQuestion();
            }
        }
        typeInText.text = sz;


    }
    void FinishOneQuestion()
    {
        int n = int.Parse(score.GetComponent<Text>().text);
        n++;
        score.GetComponent<Text>().text = n.ToString();
        SetButtonToNext();
        timeCounter.Pause();
    }
    void SetButtonToNext()
    {
        resetButton.transform.GetChild(0).GetComponent<Text>().text = "下 一 题";
    }
    void SetButtonToReset()
    {
        resetButton.transform.GetChild(0).GetComponent<Text>().text = "橡 皮 擦";
    }
    float Recount(List<string> numbers, List<string> operators, out RecountType rType)
    {
        rType = RecountType.firstFirst;
        if (numbers.Count != 4 || operators.Count != 3)
        {
            Debug.Log("Recount faile.");

            return 0;
        }
        float result = 0;

		result = float.Parse(numbers[0]);
		result = GetTwoNumberResult(result, float.Parse(numbers[1]), operators[0]);
		result = GetTwoNumberResult(result, float.Parse(numbers[2]), operators[1]);
		result = GetTwoNumberResult(result, float.Parse(numbers[3]), operators[2]);
		if (result == 24)
		{
			rType = RecountType.firstFirst;
			return result;
		}


        result = float.Parse(numbers[3]);
        result = GetTwoNumberResult(result, float.Parse(numbers[2]), operators[2]);
        result = GetTwoNumberResult(result, float.Parse(numbers[1]), operators[1]);
        result = GetTwoNumberResult(result, float.Parse(numbers[0]), operators[0]);
        if (result == 24)
        {
            rType = RecountType.lastFirst;
            return result;
        }

        result = float.Parse(numbers[0]);
        result = GetTwoNumberResult(result, float.Parse(numbers[1]), operators[0]);
        float result2 = float.Parse(numbers[2]);
        result2 = GetTwoNumberResult(result2, float.Parse(numbers[3]), operators[2]);
        result = GetTwoNumberResult(result, result2, operators[1]);
        if (result == 24)
        {
            rType = RecountType.twoSide;
            return result;
        }


        result = float.Parse(numbers[1]);
        result = GetTwoNumberResult(result, float.Parse(numbers[2]), operators[1]);
        result = GetTwoNumberResult(float.Parse(numbers[0]), result, operators[0]);
        result = GetTwoNumberResult(result, float.Parse(numbers[3]), operators[2]);

        if (result == 24)
        {
            rType = RecountType.middleLeft;
            return result;
        }

        result = float.Parse(numbers[1]);
        result = GetTwoNumberResult(result, float.Parse(numbers[2]), operators[1]);
        result = GetTwoNumberResult(result, float.Parse(numbers[3]), operators[2]);
        result = GetTwoNumberResult(float.Parse(numbers[0]), result, operators[0]);

        if (result == 24)
        {
            rType = RecountType.middleRight;
            return result;
        }


		result = float.Parse(numbers[0]);
		result = GetTwoNumberResult(result, float.Parse(numbers[1]), operators[0]);
		result = GetTwoNumberResult(result, float.Parse(numbers[2]), operators[1]);
		result = GetTwoNumberResult(result, float.Parse(numbers[3]), operators[2]);


        return result;
    }
    float GetTwoNumberResult(float a, float b, string o)
    {
        if (o == "+")
        {
            a += b;
        }
        if (o == "x")
        {
            a *= b;
        }
        if (o == "-")
        {
            a -= b;
        }
        if (o == "÷")
        {
            try
            {
                a /= b;
            }
            catch
            {
                return 0;

            }
        }
        return a;
    }
    void GetQuestion()
    {
        timeCounter.ResetAndStart();
        AnswerStruct results = new AnswerStruct();
        while (results.has == false)
        {
            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;
            Random.seed = System.Environment.TickCount;
            Debug.Log("Random.seed = "+Random.seed);
            if (isDebugRandom)
            {
                Random.seed = debugNum;
            }
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

                string sz = "(" + "(" + results.numbers[0] + results.operators[0] + results.numbers[1] + ")" + results.operators[1] + results.numbers[2] + ")" + results.operators[2] + results.numbers[3];

                Debug.Log(sz);

                for (int i = 0; i < 4; i++)
                {
                    questionButtons[i].transform.GetChild(0).GetComponent<Text>().text = results.numbers[i].ToString();
                }
                answer = results;

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
