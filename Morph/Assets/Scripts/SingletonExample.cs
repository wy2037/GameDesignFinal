using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;



public class SingletonExample : MonoBehaviour
{
    private static SingletonExample _instance;
    public static SingletonExample Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SingletonExample>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<SingletonExample>();
                    singletonObject.name = typeof(SingletonExample).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }


    //Part1: data manager

    private int _score;
    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }


    private int _temperature;
    public int Temperature
    {
        get { return _temperature; }
        set { _temperature = value; }
    }


    //not finished if json is needed may add support later

    //private string jsonFilePath = "Assets/MISC/JsonFile.json";
    //private string jsonString;
    //public string GetJsonString()
    //{
    //    return jsonString;
    //}



    //Part2: audio manager

    public AudioSource audioSource;
    public AudioClip testTemp;
    public AudioClip test1;
    public AudioClip test2;
    public AudioClip test3;
    public AudioClip test4;


    

    public void PlayMusic(AudioClip music)
    {
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }




    //Part3: scene manager
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }










    private void Awake()
    {
        //audio test, load music first

        audioSource = gameObject.AddComponent<AudioSource>();
        string pathTemp = "Media/Sounds/BGM/PokemonDP/" + "201ばんどうろ（昼）";
        testTemp = Resources.Load<AudioClip>(pathTemp);



        //jsonString = File.ReadAllText(jsonFilePath);
    }


    // Start is called before the first frame update
    void Start()
    {

        //data test
        SingletonExample.Instance.Score = 100;
        Debug.Log("Your Score: " + SingletonExample.Instance.Score);

    }

    // Update is called once per frame
    void Update()
    {

        //change scene test

        if (Input.GetKeyDown(KeyCode.E))//Enter Homepage
        {
            //SceneManager.LoadScene("HomePage");
            SingletonExample.Instance.GoToScene("HomePage");
        }



        // audio test - cont.
        if (Input.GetKeyDown(KeyCode.P))//PlayMusic
        {

            
            SingletonExample.Instance.PlayMusic(testTemp);
        }

        if (Input.GetKeyDown(KeyCode.O))//PauseMusic
        {


             SingletonExample.Instance.PauseMusic();
        }

        if (Input.GetKeyDown(KeyCode.I))//PauseMusic
        {


            SingletonExample.Instance.StopMusic();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))//ChangeMusic: Canadian
        {
            testTemp = test1;
            SingletonExample.Instance.PlayMusic(testTemp);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))//ChangeMusic: Korean
        {
            testTemp = test2;
            print("here");
            SingletonExample.Instance.PlayMusic(testTemp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//ChangeMusic: Nipponese
        {
            testTemp = test3;
            SingletonExample.Instance.PlayMusic(testTemp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))//ChangeMusic: Sinic
        {
            testTemp = test4;
            SingletonExample.Instance.PlayMusic(testTemp);
        }    


    }



}




