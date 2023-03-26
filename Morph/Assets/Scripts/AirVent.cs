using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirVent : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player" && PlayerData.Pd.state == State.Gas) {
            Debug.Log("Gas player hit air vent!");
            // Call function in player to damage
        }
    }
}
