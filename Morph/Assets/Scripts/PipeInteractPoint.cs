using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeInteractPoint : MonoBehaviour
{
    public Transform connectedPipe;
    public void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.name == "Player" && connectedPipe != null) {
            collision.gameObject.transform.position = connectedPipe.position + connectedPipe.up * 0.2f;
            PlayerData.Pd.lastCheckedPosition = collision.gameObject.transform.position;
            PlayerData.Pd.lastCheckedTemperature = PlayerData.Pd.temperature;
            if(PlayerData.Pd.state == State.Liquid) StartCoroutine(collision.gameObject.GetComponent<PlayerController>().liquidDrop2());
        }
    }
}
