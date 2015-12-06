using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartSceneScript : MonoBehaviour
{

    public Button button1;
    public Button button2;

    public AudioSource clickSound;
    void Start()
    {
        button1.onClick.AddListener(() =>
        {
            //handle click here
            if (StartSceneSetting.instance)
            {
                StartSceneSetting.instance.level = 0;
            }
            StartCoroutine(LoadLevelDelay("game"));
        });
        button2.onClick.AddListener(() =>
        {
            //handle click here
            if (StartSceneSetting.instance)
            {
                StartSceneSetting.instance.level = 1;
            }
            StartCoroutine(LoadLevelDelay("game"));
        });
        if (StartSceneSetting.instance)
        {
            StartSceneSetting.instance.PlayBGM(0);
            StartSceneSetting.instance.InitSoundBirds();
        }


    }
    public void OnClick()
    {
        if (StartSceneSetting.instance)
        {
            StartSceneSetting.instance.OnClick();
        }
    }
    public void OnDrag()
    {
        if (StartSceneSetting.instance)
        {
            StartSceneSetting.instance.OnDrag();
        }
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            Application.Quit();
        }
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
