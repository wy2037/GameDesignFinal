using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ManagerScene : MonoBehaviour
{
    private static ManagerScene _instance;
    public static ManagerScene Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ManagerScene>();
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<ManagerScene>();
                    singletonObject.name = typeof(ManagerScene).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance;
        }
    }


    //switch scenes by name and index
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))//Enter Homepage
        {
            //SceneManager.LoadScene("HomePage");
            //ManagerScene.Instance.GoToScene("HomePage");
            ManagerScene.Instance.GoToScene("Level 0");
        }

        if (Input.GetKeyDown(KeyCode.L))//Enter Homepage
        {
            //SceneManager.LoadScene("HomePage");
            //ManagerScene.Instance.GoToScene("HomePage");
            ManagerScene.Instance.GoToScene("VeryFirstPage");
        }
    }
}
