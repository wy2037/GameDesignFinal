using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    public float windForce;
    public Vector2 windDirection;
    public void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.name == "Player" && PlayerData.Pd.state == State.Gas) {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(windDirection * windForce);
            // call player funtion to cool down
        }
    }
}
