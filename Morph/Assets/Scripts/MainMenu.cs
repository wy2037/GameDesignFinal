using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

    public GameObject MainMenuCanvas;

    

    public void PlayButton(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }


    public void LevelsButton(){
        SceneManager.LoadScene("CharlesTest");
    }
    public void SettingsButton(){
        SceneManager.LoadScene("Tutorial");
    }

    
}
