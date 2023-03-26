using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeInteractPoint : MonoBehaviour
{
    public Transform connectedPipe;
    public void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.name == "Player") {
            collision.gameObject.transform.position = connectedPipe.position;
        }
    }
}
