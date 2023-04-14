using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerKeys : MonoBehaviour
{
    [SerializeField] private Transform left, mid, right, leftPressed, midPressed, rightPressed;
    [SerializeField] private bool isEnabled;

    private Transform _player;
    private PlayerController _pc;

    [SerializeField] private int activatedKeyNum;


    private HashSet<KeyCode> keysHori = new HashSet<KeyCode>{ KeyCode.A, KeyCode.D };
    private HashSet<KeyCode> keysVerti = new HashSet<KeyCode>{ KeyCode.W, KeyCode.S};
    int horiNum = 0, vertiNum = 0;
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
        isEnabled = true;

        
    }

    // Update is called once per frame
    void Update()
    {
        // horiNum = keysHori.Count(key => Input.GetKey(key));
        // VertiNum = keysVerti.Count(key=> Input.GetKey(key));
        foreach(var key in keysHori){
            if(Input.GetKeyDown(key)) ++horiNum;
            if(Input.GetKeyUp(key)) --horiNum;
        }
        foreach(var key in keysVerti){
            if(Input.GetKeyDown(key)) ++vertiNum;
            if(Input.GetKeyUp(key)) --vertiNum;
        }

        //Debug.Log($"key: {horiNum} {VertiNum}");
        switch (PlayerData.Pd.state){

            case State.Solid:
                if(!midPressed.gameObject.activeSelf && !mid.gameObject.activeSelf) midPressed.gameObject.SetActive(true);
                if(transform.localPosition.y != -0.64) transform.localPosition = new Vector3(0f, -0.64f, 0f);

                if(Input.GetKeyDown(KeyCode.Space)){
                    Debug.Log("space press");
                    pressSpace();
                }
                if(Input.GetKeyUp(KeyCode.Space)){
                    Debug.Log("space release");
                    releaseSpace();
                }
                switch (horiNum){
                    case 0:
                        releaseLeft();
                        releaseRight();
                        break;
                    case 1:
                        pressRight();
                        releaseLeft();
                        break;
                    case 2:
                        pressRight();
                        pressLeft();
                        break;
                }
                break;
            case State.Gas:
                if(!midPressed.gameObject.activeSelf && !mid.gameObject.activeSelf) midPressed.gameObject.SetActive(true);
                if(transform.localPosition.y != 0.32f) transform.localPosition = new Vector3(0f, 0.32f, 0f);
                switch (horiNum){
                    case 0:
                        releaseLeft();
                        releaseRight();
                        break;
                    case 1:
                        pressRight();
                        releaseLeft();
                        break;
                    case 2:
                        pressRight();
                        pressLeft();
                        break;
                }
                if(Input.GetKeyDown(KeyCode.Space)){
                    pressSpace();
                }else if(Input.GetKeyUp(KeyCode.Space)){
                    releaseSpace();
                }
                break;
            case State.Liquid:
                if(_pc.isRotating && isEnabled){
                    isEnabled = false;
                    disableAll();
                }else if(!_pc.isRotating && !isEnabled){
                    isEnabled = true;
                }

                if(isEnabled){
                    if(midPressed.gameObject.activeSelf) midPressed.gameObject.SetActive(false);
                    if(mid.gameObject.activeSelf) mid.gameObject.SetActive(false);
                    if(_pc.isHorizontal){
                        switch (horiNum){
                            case 0:
                                releaseLeft();
                                releaseRight();
                                break;
                            case 1:
                                pressRight();
                                releaseLeft();
                                break;
                            case 2:
                                pressRight();
                                pressLeft();
                                break;
                        }
                    }else{
                        switch (vertiNum){
                            case 0:
                                releaseLeft();
                                releaseRight();
                                break;
                            case 1:
                                pressRight();
                                releaseLeft();
                                break;
                            case 2:
                                pressRight();
                                pressLeft();
                                break;
                        }
                    }
                }
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
        mid.gameObject.SetActive(false);
        midPressed.gameObject.SetActive(true);
    }

    void releaseSpace(){
        midPressed.gameObject.SetActive(false);
        mid.gameObject.SetActive(true);
    }


    void pressRight(){
        right.gameObject.SetActive(false);
        rightPressed.gameObject.SetActive(true);
    }

    void releaseRight(){
        rightPressed.gameObject.SetActive(false);
        right.gameObject.SetActive(true);
    }

    void disableAll(){
        left.gameObject.SetActive(false);
        leftPressed.gameObject.SetActive(false);
        mid.gameObject.SetActive(false);
        midPressed.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        rightPressed.gameObject.SetActive(false);
    }

    void enableLiquid(){
        left.gameObject.SetActive(true);
        leftPressed.gameObject.SetActive(true);
        right.gameObject.SetActive(true);
        rightPressed.gameObject.SetActive(true);
    }
}
