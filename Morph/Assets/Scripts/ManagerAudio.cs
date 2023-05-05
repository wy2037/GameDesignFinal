using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

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
                DontDestroyOnLoad(_instance.gameObject);
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


    public AudioSource audioSourceTheme;
    public AudioSource audioSourceThemeV;

    public AudioSource audioSourceA;
    public AudioSource audioSourceB;
    public AudioSource audioSourceC;
    public AudioSource audioSourceSFX;


    public AudioClip trackTheme;
    public AudioClip trackThemeV;
    public AudioClip trackA;
    public AudioClip trackB;
    public AudioClip trackC;
    public AudioClip trackSFX1;//SFX death - fail
    public AudioClip trackSFX2;//SFX end - pass level
    public AudioClip trackSFX3;//SFX clearall - pass all levels
    public AudioClip trackSFX4;//SFX jump
    public AudioClip trackSFX5;//SFX change to solid
    public AudioClip trackSFX6;//SFX change to liquid
    public AudioClip trackSFX7;//SFX change to gas

    //REMEMBER TO ASSIGN
    public AudioClip test1;
    public AudioClip test2;
    public AudioClip test3;
    public AudioClip test4;

    public void PlayTheme()
    {

        audioSourceTheme.clip = trackTheme;
        audioSourceTheme.loop = true;
        audioSourceTheme.Play();
        print("play now");
    }

    public void PlayThemeV()
    {
        audioSourceThemeV.clip = trackThemeV;
        audioSourceThemeV.loop = true;
        audioSourceThemeV.pitch = 0.82f;
        audioSourceThemeV.Play();
    }
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
            case "death":
                audioSourceSFX.clip = trackSFX1;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                break;
            case "end":
                ManagerAudio.Instance.Stop();
                audioSourceSFX.clip = trackSFX2;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                
                break;
            case "clearall":
                ManagerAudio.Instance.Stop();

                audioSourceSFX.clip = trackSFX3;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                break;
            case "jump":
                audioSourceSFX.clip = trackSFX4;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                break;
            case "solid":
                audioSourceSFX.clip = trackSFX5;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                break;
            case "liquid":
                audioSourceSFX.clip = trackSFX6;
                audioSourceSFX.loop = false;
                audioSourceSFX.Play();
                break;
            case "gas":
                audioSourceSFX.clip = trackSFX7;
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
        audioSourceTheme.Stop();
        audioSourceThemeV.Stop();
        audioSourceA.Stop();
        audioSourceB.Stop();
        audioSourceC.Stop();
        //audioSourceSFX.Stop();
    }

    public void Pause()
    {
        audioSourceTheme.Pause();
        audioSourceA.Pause();
        audioSourceB.Pause();
        audioSourceC.Pause();
        //audioSourceSFX.Pause();
    }

    public void Volume0()
    {
        audioSourceTheme.volume = 0;
        audioSourceA.volume = 0;
        audioSourceB.volume = 0;
        audioSourceC.volume = 0;
    }

    //Unity里提过代码可以调整音量值0-256，bgm源文件音量较小故调整为150，sfx暂未调整。
    public void Volume100Theme()
    {
        audioSourceTheme.volume = 150;

    }

    public void Volume100A()
    {
        audioSourceA.volume = 150;

    }

    public void Volume100B()
    {
        audioSourceB.volume = 150;

    }

    public void Volume100C()
    {
        audioSourceC.volume = 150;

    }



    /// <summary>
    /// ///////////////////////////
    /// </summary>
    private void Awake()
    {
        //audio test, load music first

        audioSourceTheme = gameObject.AddComponent<AudioSource>();
        audioSourceThemeV = gameObject.AddComponent<AudioSource>();

        audioSourceA = gameObject.AddComponent<AudioSource>();
        audioSourceB = gameObject.AddComponent<AudioSource>();
        audioSourceC = gameObject.AddComponent<AudioSource>();
        audioSourceSFX = gameObject.AddComponent<AudioSource>();


        //audioSourceTheme.volume = 150;
        //audioSourceA.volume = 150;
        //audioSourceB.volume = 150;
        //audioSourceC.volume = 150;
        //audioSourceSFX.volume = 150;


        //缺省值，如果后续代码运行正常理论上不应该是这三个示范音乐
        string pathA = "Media/Sounds/BGM/PokemonDP/" + "201ばんどうろ（昼）";
        string pathB = "Media/Sounds/BGM/A Short Hike/06. A Short Flight";
        string pathC = "Media/Sounds/BGM/Flyff/08 Mystery";
        trackA = Resources.Load<AudioClip>(pathA);
        trackB = Resources.Load<AudioClip>(pathB);
        trackC = Resources.Load<AudioClip>(pathC);

        //jsonString = File.ReadAllText(jsonFilePath);
    }




    // Update is called once per frame



    int prevSceneIndex = -1;
    int prevState = -1;
    //public enum State
    //{
    //    Solid,
    //    Liquid,
    //    Gas
    //}

    void Update()
    {

        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

        
        //以下内容应该只对6个level有效。
        if (activeSceneIndex <= 7 && activeSceneIndex >= 2)
        {
            if (prevSceneIndex != -1)
            {
                //ManagerAudio.Instance.PlayA(trackA);
            }

            if (prevSceneIndex != activeSceneIndex)
            {
                prevSceneIndex = activeSceneIndex;

                int musicChoice = ((prevSceneIndex-2) % 5) + 1;//是因为目前只有5个loopBGM可切换。//注意序号应该从1开始因而+1//6个leve的序号为2-7，所以先要-2
                                                           //int musicStatus = 1;//1,2,3 for gas, solid and liquid;
                print("music choice" +musicChoice);
                string pathA = "Media/Sounds/BGM/Levels/Morph Levels v1 P" + musicChoice + "S1";
                string pathB = "Media/Sounds/BGM/Levels/Morph Levels v1 P" + musicChoice + "S2";
                string pathC = "Media/Sounds/BGM/Levels/Morph Levels v1 P" + musicChoice + "S3";
                trackA = Resources.Load<AudioClip>(pathA);
                trackB = Resources.Load<AudioClip>(pathB);
                trackC = Resources.Load<AudioClip>(pathC);
                ManagerAudio.Instance.PlayA(trackA);
                ManagerAudio.Instance.PlayB(trackB);
                ManagerAudio.Instance.PlayC(trackC);

            }
            //轨道中
            //A: Gas
            //B: Solid
            //C: Liquid

            //int musicStatus = 0;
            //int musicStatus = playerdata.pd.state;



            //int activeState = 1;
            int activeState = -2;
            if (PlayerData.Pd.state == State.Gas)
            {
                //print("1111");
                activeState = 1;

            }
            else if (PlayerData.Pd.state == State.Solid)
            {
                activeState = 2;

                //print("2222");
            }
            else
            {
                activeState = 3;

                //print("3333");
            }




            if (prevState != activeState)
            {
                if (prevState == -1)
                {
                    ManagerAudio.Instance.PlayA(trackA);
                    ManagerAudio.Instance.PlayB(trackB);
                    ManagerAudio.Instance.PlayC(trackC);
                }

                prevState = activeState;
                ManagerAudio.Instance.Volume0();


                if (activeState == 1)
                {
                    ManagerAudio.Instance.Volume100A();
                }
                else if (activeState == 2)
                {
                    ManagerAudio.Instance.Volume100B();
                }
                else
                {
                    ManagerAudio.Instance.Volume100C();
                }

            }
        }
        



























        //audioSourceA.volume = 0.5f; // 设置音量为50%
        //audioSourceA.pitch = 1.5f; // 设置音高为原来的1.5倍



        ////////////////////////////////
        // audio test - cont.
        //以下内容在融合到main后理论上没有作用还可能引发冲突，故注释掉。
        //if (Input.GetKeyDown(KeyCode.A))//PlayMusic
        //{

        //    ManagerAudio.Instance.PlayA(trackA);
        //}

        //if (Input.GetKeyDown(KeyCode.S))//PlayMusic
        //{

        //    ManagerAudio.Instance.PlayB(trackB);
        //}

        //if (Input.GetKeyDown(KeyCode.D))//PlayMusic
        //{

        //    ManagerAudio.Instance.PlayC(trackC);
        //}

        //if (Input.GetKeyDown(KeyCode.Q))//PauseMusic
        //{


        //    ManagerAudio.Instance.Pause();
        //}

        //if (Input.GetKeyDown(KeyCode.W))//PauseMusic
        //{


        //    ManagerAudio.Instance.Stop();
        //}
        //if (Input.GetKeyDown(KeyCode.Z))//PauseMusic
        //{


        //    ManagerAudio.Instance.PlaySFX("start");
        //}
        //if (Input.GetKeyDown(KeyCode.X))//PauseMusic
        //{


        //    ManagerAudio.Instance.PlaySFX("end");
        //}
        //if (Input.GetKeyDown(KeyCode.C))//PauseMusic
        //{


        //    ManagerAudio.Instance.PlaySFX("clear");
        //}




        ////////////////////////////////



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
