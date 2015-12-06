using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StartSceneSetting : MonoBehaviour
{


    public int level = 0;
    public static StartSceneSetting instance;
    public bool playBgm = true;
    public List<AudioClip> bgm;
    public AudioSource audio;
    bool lastPlayBgm = true;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(476, 847, true);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(audio);


    }
    public void SetLevel(int l)
    {
        level = l;
    }
    public void PlayBGM(int index)
    {
        if (playBgm)
        {
            audio.clip = bgm[index];
            audio.Play();
        }
    }
    public void SetBGMVolumn(float vol)
    {
        //0-1
        audio.volume = vol;
    }
    public void InitSoundBirds()
    {
        Vector3 vec = GameObject.Find("birds").transform.position;
        vec.y = StartSceneSetting.instance.audio.volume * Screen.height / 2;
        GameObject.Find("birds").transform.position = vec;
    }
    public void OnClick()
    {
        Vector3 vec = GameObject.Find("birds").transform.position;
        vec.y = Input.mousePosition.y;

        LeanTween.cancel(GameObject.Find("birds"));
        LeanTween.value(GameObject.Find("birds"), GameObject.Find("birds").transform.position, vec, 0.1f).setOnUpdate((Vector3 value) =>
        {
            GameObject.Find("birds").transform.position = value;
        });
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
       
        if (vec.y < Screen.height / 2 * 0.1f)
        {
            SetBGMVolumn(0);
        }
        else
        {
            SetBGMVolumn(vec.y / (Screen.height / 2));
        }
       

    }
    void Update()
    {

        if (lastPlayBgm != playBgm)
        {
            lastPlayBgm = playBgm;
            if (playBgm)
            {
                audio.Play();
            }
            else
            {
                if (audio.isPlaying)
                {
                    audio.Pause();
                }
            }
        }

        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            if (Application.loadedLevelName != "startScene")
            {
                Application.LoadLevel("startScene");
            }

        }
    }
}
