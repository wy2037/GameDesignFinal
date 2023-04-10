using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    float respawnTime;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            //PlayerData.Pd.temperature = RoomTemperature.Rt.roomTemperature;
            PlayerData.Pd.temperature = PlayerData.Pd.lastCheckedTemperature;
            Invoke("Respawn", respawnTime);
        }
    }

    void Respawn() {
        player.position = PlayerData.Pd.lastCheckedPosition;
    }
}
