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

    public void Level1Button(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
    public void Level2Button(){
        SceneManager.LoadScene(3);
        Time.timeScale = 1f;
    }
    public void Level3Button(){
        SceneManager.LoadScene(4);
    }
    public void Level4Button(){
        SceneManager.LoadScene(5);
    }
    public void Level5Button(){
        SceneManager.LoadScene(6);
    }
    public void Level6Button(){
        SceneManager.LoadScene(7);
    }

    public void RestartButton(){
        //SceneManager.LoadScene("CharlesTest");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
