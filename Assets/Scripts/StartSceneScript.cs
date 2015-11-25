using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour {

    public Button button1;
    public Button button2;

    public AudioSource clickSound;
	void Start () {
        button1.onClick.AddListener(() =>
        {
            //handle click here
            if (StartSceneSetting.instance)
            {
                StartSceneSetting.instance.level = 0;
            }
            StartCoroutine(LoadLevelDelay("game_click"));
        });
        button2.onClick.AddListener(() =>
        {
            //handle click here
            if (StartSceneSetting.instance)
            {
                StartSceneSetting.instance.level = 1;
            }
            StartCoroutine(LoadLevelDelay("game_click"));
        });
        if (StartSceneSetting.instance)
        {
            StartSceneSetting.instance.PlayBGM(0);
        }
       
       
	}
    
        void Update()
    {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            Application.Quit();
        }
    }
    
	public void OnClick()
    {
        StartSceneSetting.instance.playBgm = !StartSceneSetting.instance.playBgm;
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
	
}
