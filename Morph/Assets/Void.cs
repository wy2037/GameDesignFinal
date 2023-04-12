using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Void : MonoBehaviour
{
    public Transform player;

    [SerializeField] private Transform _player;
    
    float respawnTime;


    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;    
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            //PlayerData.Pd.temperature = RoomTemperature.Rt.roomTemperature;
            //PlayerData.Pd.temperature = PlayerData.Pd.lastCheckedTemperature;
            //Invoke("Respawn", respawnTime);
            StartCoroutine(_player.GetComponent<PlayerController>().die());
        }
    }

    void Respawn() {
        player.position = PlayerData.Pd.lastCheckedPosition;
    }


    // private IEnumerator die(){
    //     _player.GetComponent<PlayerController>().enabled = false;
    //     _player.GetComponent<Rigidbody2D>().isKinematic = true;
    //     _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //     _player.GetComponent<Collider2D>().enabled = false;
    //     var sr = _player.GetComponent<SpriteRenderer>();
    //     sr.color = Color.red;
    //     sr
    //     .DOFade(
    //         0,
    //         1f
    //     );

    //     _player
    //     .DOLocalJump(
    //         Vector3.right * (Random.Range(0, 1) == 1? 1 : -1),
    //         1,
    //         1,
    //         1f
    //     )
    //     .SetRelative();

    //     yield return new WaitForSeconds(1.1f);

    //     _player
    //     .DOMove(
    //         PlayerData.Pd.lastCheckedPosition,
    //         0.1f
    //     );


    //     // after moving
    //     _player.rotation = Quaternion.identity;
    //     PlayerData.Pd.temperature = PlayerData.Pd.lastCheckedTemperature;
    //     sr.color = Color.white;

    //     for (int i = 0; i < 3; i++)
    //     {
    //         sr.color = new Color(1, 1, 1, 1);
    //         yield return new WaitForSeconds(0.2f);
    //         sr.color = new Color(1, 1, 1, 0);
    //         yield return new WaitForSeconds(0.2f);
    //     }
    //     sr.color = new Color(1, 1, 1, 1);
    //     _player.GetComponent<Rigidbody2D>().isKinematic = false;
    //     _player.GetComponent<PlayerController>().enabled = true;
    //     _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //     _player.GetComponent<Collider2D>().enabled = true;
    //     if(PlayerData.Pd.state == State.Liquid) StartCoroutine(_player.GetComponent<PlayerController>().liquidDrop2());
    // }
}
