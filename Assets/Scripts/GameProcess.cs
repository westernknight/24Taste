using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameProcess : MonoBehaviour
{
    public int oneQuestionTime = 15;
    public class AnswerStruct
    {
        public bool has = false;
        public List<int> numbers = new List<int>();
        public List<string> operators = new List<string>();
    }
    /// <summary>
    /// 3个符号的所有排列，其实里面会有重复的
    /// </summary>
    List<string> allMethod = new List<string>();
    bool getNext = false;

    public AnswerStruct lastAnswer = null;

    public AudioSource clickSound;

    public static GameProcess instance;

    TimeCounter timer;
    GameObject OperatorPanel;
    GameObject AnswerCardsPanel;
    GameObject QuestionCardsPanel;

    GameObject tweentyFourText;

    Text operator1;
    Text operator2;
    Text operator3;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        timer = GameObject.Find("TimeCounter").GetComponent<TimeCounter>();
        OperatorPanel = GameObject.Find("OperatorPanel");
        AnswerCardsPanel = GameObject.Find("AnswerCardsPanel");
        QuestionCardsPanel = GameObject.Find("QuestionCardsPanel");
        tweentyFourText = GameObject.Find("24");

        operator1 = GameObject.Find("operator1").GetComponent<Text>();
        operator2 = GameObject.Find("operator2").GetComponent<Text>();
        operator3 = GameObject.Find("operator3").GetComponent<Text>();


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
        if (StartSceneSetting.instance)
        {
            if (StartSceneSetting.instance.level == 0)
            {
                StartSceneSetting.instance.PlayBGM(1);
            }
            else
            {
                StartSceneSetting.instance.PlayBGM(Random.Range(2, 6));
            }
            Vector3 vec = GameObject.Find("birds").transform.position;
            vec.y = StartSceneSetting.instance.audio.volume * Screen.height;
            GameObject.Find("birds").transform.position = vec;


        }

        GetQuestion();
    }
    public void MoveCardToAnswerSlot(GameObject go)
    {
        if (go.transform.parent.parent.gameObject == QuestionCardsPanel)
        {
            for (int i = 0; i < AnswerCardsPanel.transform.childCount; i++)
            {
                var slot = AnswerCardsPanel.transform.GetChild(i);
                if (slot.childCount == 0)
                {

                    go.transform.SetParent(slot);
                    LeanTween.value(go, go.transform.position, slot.position, 0.2f).setOnUpdate(
                        (Vector3 vec) =>
                        {
                            go.transform.position = vec;
                        }
                        ).onComplete += () =>
                        {
                            Debug.Log("onComplete");
                            List<string> numbers = new List<string>();
                            for (int j = 0; j < AnswerCardsPanel.transform.childCount; j++)
                            {
                                var check_slot = AnswerCardsPanel.transform.GetChild(j);
                                if (check_slot.childCount != 0)
                                {
                                    numbers.Add(check_slot.GetChild(0).GetChild(0).GetComponent<Text>().text);
                                }
                            }
                            if (numbers.Count == 4)
                            {
                                if (Recount(numbers) == 24)
                                {
                                    GameProcess.instance.Right();
                                }
                                else
                                {
                                    GameProcess.instance.Wrong();
                                }
                            }

                        };
                    break;

                }
            }
        }
        else
        {
            for (int i = 0; i < QuestionCardsPanel.transform.childCount; i++)
            {
                var child = QuestionCardsPanel.transform.GetChild(i);
                if (child.childCount == 0)
                {

                    go.transform.SetParent(child);
                    LeanTween.value(go, go.transform.position, child.position, 0.2f).setOnUpdate(
                        (Vector3 vec) =>
                        {
                            go.transform.position = vec;
                        }
                        );
                    break;

                }
            }
        }
    }

    public void ResetCardsPositionWithAnim()
    {
        List<GameObject> slots = new List<GameObject>();

        for (int i = 0; i < QuestionCardsPanel.transform.childCount; i++)
        {
            var slot = QuestionCardsPanel.transform.GetChild(i);
            if (slot.childCount == 0)
            {
                slots.Add(slot.gameObject);
            }

        }

        List<GameObject> objList = new List<GameObject>();
        for (int i = 0; i < AnswerCardsPanel.transform.childCount; i++)
        {
            var child = AnswerCardsPanel.transform.GetChild(i);
            if (child.childCount != 0)
            {
                objList.Add(child.GetChild(0).gameObject);
            }

        }
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].transform.SetParent(slots[i].transform);
            var obj = objList[i];
            var slot = slots[i];
            LeanTween.value(obj, obj.transform.position, slot.transform.position, 0.2f).setOnUpdate(
                        (Vector3 vec) =>
                        {
                            obj.transform.position = vec;
                        }
                        );
        }
    }
    public void ResetCardsPosition()
    {
        List<GameObject> slots = new List<GameObject>();

        for (int i = 0; i < QuestionCardsPanel.transform.childCount; i++)
        {
            var slot = QuestionCardsPanel.transform.GetChild(i);
            if (slot.childCount == 0)
            {
                slots.Add(slot.gameObject);
            }

        }

        List<GameObject> objList = new List<GameObject>();
        for (int i = 0; i < AnswerCardsPanel.transform.childCount; i++)
        {
            var child = AnswerCardsPanel.transform.GetChild(i);
            if (child.childCount != 0)
            {
                objList.Add(child.GetChild(0).gameObject);
            }

        }
        for (int i = 0; i < objList.Count; i++)
        {
            objList[i].transform.SetParent(slots[i].transform);
            var obj = objList[i];
            var slot = slots[i];
            obj.transform.position = slot.transform.position;
        }
    }
    public void Right()
    {
        Debug.Log("Right");
        timer.Pause();
        StartCoroutine(GameNext());
    }
    public void Wrong()
    {
        Debug.Log("Wrong");

        List<string> numbers = new List<string>();
        for (int j = 0; j < AnswerCardsPanel.transform.childCount; j++)
        {
            var check_slot = AnswerCardsPanel.transform.GetChild(j);
            if (check_slot.childCount != 0)
            {
                numbers.Add(check_slot.GetChild(0).GetChild(0).GetComponent<Text>().text);
            }
        }
        tweentyFourText.GetComponent<Text>().text = Recount(numbers).ToString();
        StartCoroutine(GameReset());

       
    }


    float Recount(List<string> numbers)
    {
        float result = float.Parse(numbers[0]); 
        for (int i = 1; i < numbers.Count; i++)
        {
            if (lastAnswer.operators[i-1] == "+")
            {
                result += float.Parse(numbers[i]);
            }
            if (lastAnswer.operators[i - 1] == "x")
            {
                result *= float.Parse(numbers[i]);
            }
            if (lastAnswer.operators[i - 1] == "-")
            {
                result -= float.Parse(numbers[i]);
            }
            if (lastAnswer.operators[i - 1] == "/")
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
    IEnumerator GameReset()
    {
        yield return new WaitForSeconds(1.5f);
        ResetCardsPositionWithAnim();
        tweentyFourText.GetComponent<Text>().text = "24";
    }
    IEnumerator GameNext()
    {
        yield return new WaitForSeconds(3);
       // Application.LoadLevel(Application.loadedLevelName);
        ResetCardsPosition();
        GetQuestion();
        timer.ResetAndStart();
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
    public void OnClick()
    {

        StartCoroutine(LoadLevelDelay("startScene"));

    }
    IEnumerator LoadLevelDelay(string name)
    {
        clickSound.Play();
        while (clickSound.isPlaying)
        {
            yield return null;
        }
        Application.LoadLevel(name);
    }
    public void OnDrag()
    {
        Vector3 vec = GameObject.Find("birds").transform.position;
        vec.y = Input.mousePosition.y;
        if (vec.y > Screen.height / 2)
        {
            vec.y = Screen.height / 2;
        }
        GameObject.Find("birds").transform.position = vec;

        if (StartSceneSetting.instance)
        {
            StartSceneSetting.instance.SetBGMVolumn(vec.y / (Screen.height / 2));
        }

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


                GameObject.Find("QuestionCardsPanel").transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = a.ToString();
                GameObject.Find("QuestionCardsPanel").transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = b.ToString();
                GameObject.Find("QuestionCardsPanel").transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = c.ToString();
                GameObject.Find("QuestionCardsPanel").transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = d.ToString();

                operator1.text = results.operators[0];
                operator2.text = results.operators[1];
                operator3.text = results.operators[2];
            }
        }
        lastAnswer = results;

    }
    public void PressButton()
    {

        clickSound.Play();
        if (getNext)
        {
            GetQuestion();
            getNext = false;
            GameObject.Find("answerText0").GetComponent<Text>().text = "";
            GameObject.Find("answerText1").GetComponent<Text>().text = "";
            GameObject.Find("answerText2").GetComponent<Text>().text = "";
            GameObject.Find("ButtonText").GetComponent<Text>().text = "答案";
        }
        else
        {
            getNext = true;
            GameObject.Find("ButtonText").GetComponent<Text>().text = "下道题";
            //GameObject.Find("answerText0").GetComponent<Text>().text = lastAnswer[0];
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
