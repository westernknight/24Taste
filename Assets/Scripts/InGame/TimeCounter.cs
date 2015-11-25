using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{

    Text timeText;
    float elapseTime = 0;
    bool running = false;
    void Start()
    {
        timeText = GetComponentInChildren<Text>();
        ResetAndStart();
    }

    public void ResetAndStart()
    {
        timeText.color = new Color(0, 111 / 255.0f, 33 / 255.0f);
        running = true;
        elapseTime = 0;


        float nowTime = GameProcess.instance.oneQuestionTime;
        string a = ((int)(nowTime) / 10).ToString();
        string b = ((int)(nowTime) % 10).ToString();
        string c = ((int)((nowTime) * 10) % 10).ToString();
        string d = ((int)((nowTime) * 100) % 10).ToString();
        timeText.text = a + b + "." + c + d;

    }
    public void Pause()
    {
        running = false;
    }
    public void Resume()
    {
        running = true;
    }
    void TimeOut()
    {
        Debug.Log("TimeOut");
    }
    void Update()
    {
        if (running)
        {

            elapseTime += Time.deltaTime;
            if (GameProcess.instance.oneQuestionTime - elapseTime > 0)
            {
                float nowTime = GameProcess.instance.oneQuestionTime - elapseTime;
                string a = ((int)(nowTime) / 10).ToString();
                string b = ((int)(nowTime) % 10).ToString();
                string c = ((int)((nowTime) * 10) % 10).ToString();
                string d = ((int)((nowTime) * 100) % 10).ToString();
                timeText.text = a + b+"."+c+d;
                if (nowTime<5)
                {
                    timeText.color = Color.red;
                }
            }
            else
            {
                timeText.text = "00.00";
                running = false;
                TimeOut();
            }
        }
    }

}
