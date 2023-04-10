using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AirVent : MonoBehaviour
{

    private Transform _player;

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Player" && PlayerData.Pd.state == State.Gas) {
            Debug.Log("Gas player hit air vent!");
            // Call function in player to damage
            StartCoroutine(die());
        }
    }


    private IEnumerator die(){
        _player.GetComponent<PlayerController>().enabled = false;
        _player.GetComponent<Rigidbody2D>().isKinematic = true;
        _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        var sr = _player.GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        sr
        .DOFade(
            0,
            1f
        );

        _player
        .DOLocalJump(
            Vector3.right * (Random.Range(0, 1) == 1? 1 : -1),
            1,
            1,
            1f
        )
        .SetRelative();

        yield return new WaitForSeconds(1.1f);

        _player
        .DOMove(
            PlayerData.Pd.lastCheckedPosition,
            0.1f
        );
        PlayerData.Pd.temperature = PlayerData.Pd.lastCheckedTemperature;
        sr.color = Color.white;

        for (int i = 0; i < 3; i++)
        {
            sr.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            sr.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.2f);
        }
        sr.color = new Color(1, 1, 1, 1);
        _player.GetComponent<Rigidbody2D>().isKinematic = false;
        _player.GetComponent<PlayerController>().enabled = true;
        _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        
    }
}
