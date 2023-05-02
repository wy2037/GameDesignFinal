using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (Paused){
                Play();
            }
            else{
                Stop();
            }
        }
    }

    void Stop(){
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void Play(){
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void MainMenuButton(){
        SceneManager.LoadScene("Sample");
    }

    public void SandboxButton(){
        SceneManager.LoadScene("CharlesTest");
    }
    public void TutorialButton(){
        SceneManager.LoadScene("Tutorial");
    }

    // public void RestartButton(){
    //     //SceneManager.LoadScene("CharlesTest");
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }

    // function to restart level
    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        PauseMenuCanvas.SetActive(false);
        Paused = false;
        Time.timeScale = 1f;
    }


    // function to load level based on index
    public void LoadLevel(int levelIndex){
        SceneManager.LoadScene(levelIndex);
        PauseMenuCanvas.SetActive(false);
        Paused = false;
        Time.timeScale = 1f;
    }
    // public void Level1Button(){
    //     SceneManager.LoadScene(2);
    //     PauseMenuCanvas.SetActive(false);
    //     Paused = false;
    //     Time.timeScale = 1f;
    // }
    // public void Level2Button(){
    //     SceneManager.LoadScene(3);
    //     PauseMenuCanvas.SetActive(false);
    //     Paused = false;
    //     Time.timeScale = 1f;
    // }
    // public void Level3Button(){
    //     SceneManager.LoadScene(4);
    //     PauseMenuCanvas.SetActive(false);
    //     Time.timeScale = 1f;
    // }
    // public void Level4Button(){
    //     SceneManager.LoadScene(5);
    //     PauseMenuCanvas.SetActive(false);
    //     Time.timeScale = 1f;
    // }
    // public void Level5Button(){
    //     SceneManager.LoadScene(6);
    //     PauseMenuCanvas.SetActive(false);
    //     Time.timeScale = 1f;
    // }
    // public void Level6Button(){
    //     SceneManager.LoadScene(7);
    //     PauseMenuCanvas.SetActive(false);
    //     Time.timeScale = 1f;
    // }
}
