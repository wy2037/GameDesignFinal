using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ManagerAudio ma = ManagerAudio.Instance;
        ma.PlayTheme();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            //Debug.Log($"{SceneManager.GetActiveScene().buildIndex}");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            //SceneManager.LoadScene("World Map");
            //ManagerScene.Instance.GoToScene("World Map");
            ManagerScene.Instance.GoToScene("Level 1");
        }
    }
}

