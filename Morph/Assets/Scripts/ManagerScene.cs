using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class ManagerScene : MonoBehaviour
{
    private static ManagerScene _instance2;
    public static ManagerScene Instance
    {
        get
        {
            if (_instance2 == null)
            {
                _instance2 = FindObjectOfType<ManagerScene>();
                //DontDestroyOnLoad(_instance2.gameObject);

                if (_instance2 == null)
                {
                    GameObject singletonObject = new GameObject();
                    _instance2 = singletonObject.AddComponent<ManagerScene>();
                    singletonObject.name = typeof(ManagerScene).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return _instance2;
        }
    }




    public float firstTimeAtMap;
    

    //switch scenes by name and index


    public float getFlag()
    {
        return firstTimeAtMap;
    }
    public void setFlag(float num)
    {
        firstTimeAtMap = num;
    }



    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public string GetName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public float GetIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }


    // Start is called before the first frame update
    void Start()
    {
        firstTimeAtMap = 1f;//第一次在世界地图1，已经访问过则为0
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))//Enter Homepage
        //{
        //    //SceneManager.LoadScene("HomePage");
        //    //ManagerScene.Instance.GoToScene("HomePage");
        //    ManagerScene.Instance.GoToScene("Level 0");
        //}

        //if (Input.GetKeyDown(KeyCode.L))//Enter Homepage
        //{
        //    //SceneManager.LoadScene("HomePage");
        //    //ManagerScene.Instance.GoToScene("HomePage");
        //    ManagerScene.Instance.GoToScene("VeryFirstPage");
        //}
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    ManagerScene.Instance.GoToScene("Level 2");
        //}
    }
}
