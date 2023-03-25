using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    // children
    private Transform feet;
    private Transform right;
    private Transform center;

    // bool
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isWallHit;
    [SerializeField] private bool isRotating;
    [SerializeField] private bool isHorizontal;
    // layer
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask ceilingLayer;

    // component
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _col;

    [SerializeField] private float distanceToSurface;
    [SerializeField] private float inputX;
    [SerializeField] private float inputY;
    [SerializeField] private int localDirection;
    
    private void Awake() {
        center = transform.Find("center");
        feet = transform.Find("feet");
        right = transform.Find("right");

        distanceToSurface = Vector2.Distance(center.position, feet.position);

        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _col = GetComponent<Collider2D>();

        // bool
        isHorizontal = true; 
    }
    private void Update() {
        // temperate way of switching state
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
        changeDirection();
        // set speed
        _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, _rb.velocity.y);
        if(checkGrounded() && Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("solid is grounded and gonna jump");
            _rb.velocity += new Vector2(0, PlayerData.Pd.jumpForce);
        }

    }

    void liquidControl(){
        _rb.isKinematic = true;
        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        changeDirection();
        Vector2 hitPos = checkWall();
        if(hitPos != Vector2.zero && !isRotating){
            isRotating = true;
            Debug.Log(hitPos);
            Debug.Log(transform.right);
            Vector2 endCenter = hitPos - (Vector2)transform.right * distanceToSurface;
            Debug.Log(endCenter);
            _col.enabled = false;
            transform
            .DOMove(
                endCenter,
                3f
            );
            transform
            .DOLocalRotate(
                new Vector3(0, 0, 90),
                3f
            )
            .SetRelative()
            .OnComplete(()=>{
                isRotating = false;
                _col.enabled = true;
                isHorizontal = !isHorizontal;
            });
        }
        // movement
        if(!isRotating){
            if(isHorizontal){
                transform.Translate(inputX * 0.05f, 0, 0);
            }else{
                transform.Translate(inputY * 0.05f, 0, 0);
            }
        }

    }

    void gasControl(){

    }

    void changeDirection(){
        if(inputX == 0 && isHorizontal) localDirection = (int)transform.localScale.x;
        else if(inputY == 0 && !isHorizontal) localDirection = (int)transform.localScale.x;
        else if(inputX > float.Epsilon && isHorizontal) localDirection = 1;
        else if(inputX < float.Epsilon && isHorizontal) localDirection = -1;
        else if(inputY > float.Epsilon && !isHorizontal) localDirection = 1;
        else if(inputY < float.Epsilon && !isHorizontal) localDirection = -1;
        transform.localScale = new Vector3(localDirection, 1, 1);
    }

    bool checkGrounded(){
        isGrounded = Physics2D.Raycast(feet.position, -transform.up, 0.2f, groundLayer);
        return isGrounded;
    }

    Vector2 checkWall(){
        if(isRotating) return Vector2.zero;
        int combinedMask = wallLayer | ceilingLayer | groundLayer;
        
        RaycastHit2D hit = Physics2D.Raycast(right.position, transform.right, 0.2f, combinedMask);
        isWallHit = (hit.collider == null) ? false:true;
        //Debug.DrawRay(transform.position, transform.right * Mathf.Infinity, Color.red);
        return (isWallHit)? hit.point : Vector2.zero;
    }

    bool checkVoid(){
        return false;
    }
}
