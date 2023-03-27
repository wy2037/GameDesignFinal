using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    public float windForce;
    public Vector2 windDirection;
    private Transform _player;
    private Rigidbody2D _playerRb;
    private void Awake() {
        _player = GameObject.FindWithTag("Player").transform;
        _playerRb = _player.GetComponent<Rigidbody2D>();
    }
    public void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && (PlayerData.Pd.state == State.Gas || PlayerData.Pd.state == State.Liquid)) {
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(windDirection * windForce);
            _playerRb.AddForce(windDirection * windForce);
            //_player.Translate(windDirection * windForce);

            // call player funtion to cool down
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && PlayerData.Pd.state == State.Liquid){
            _player.GetComponent<PlayerController>().liquidDrop();
        } 
    }
}
