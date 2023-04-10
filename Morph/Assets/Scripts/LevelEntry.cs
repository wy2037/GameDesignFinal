using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class LevelEntry : MonoBehaviour
{
    [SerializeField] private bool isStart;
    private GameObject _player;
    public Transform playerPrefab;

    [SerializeField] private Animator _ani;
    [SerializeField] private int curScene;

    private void Awake() {
        _ani = GetComponent<Animator>();
        try{
            _player = GameObject.FindGameObjectWithTag("Player");
        }catch{
            _player = Instantiate(playerPrefab, transform.position, Quaternion.identity).gameObject;
        }
        
    }
    void Start()
    {

        if(isStart){
            StartCoroutine(loadStart());
        }
        curScene = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !isStart){
            StartCoroutine(loadNext());
        }
        
    }

    private IEnumerator loadStart(){
            _player.GetComponent<PlayerController>().enabled = false;
            _player.GetComponent<Rigidbody2D>().isKinematic = true;
        PlayerData.Pd.lastCheckedPosition = transform.position;
        PlayerData.Pd.lastCheckedTemperature = PlayerData.Pd.levelRoomTemperatures[SceneManager.GetActiveScene().buildIndex];
        _player.transform
        .DOMove(
            transform.position,
            0.1f
        );
        Debug.Log($"{_player.transform.position} {transform.position}");
        _player.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        
        _ani.SetBool("Open", true);

        yield return new WaitForSeconds(1f);
        _player.GetComponent<SpriteRenderer>()
        .DOFade(
            1f,
            1f
        )
        .OnComplete(()=>{
            _player.GetComponent<PlayerController>().enabled = true;
            _player.GetComponent<Rigidbody2D>().isKinematic = false;
            _ani.SetBool("Open", false);
        });


    }
    private IEnumerator loadNext(){
        DOTween.KillAll();
        _ani.SetBool("Open", true);
        _player.GetComponent<PlayerController>().enabled = false;
        _player.GetComponent<Rigidbody2D>().isKinematic = true;
        _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _player.transform
        .DOMove(
            transform.position,
            1f
        );
        yield return new WaitForSeconds(1f);
        _player.GetComponent<SpriteRenderer>()
        .DOFade(
            0f,
            1f
        );
        yield return new WaitForSeconds(1f);
        _ani.SetBool("Open", false);

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(curScene + 1);
    }
}
