using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelEntry : MonoBehaviour
{
    [SerializeField] private int curScene;
    void Start()
    {
        curScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            SceneManager.LoadScene(curScene + 1);
        }
        
    }
}
