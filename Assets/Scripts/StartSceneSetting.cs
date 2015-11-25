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
    }
}
