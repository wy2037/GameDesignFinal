using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AirVent : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player" && PlayerData.Pd.state == State.Gas) {
            Debug.Log("Gas player hit air vent!");
            // Call function in player to damage
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
    }
}
