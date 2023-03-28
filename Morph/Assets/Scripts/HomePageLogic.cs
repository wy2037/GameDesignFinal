using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Your Score: From Home Page Start：" + SingletonExample.Instance.Score);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))//Get
        {
            Debug.Log("Your Score: From Home Page Update：" + SingletonExample.Instance.Score);
        }

        if (Input.GetKeyDown(KeyCode.S))//Set
        {
            SingletonExample.Instance.Score = 200;
            Debug.Log("Your Score: From Home Page Update 200：" + SingletonExample.Instance.Score);

        }

        if (Input.GetKeyDown(KeyCode.B))//Back to VeryFirstPage
        {
            SingletonExample.Instance.GoToScene("VeryFirstPage");
        }
        
    }
}
