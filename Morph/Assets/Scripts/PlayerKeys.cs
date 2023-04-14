using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerKeys : MonoBehaviour
{
    private Transform left, mid, right, leftPressed, midPressed, rightPressed;

    private Transform _player;
    private PlayerController _pc;

    [SerializeField] private int activatedKeyNum;


    private HashSet<KeyCode> keysHori = new HashSet<KeyCode>{ KeyCode.A, KeyCode.D };
    private HashSet<KeyCode> keysVerti = new HashSet<KeyCode>{ KeyCode.W, KeyCode.S};
    int horiNum, VertiNum;
    void Start()
    {
        _player = transform.parent;
        _pc = _player.GetComponent<PlayerController>();
        left = transform.Find("left");
        mid = transform.Find("middle");
        right = transform.Find("right");
        leftPressed = transform.Find("left_pressed");
        midPressed = transform.Find("middle_pressed");
        rightPressed = transform.Find("right_pressed");

        
    }

    // Update is called once per frame
    void Update()
    {
        horiNum = keysHori.Count(key => Input.GetKey(key));
        VertiNum = keysVerti.Count(key=> Input.GetKey(key));

        Debug.Log($"key: {horiNum} {VertiNum}");
        switch (PlayerData.Pd.state){

            case State.Solid:
                if(!midPressed.gameObject.activeSelf) midPressed.gameObject.SetActive(true);
                if(!mid.gameObject.activeSelf) mid.gameObject.SetActive(true);
                if(Input.GetKeyDown(KeyCode.A)){
                    pressLeft();
                }
                if(Input.GetKeyUp(KeyCode.A)){
                    releaseLeft();
                }
                if(Input.GetKeyDown(KeyCode.Space)){
                    pressSpace();
                }
                if(Input.GetKeyUp(KeyCode.Space)){
                    releaseSpace();
                }
                if(Input.GetKeyDown(KeyCode.D)){
                    pressRight();
                }
                if(Input.GetKeyUp(KeyCode.D)){
                    releaseRight();
                }
                break;
            case State.Gas:
                if(!midPressed.gameObject.activeSelf) midPressed.gameObject.SetActive(true);
                if(!mid.gameObject.activeSelf) mid.gameObject.SetActive(true);
                if(Input.GetKeyDown(KeyCode.A)){
                    pressLeft();
                }
                if(Input.GetKeyUp(KeyCode.A)){
                    releaseLeft();
                }
                if(Input.GetKeyDown(KeyCode.Space)){
                    pressSpace();
                }
                if(Input.GetKeyUp(KeyCode.Space)){
                    releaseSpace();
                }
                if(Input.GetKeyDown(KeyCode.D)){
                    pressRight();
                }
                if(Input.GetKeyUp(KeyCode.D)){
                    releaseRight();
                }
                break;
            case State.Liquid:
                if(midPressed.gameObject.activeSelf) midPressed.gameObject.SetActive(false);
                if(mid.gameObject.activeSelf) mid.gameObject.SetActive(false);
                break;
        }
    }



    void pressLeft(){
        left.gameObject.SetActive(false);
        leftPressed.gameObject.SetActive(true);
    }

    void releaseLeft(){
        leftPressed.gameObject.SetActive(false);
        left.gameObject.SetActive(true);
    }

    void pressSpace(){
        midPressed.gameObject.SetActive(true);
        mid.gameObject.SetActive(false);
    }

    void releaseSpace(){
        mid.gameObject.SetActive(true);
        midPressed.gameObject.SetActive(false);
    }


    void pressRight(){
        rightPressed.gameObject.SetActive(true);
        right.gameObject.SetActive(false);
    }

    void releaseRight(){
        right.gameObject.SetActive(true);
        rightPressed.gameObject.SetActive(false);
    }
}
