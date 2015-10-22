using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartSceneRun : MonoBehaviour {

    public Button button1;
    public Button button2;
	void Start () {
        button1.onClick.AddListener(() =>
        {
            //handle click here
            StartSceneSetting.instance.level = 0;
            Application.LoadLevel("game");
        });
        button2.onClick.AddListener(() =>
        {
            //handle click here
            StartSceneSetting.instance.level = 1;
            Application.LoadLevel("game");
        });
        StartSceneSetting.instance.PlayBGM(0);
       
	}
	public void OnClick()
    {
        StartSceneSetting.instance.playBgm = !StartSceneSetting.instance.playBgm;
    }
	
}
