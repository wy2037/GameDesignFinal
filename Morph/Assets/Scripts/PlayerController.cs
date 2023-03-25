using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Transform feet;
    [SerializeField] private bool isGrounded;
    // layer
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask ceilingLayer;


    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float inputX;
    [SerializeField] private float InputY;
    [SerializeField] private int localDirection;
    
    private void Awake() {
        feet = transform.Find("feet");
        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Z)){
            PlayerData.Pd.state = State.Solid;
        }
        else if (Input.GetKeyDown(KeyCode.X)){
            PlayerData.Pd.state = State.Liquid;
        }
        else if (Input.GetKeyDown(KeyCode.C)){
            PlayerData.Pd.state = State.Gas;
        }


        switch(PlayerData.Pd.state){
            case State.Solid:
            {
                solidControl();
                break;
            }
            case State.Liquid:
            {
                liquidControl();
                break;
            }
            case State.Gas:
            {
                gasControl();
                break;
            }
            
        }
    }

    void solidControl(){
        Debug.Log("solid control");
        _rb.isKinematic = false;
        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        // change direction
        if(inputX == 0) localDirection = (int)transform.localScale.x;
        else if(inputX > float.Epsilon) localDirection = 1;
        else if(inputX < float.Epsilon) localDirection = -1;
        transform.localScale = new Vector3(localDirection, 1, 1);

        // set speed
        _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, _rb.velocity.y);
        if(checkGrounded() && Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("solid is grounded and gonna jump");
            _rb.velocity += new Vector2(0, PlayerData.Pd.jumpForce);
        }
    }

    void liquidControl(){
        _rb.isKinematic = true;
    }

    void gasControl(){

    }

    bool checkGrounded(){
        isGrounded = Physics2D.Raycast(feet.position, -transform.up, 0.2f, groundLayer);
        return isGrounded;
    }
}
