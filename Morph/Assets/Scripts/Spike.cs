using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && PlayerData.Pd.state == State.Solid) {
            Debug.Log("Solid player hit spike!");
            // Call function in player to damage
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
    }
}
