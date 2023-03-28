using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    // for testing
    bool stateFlag = true;
    // children
    private Transform head;
    private Transform feet;
    private Transform right;
    private Transform center;

    // bool
    [SerializeField] private bool isAttached;
    [SerializeField] private bool isCeiling;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isWallHit;
    [SerializeField] private bool isCornerMet;
    [SerializeField] private bool isRotating;
    [SerializeField] private bool isHorizontal;
    // layer
    public LayerMask groundLayer;

    // component
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BoxCollider2D _col;
    [SerializeField] private SpriteRenderer _sr;

    [SerializeField] private float distanceToSurface;
    [SerializeField] private float inputX;
    [SerializeField] private float inputY;
    [SerializeField] private int localDirection; // left or right

    // sprite
    public Sprite solidSprite;
    public Sprite liquidSprite; 
    public Sprite gasSprite;

    // temperature changing points
    [SerializeField] private int solidToLiquid;
    [SerializeField] private int liquidToGas;
    
    private void Awake() {
        head = transform.Find("head");
        center = transform.Find("center");
        feet = transform.Find("feet");
        right = transform.Find("right");

        distanceToSurface = Vector2.Distance(center.position, feet.position);

        _rb = GetComponent<Rigidbody2D>();
        _rb.isKinematic = true;
        _col = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();

        // bool
        isHorizontal = true; 
        stateFlag = true;
    }
    private void Update() {

        if(stateFlag){
            // temperate way of switching state
            if(Input.GetKeyDown(KeyCode.Z)){
                solidInit();
            }
            else if (Input.GetKeyDown(KeyCode.X)){
                liquidInit();
            }
            else if (Input.GetKeyDown(KeyCode.C)){
                gasInit();
            }
            else if(Input.GetKeyDown(KeyCode.V)){
                StartCoroutine(liquidDrop2());
            }
        }else{
            if(PlayerData.Pd.temperature <= solidToLiquid){
                solidInit();
            }
            else if (PlayerData.Pd.temperature > solidToLiquid && PlayerData.Pd.temperature <= liquidToGas){
                liquidInit();
            }
            else if (PlayerData.Pd.temperature > liquidToGas){
                gasInit();
            }
            else if(Input.GetKeyDown(KeyCode.V)){
                StartCoroutine(liquidDrop2());
            }
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


    void solidInit(){
        this.gameObject.layer = LayerMask.NameToLayer("PlayerS");
        PlayerData.Pd.state = State.Solid;

        _col.offset = new Vector2(0,0.01f);
        _col.size = new Vector2(0.64f,0.66f);
        
        _rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.identity;
        isHorizontal = true;
        _rb.gravityScale = PlayerData.Pd.gravityScale;

    }
    void liquidInit(){
        this.gameObject.layer = LayerMask.NameToLayer("PlayerL");
        PlayerData.Pd.state = State.Liquid;
        
        _col.offset = new Vector2(0,-0.115f);
        _col.size = new Vector2(0.64f,0.41f);

        _rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints2D.None;
        StartCoroutine(liquidDrop2());
    }
    void gasInit(){
        this.gameObject.layer = LayerMask.NameToLayer("PlayerG");
        PlayerData.Pd.state = State.Gas;

        _col.offset = new Vector2(0, 0.09f);
        _col.size = new Vector2(0.64f, 0.46f);
        
        _rb.isKinematic = false;
        _rb.gravityScale = -PlayerData.Pd.gravityScale;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isHorizontal = true;

    }
    void solidControl(){
        // initiate
        //Debug.Log("solid control");
        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        // change direction
        changeDirection();
        // set speed
        _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, _rb.velocity.y);
        if(checkAttached() && Input.GetKeyDown(KeyCode.Space)){
            //Debug.Log("solid is grounded and gonna jump");
            _rb.velocity += new Vector2(0, PlayerData.Pd.jumpForce);
        }

    }
    void liquidControl(){
        //initiate
        if(!isFalling){
            _rb.gravityScale = 0;
            _rb.velocity = Vector2.zero;
        }

        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        changeDirection();
        Vector2 wallHitPos = checkWall();
        Vector2 cornerHitPos = checkCorner();
        checkAttached();
        // wall rotate
        if(wallHitPos != Vector2.zero && !isRotating && !isFalling){
            isRotating = true;
            Debug.Log("#1");
            // Debug.Log(wallHitPos);
            // Debug.Log(transform.right);
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
                isWallHit = false;
                _col.enabled = true;
                isHorizontal = !isHorizontal;
            });
        }
        // corner rotate
        if(cornerHitPos != Vector2.zero & !isRotating && !isFalling){
            isRotating = true;
            Vector2 endCenter = (Vector2)transform.right * (localDirection) * distanceToSurface + cornerHitPos;
            
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
                isCornerMet = false;
                _col.enabled = true;
                isHorizontal = !isHorizontal;
            });
        }

        // movement
        if(!isRotating){
            if(isHorizontal && (feet.position.y < center.position.y)){
                //transform.Translate(inputX * 0.05f, 0, 0);
                _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, _rb.velocity.y);
            }
            else if(isHorizontal && (feet.position.y > center.position.y)){
                //transform.Translate(-inputX * 0.05f, 0, 0);
                _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, _rb.velocity.y);
            }
            else if(!isHorizontal && (feet.position.x > center.position.x)){
                //transform.Translate(inputY * 0.05f, 0, 0);
                _rb.velocity = new Vector2(0, inputY * PlayerData.Pd.speed);
            }
            else if (!isHorizontal && (feet.position.x < center.position.x)){
                //transform.Translate(-inputY * 0.05f, 0, 0);
                _rb.velocity = new Vector2(0, inputY * PlayerData.Pd.speed);
            }
        }

    }


    void gasControl(){
        // initialize
        transform.rotation = Quaternion.identity;

        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        // change direction
        changeDirection();
        // set speed
        _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, _rb.velocity.y);
        if(checkCeiling() && Input.GetKeyDown(KeyCode.Space)){
            _rb.velocity += new Vector2(0, -PlayerData.Pd.jumpForce);
        }
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

    bool checkAttached(){
        isAttached = Physics2D.Raycast(feet.position, -transform.up, 0.05f, groundLayer);
        return isAttached;
    }

    bool checkCeiling(){
        isCeiling = Physics2D.Raycast(head.position, transform.up, 0.05f, groundLayer);
        return isCeiling;
    }

    Vector2 checkWall(){
        if(isRotating) return Vector2.zero;
        //int combinedMask = wallLayer | ceilingLayer | groundLayer;
        
        RaycastHit2D hit = Physics2D.Raycast(right.position, transform.right * localDirection, 0.2f, groundLayer);
        isWallHit = (hit.collider == null) ? false:true;
        //Debug.DrawRay(transform.position, transform.right * Mathf.Infinity, Color.red);
        return (isWallHit)? hit.point : Vector2.zero;
    }

    Vector2 checkCorner(){
        if(isCornerMet || isRotating || _rb.gravityScale != 0) return Vector2.zero;
        //int combinedMask = wallLayer | ceilingLayer | groundLayer;
        RaycastHit2D hit = new RaycastHit2D();
        if(!checkAttached()){
            hit = Physics2D.Raycast(
                feet.position + transform.right * localDirection * 0.32f, 
                (-transform.right * localDirection - transform.up).normalized, 
                0.6f, 
                groundLayer
            );
            
            //Debug.Log($"corner: {((hit.collider == null) ? false : true)}");
            isCornerMet = (hit.collider == null) ? false : true;
        }
        return (isCornerMet) ? hit.point : Vector2.zero; 
    }

/*
    public void liquidDrop(){
        if(PlayerData.Pd.state == State.Liquid){
            isRotating = true;
            Vector2 dropPos = Vector2.zero;
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);
            dropPos = hit.point;
            dropPos.y += distanceToSurface;
            //Debug.Log($"drop pos{dropPos}");

            transform
            .DOLocalRotate(
                Vector3.zero,
                0.3f
            );
            //.SetRelative();
            
            transform
            .DOMove(
                dropPos,
                1f
            )
            .SetEase(Ease.InQuad)
            .OnUpdate(()=>{
                inputX = Input.GetAxisRaw("Horizontal");
                transform.Translate(inputX * 0.05f, 0, 0);
            })
            .OnComplete(()=>{
                isRotating = false;
            });
            
        }
    }
*/
    public IEnumerator liquidDrop2(){
        if(!isFalling){
            isFalling = true;
            transform.rotation = Quaternion.identity;
            isHorizontal = true;
            _rb.gravityScale = PlayerData.Pd.gravityScale;
            yield return new WaitUntil(()=>(checkAttached()));
            _rb.gravityScale = 0f;
            isFalling = false;
            
        }
    }

}
