using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAudio : MonoBehaviour
{

    private static ManagerAudio _instance;
    public static ManagerAudio Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ManagerAudio>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<ManagerAudio>();
                    singletonObject.name = typeof(ManagerAudio).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }


    public AudioSource audioSourceA;
    public AudioSource audioSourceB;
    public AudioSource audioSourceC;
    public AudioSource audioSourceSFX;


    public AudioClip trackA;
    public AudioClip trackB;
    public AudioClip trackC;
    public AudioClip trackSFX1;//SFX start
    public AudioClip trackSFX2;//SFX end
    public AudioClip trackSFX3;//SFX clearance

    //REMEMBER TO ASSIGN
    public AudioClip test1;
    public AudioClip test2;
    public AudioClip test3;
    public AudioClip test4;

    public void PlayA(AudioClip music)
    {
        audioSourceA.clip = music;
        audioSourceA.loop = true;
        audioSourceA.Play();
    }

    public void PlayB(AudioClip music)
    {
        audioSourceB.clip = music;
        audioSourceB.loop = true;
        audioSourceB.Play();
    }

    public void PlayC(AudioClip music)
    {
        audioSourceC.clip = music;
        audioSourceC.loop = true;
        audioSourceC.Play();
    }

    public void PlaySFX(string str)
    {


        switch (str)
        {
            case "start":
                audioSourceSFX.clip = trackSFX1;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                break;
            case "end":
                audioSourceSFX.clip = trackSFX2;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                break;
            case "clear":
                audioSourceSFX.clip = trackSFX3;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                break;
            default:
                print("No so called SFX");
                break;
        }





    }

    public void Stop()
    {
        audioSourceA.Stop();
        audioSourceB.Stop();
        audioSourceC.Stop();
        audioSourceSFX.Stop();
    }

    public void Pause()
    {
        audioSourceA.Pause();
        audioSourceB.Pause();
        audioSourceC.Pause();
        audioSourceSFX.Pause();
    }



    /// <summary>
    /// ///////////////////////////
    /// </summary>
    private void Awake()
    {
        //audio test, load music first

        audioSourceA = gameObject.AddComponent<AudioSource>();
        audioSourceB = gameObject.AddComponent<AudioSource>();
        audioSourceC = gameObject.AddComponent<AudioSource>();
        audioSourceSFX = gameObject.AddComponent<AudioSource>();

        string pathA = "Media/Sounds/BGM/PokemonDP/" + "201ばんどうろ（昼）";
        string pathB = "Media/Sounds/BGM/A Short Hike/06. A Short Flight";
        string pathC = "Media/Sounds/BGM/Flyff/08 Mystery";
        trackA = Resources.Load<AudioClip>(pathA);
        trackB = Resources.Load<AudioClip>(pathB);
        trackC = Resources.Load<AudioClip>(pathC);

        //jsonString = File.ReadAllText(jsonFilePath);
    }




    // Update is called once per frame
    void Update()
    {


        //audioSourceA.volume = 0.5f; // 设置音量为50%
        //audioSourceA.pitch = 1.5f; // 设置音高为原来的1.5倍




        // audio test - cont.
        if (Input.GetKeyDown(KeyCode.A))//PlayMusic
        {

            ManagerAudio.Instance.PlayA(trackA);
        }

        if (Input.GetKeyDown(KeyCode.S))//PlayMusic
        {

            ManagerAudio.Instance.PlayB(trackB);
        }

        if (Input.GetKeyDown(KeyCode.D))//PlayMusic
        {

            ManagerAudio.Instance.PlayC(trackC);
        }

        if (Input.GetKeyDown(KeyCode.Q))//PauseMusic
        {


            ManagerAudio.Instance.Pause();
        }

        if (Input.GetKeyDown(KeyCode.W))//PauseMusic
        {


            ManagerAudio.Instance.Stop();
        }
        if (Input.GetKeyDown(KeyCode.Z))//PauseMusic
        {


            ManagerAudio.Instance.PlaySFX("start");
        }
        if (Input.GetKeyDown(KeyCode.X))//PauseMusic
        {


            ManagerAudio.Instance.PlaySFX("end");
        }
        if (Input.GetKeyDown(KeyCode.C))//PauseMusic
        {


            ManagerAudio.Instance.PlaySFX("clear");
        }

        //if (Input.GetKeyDown(KeyCode.Alpha1))//ChangeMusic: Canadian
        //{
        //    testTemp = test1;
        //    SingletonExample.Instance.PlayMusic(testTemp);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))//ChangeMusic: Corean
        //{
        //    testTemp = test2;
        //    print("here");
        //    SingletonExample.Instance.PlayMusic(testTemp);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))//ChangeMusic: Nipponese
        //{
        //    testTemp = test3;
        //    SingletonExample.Instance.PlayMusic(testTemp);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))//ChangeMusic: Sinic
        //{
        //    testTemp = test4;
        //    SingletonExample.Instance.PlayMusic(testTemp);
        //}

    }
}
