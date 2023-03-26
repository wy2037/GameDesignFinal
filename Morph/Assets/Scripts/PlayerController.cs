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
    [SerializeField] private bool isAttached;
    [SerializeField] private bool isWallHit;
    [SerializeField] private bool isCornerMet;
    [SerializeField] private bool isRotating;
    [SerializeField] private bool isHorizontal;
    // layer
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask ceilingLayer;

    // component
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _col;
    [SerializeField] private SpriteRenderer _sr;

    [SerializeField] private float distanceToSurface;
    [SerializeField] private float inputX;
    [SerializeField] private float inputY;
    [SerializeField] private int localDirection; // left or right

    // sprite
    public Sprite solidSprite;
    public Sprite liquidSprite; 
    public Sprite gasSprite;
    
    private void Awake() {
        center = transform.Find("center");
        feet = transform.Find("feet");
        right = transform.Find("right");

        distanceToSurface = Vector2.Distance(center.position, feet.position);

        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _col = GetComponent<Collider2D>();
        _sr = GetComponent<SpriteRenderer>();

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
                _sr.sprite = solidSprite;
                solidControl();
                break;
            }
            case State.Liquid:
            {
                _sr.sprite = liquidSprite;
                liquidControl();
                break;
            }
            case State.Gas:
            {
                _sr.sprite = gasSprite;
                gasControl();
                break;
            }
            
        }
    }

    void solidControl(){
        // initiate
        Debug.Log("solid control");
        _rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.gravityScale = PlayerData.Pd.gravityScale;
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
        //initiate
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.None;
        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        changeDirection();
        Vector2 wallHitPos = checkWall();
        Vector2 cornerHitPos = checkCorner();
        // wall rotate
        if(wallHitPos != Vector2.zero && !isRotating){
            isRotating = true;
            Debug.Log(wallHitPos);
            Debug.Log(transform.right);
            Vector2 endCenter = ((Vector2)center.position - wallHitPos).normalized * distanceToSurface + wallHitPos;
            Debug.Log(endCenter);
            _col.enabled = false;
            transform
            .DOMove(
                endCenter,
                0.2f
            );
            transform
            .DOLocalRotate(
                new Vector3(0, 0, 90 * localDirection),
                0.2f
            )
            .SetRelative()
            .OnComplete(()=>{
                isRotating = false;
                _col.enabled = true;
                isHorizontal = !isHorizontal;
            });
        }
        // corner rotate
        if(cornerHitPos != Vector2.zero & !isRotating){
            isRotating = true;
            Vector2 endCenter = (Vector2)transform.right * (localDirection) * distanceToSurface + cornerHitPos;
            Debug.Log(endCenter);
            _col.enabled = false;
            transform
            .DOMove(
                endCenter,
                0.2f
            );
            transform
            .DOLocalRotate(
                new Vector3(0, 0, -90 * localDirection),
                0.2f
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
            if(isHorizontal && (feet.position.y < center.position.y)){
                transform.Translate(inputX * 0.05f, 0, 0);
            }
            else if(isHorizontal && (feet.position.y > center.position.y)){
                transform.Translate(-inputX * 0.05f, 0, 0);
            }
            else if(!isHorizontal && (feet.position.x > center.position.x)){
                transform.Translate(inputY * 0.05f, 0, 0);
            }
            else if (!isHorizontal && (feet.position.x < center.position.x)){
                transform.Translate(-inputY * 0.05f, 0, 0);
            }
        }

    }

    void gasControl(){
        // initialize
        _rb.gravityScale = -PlayerData.Pd.gravityScale;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        // change direction
        changeDirection();
        // set speed
        _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, _rb.velocity.y);
    }

    void changeDirection(){
        // static
        if(inputX == 0 && isHorizontal) localDirection = (int)transform.localScale.x;
        else if(inputY == 0 && !isHorizontal) localDirection = (int)transform.localScale.x;
        // horizontal
        else if(inputX > float.Epsilon && isHorizontal && (feet.position.y < center.position.y)) localDirection = 1;
        else if(inputX > float.Epsilon && isHorizontal && (feet.position.y > center.position.y)) localDirection = -1;
        else if(inputX < float.Epsilon && isHorizontal && (feet.position.y < center.position.y)) localDirection = -1;
        else if(inputX < float.Epsilon && isHorizontal && (feet.position.y > center.position.y)) localDirection = 1;
        // vertical
        else if(inputY > float.Epsilon && !isHorizontal && (feet.position.x > center.position.x)) localDirection = 1;
        else if(inputY > float.Epsilon && !isHorizontal && (feet.position.x < center.position.x)) localDirection = -1;
        else if(inputY < float.Epsilon && !isHorizontal && (feet.position.x > center.position.x)) localDirection = -1;
        else if(inputY < float.Epsilon && !isHorizontal && (feet.position.x < center.position.x)) localDirection = 1;
        transform.localScale = new Vector3(localDirection, 1, 1);
    }

    bool checkGrounded(){
        isGrounded = Physics2D.Raycast(feet.position, -transform.up, 0.2f, groundLayer);
        return isGrounded;
    }

    bool checkAttached(){
        int combinedMask = wallLayer | ceilingLayer | groundLayer;
        isAttached = Physics2D.Raycast(feet.position, -transform.up, 0.1f, combinedMask);
        return isAttached;
    }

    Vector2 checkWall(){
        if(isRotating) return Vector2.zero;
        int combinedMask = wallLayer | ceilingLayer | groundLayer;
        
        RaycastHit2D hit = Physics2D.Raycast(right.position, transform.right, 0.1f, combinedMask);
        isWallHit = (hit.collider == null) ? false:true;
        //Debug.DrawRay(transform.position, transform.right * Mathf.Infinity, Color.red);
        return (isWallHit)? hit.point : Vector2.zero;
    }

    Vector2 checkCorner(){
        if(isRotating) return Vector2.zero;
        int combinedMask = wallLayer | ceilingLayer | groundLayer;
        RaycastHit2D hit = new RaycastHit2D();
        if(!checkAttached()){
            hit = Physics2D.Raycast(feet.position, (-transform.right * localDirection - transform.up).normalized, 0.1f, combinedMask);
            Debug.DrawLine(feet.position, feet.position + (-transform.right - transform.up).normalized, Color.red);
            Debug.Log($"corner: {((hit.collider == null) ? false : true)}");
            isCornerMet = (hit.collider == null) ? false : true;
        }
        return (isCornerMet) ? hit.point : Vector2.zero; 
    }
}
