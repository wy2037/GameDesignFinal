using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    // for testing
    public bool stateFlag = true;
    // children
    private Transform head;
    private Transform feet;
    private Transform right;
    private Transform center;
    private Transform front;
    private Transform back;

    // bool
    [Header("status")]
    [SerializeField] public bool isAttached;
    [SerializeField] private bool isCeiling;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isWallHit;
    [SerializeField] private bool isCornerMet;
    [SerializeField] private bool isRotating;
    [SerializeField] private bool isHorizontal;

    [SerializeField] private State previousState;

    // data
    [Header("PlayerData")]
    [SerializeField] private float distanceToSurface;
    [SerializeField] private float inputX;
    [SerializeField] private float inputY;
    [SerializeField] private int localDirection; // left or right
    [SerializeField] private float maxFallVelocity;
    // temperature changing points
    [SerializeField] private int solidToLiquid;
    [SerializeField] private int liquidToGas;

    // float
    public float rotateDuration = 0.2f;
    public float afkCooldown;
    [SerializeField] private float curAfkCooldown;

    // sprite
    [Header("Sprite")]
    public Sprite solidSprite;
    public Sprite liquidSprite; 
    public Sprite gasSprite;

    // layer
    [Header("LayerMask")]
    public LayerMask groundLayer;

    // component
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private BoxCollider2D _col;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Animator _ani;


    
    private void Awake() {
        // get children
        head = transform.Find("head");
        center = transform.Find("center");
        feet = transform.Find("feet");
        right = transform.Find("right");
        front = transform.Find("front");
        back = transform.Find("back");

        distanceToSurface = Vector2.Distance(transform.position, feet.position);

        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _ani = GetComponent<Animator>();

        // bool
        _rb.isKinematic = false;
        isHorizontal = true; 
        stateFlag = false;


        
    }

    private void Start() {
        _ani.SetTrigger("StartScene");
        if(PlayerData.Pd.temperature <= solidToLiquid){
            solidInit();
        }
        else if (PlayerData.Pd.temperature > solidToLiquid && PlayerData.Pd.temperature <= liquidToGas){
            liquidInit();
        }
        else if (PlayerData.Pd.temperature > liquidToGas){
            gasInit();
        }
        curAfkCooldown = afkCooldown;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.F)){
            stateFlag = !stateFlag;
        }

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
            if(PlayerData.Pd.temperature <= solidToLiquid && PlayerData.Pd.state != State.Solid){
                solidInit();
            }
            else if (PlayerData.Pd.temperature > solidToLiquid && PlayerData.Pd.temperature <= liquidToGas && PlayerData.Pd.state != State.Liquid){
                liquidInit();
            }
            else if (PlayerData.Pd.temperature > liquidToGas && PlayerData.Pd.state != State.Gas){
                gasInit();
            }
            else if(Input.GetKeyDown(KeyCode.V)){
                StartCoroutine(liquidDrop2());
            }
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

        // afk
        curAfkCooldown -= Time.deltaTime;
        if( inputX != 0 || inputY != 0 ){
            //_ani.SetTrigger("AFK");
            curAfkCooldown = afkCooldown;
        }
        if(curAfkCooldown < 0){
            _ani.SetTrigger("AFK");
            curAfkCooldown = afkCooldown;
        }

        // debug
        checkAttached();
        checkCeiling();
        checkCorner();
        Debug.DrawRay(front.position, (-transform.right * localDirection - transform.up).normalized * 0.5f,
            Color.green
            );
        Debug.DrawRay(back.position, (transform.right * localDirection - transform.up).normalized * 0.5f,
            Color.blue
            );

        Debug.DrawRay(front.position, -transform.up * 0.2f, Color.cyan);
        Debug.DrawRay(back.position, -transform.up * 0.2f, Color.cyan);
        Debug.DrawRay(feet.position, -transform.up * 0.05f, Color.cyan);
    }


    void solidInit(){
        PlayerData.Pd.state = State.Solid;
        this.gameObject.layer = LayerMask.NameToLayer("PlayerS");
        // animation
        _ani.SetBool("Solid", true);
        _ani.SetBool("Gas", false);
        _ani.SetBool("Liquid", false);
        switch (previousState){
            case State.Liquid:
                _ani.SetTrigger("LiquidToSolid");
                break;
        }
        previousState = State.Solid;



        _col.offset = new Vector2(0,-0.15f);
        _col.size = new Vector2(0.64f,0.66f);
        
        _rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.identity;
        isHorizontal = true;
        _rb.gravityScale = PlayerData.Pd.gravityScale;

    }
    void liquidInit(){
        PlayerData.Pd.state = State.Liquid;
        this.gameObject.layer = LayerMask.NameToLayer("PlayerL");
        // animation
        _ani.SetBool("Liquid", true);
        _ani.SetBool("Solid", false);
        _ani.SetBool("Gas", false);
        switch (previousState){
            case State.Solid:
                _ani.SetTrigger("SolidToLiquid");
                break;
            case State.Gas:
                _ani.SetTrigger("GasToLiquid");
                break;
        }
        previousState = State.Liquid;

        
        _col.offset = new Vector2(0,-0.27f);
        _col.size = new Vector2(0.64f,0.41f);

        _rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(liquidDrop2());
    }
    void gasInit(){
        PlayerData.Pd.state = State.Gas;
        this.gameObject.layer = LayerMask.NameToLayer("PlayerG");
        // animation
        _ani.SetBool("Gas", true);
        _ani.SetBool("Liquid", false);
        _ani.SetBool("Solid", false);
        switch (previousState){
            case State.Liquid:
                _ani.SetTrigger("LiquidToGas");
                break;

        }
        previousState = State.Gas;

        _col.offset = new Vector2(0,-0.06f);
        _col.size = new Vector2(0.6f, 0.46f);
        
        _rb.isKinematic = false;
        _rb.gravityScale = -PlayerData.Pd.gravityScale;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isHorizontal = true;

    }
    void solidControl(){
        // initiate
        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        _ani.SetFloat("Speed", Mathf.Abs(inputX));
        // change direction
        changeDirection();
        // change drop rate
        inputY = _rb.velocity.y;
        if(inputY < 0){
            inputY *= 1.015f;
        }
        // set speed
        _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, Mathf.Max(inputY, -maxFallVelocity));
        if(checkAttached() && Input.GetKeyDown(KeyCode.Space)){
            _ani.SetBool("Grounded", isAttached);
            _ani.SetTrigger("Jump");
            //Debug.Log("solid is grounded and gonna jump");
            _rb.velocity += new Vector2(0, PlayerData.Pd.jumpForce);
        }



        _ani.SetBool("Grounded", isAttached);

    }
    void liquidControl(){
        //initiate
        if(isRotating) return;
        // if(Input.GetKeyDown(KeyCode.Space)) StartCoroutine(liquidDrop2());
        if(!isFalling){
            _rb.gravityScale = 0;
            _rb.velocity = Vector2.zero;
        }

        // get input
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        _ani.SetFloat("Speed", Mathf.Abs(inputX) + Mathf.Abs(inputY));


        changeDirection();
        Vector2 wallHitPos = checkWall();
        // Vector2 cornerHitPos = checkCorner();
        bool isFront = true;
        Vector2 cornerHitPos = checkCorner2(ref isFront);
        checkAttached();
        _ani.SetBool("Grounded", isAttached);
        // wall rotate
        if(wallHitPos != Vector2.zero && !isRotating){
            isRotating = true;
            isHorizontal = !isHorizontal;

            Vector2 endCenter = ((Vector2)center.position - wallHitPos).normalized * distanceToSurface + wallHitPos;

            // transform.position = endCenter;
            // transform.eulerAngles += new Vector3(0, 0, 90 * localDirection);
            // isRotating = false;
            // isWallHit = false;
            transform
            .DOMove(
                endCenter,
                rotateDuration
            );
            transform
            .DOLocalRotate(
                new Vector3(0, 0, 90 * localDirection),
                rotateDuration
            )
            .SetRelative()
            .OnComplete(()=>{
                isRotating = false;
                isWallHit = false;
                if(PlayerData.Pd.state == State.Solid || PlayerData.Pd.state == State.Gas) transform.rotation = Quaternion.identity;
            });
        }
        // corner rotate
        if(cornerHitPos != Vector2.zero & !isRotating && !isFalling){
            isRotating = true;
            isHorizontal = !isHorizontal;
            Vector2 endCenter =  ((isFront)? 1:-1) * (Vector2)transform.right * (localDirection) * distanceToSurface * 1.01f  + cornerHitPos;
            Debug.Log(endCenter);
            

            checkAttached();
            Sequence sq = DOTween.Sequence();
            sq
            .SetId("corner rotate")
            .OnStart(()=>{
                transform
                .DOMove(
                    endCenter,
                    rotateDuration
                );
                transform
                .DOLocalRotate(
                    new Vector3(0, 0, ((isFront)? 1 : -1) * -90 * localDirection),
                    rotateDuration
                )
                .SetRelative()
                .OnComplete(()=>{
                    checkAttached();
                    isRotating = false;
                    isCornerMet = false;
                    if(PlayerData.Pd.state == State.Solid || PlayerData.Pd.state == State.Gas) transform.rotation = Quaternion.identity;
                });
            })
            .AppendInterval(rotateDuration + 0.1f)
            .OnComplete(()=>{
                
            });
        }

        // if(!isFalling && !checkAttached()){
        //     StartCoroutine(liquidDrop2());
        // }

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
        _ani.SetFloat("Speed", Mathf.Abs(inputX));
        // change direction
        changeDirection();
        // set speed
        _rb.velocity = new Vector2(inputX * PlayerData.Pd.speed, Mathf.Min(_rb.velocity.y, maxFallVelocity));
        if(checkCeiling() && Input.GetKeyDown(KeyCode.Space)){
            _ani.SetBool("Grounded", isCeiling);
            _ani.SetTrigger("Jump");
            _rb.velocity += new Vector2(0, -PlayerData.Pd.jumpForce);
        }
        _ani.SetBool("Grounded", isCeiling);
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
        
        RaycastHit2D hit = Physics2D.Raycast(right.position, transform.right * localDirection, 0.2f, groundLayer);
        isWallHit = (hit.collider == null) ? false:true;
        return (isWallHit)? hit.point : Vector2.zero;
    }

    Vector2 checkCorner(){
        if(isCornerMet || isRotating || _rb.gravityScale != 0) return Vector2.zero;
        RaycastHit2D hit = new RaycastHit2D();
        if(!checkAttached()){
            hit = Physics2D.Raycast(
                feet.position + transform.right * localDirection * 0.18f, 
                (-transform.right * localDirection - transform.up).normalized, 
                0.35f, 
                groundLayer
            );
            
            isCornerMet = (hit.collider == null) ? false : true;
        }
        return (isCornerMet) ? hit.point : Vector2.zero; 
    }


    Vector2 checkCorner2(ref bool isFront){
        if(isRotating || _rb.gravityScale != 0) return Vector2.zero;
        RaycastHit2D hit = new RaycastHit2D();
        RaycastHit2D frontHit = new RaycastHit2D();
        RaycastHit2D backHit = new RaycastHit2D();
        if(!checkAttached()){
            frontHit = Physics2D.Raycast(front.position, -transform.up, 0.2f, groundLayer);
            backHit = Physics2D.Raycast(back.position, -transform.up, 0.2f, groundLayer);
            Debug.Log($"{frontHit.point}   {backHit.point}");
            if(!frontHit && backHit) isFront = true;
            if(frontHit && !backHit) isFront = false;
            Debug.Log(isFront);
            // if(!frontHit && !backHit){
            //     StartCoroutine(liquidDrop2());
            //     return Vector2.zero;
            // }
            if(isFront) hit = Physics2D.Raycast(front.position, (-transform.right * localDirection - transform.up).normalized, 0.5f, groundLayer);
            Debug.Log(hit.point);
            if(!isFront) hit = Physics2D.Raycast(back.position, (transform.right * localDirection - transform.up).normalized, 0.5f, groundLayer);
            Debug.Log(hit.point);
            // hit = (frontHit)? Physics2D.Raycast(
            //         front.position, 
            //         (-transform.right * localDirection - transform.up).normalized, 
            //         0.5f, 
            //         groundLayer
            //     ):Physics2D.Raycast(
            //         back.position,
            //         (transform.right * localDirection - transform.up).normalized, 
            //         0.5f, 
            //         groundLayer
            //     );
            
            isCornerMet = ((frontHit && !backHit) || (!frontHit && backHit)) ? true : false;
            Debug.Log($"corner: {hit.point}");
        }
        return (isCornerMet) ? hit.point : Vector2.zero; 
    }
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


    private void OnDestroy() {
        DOTween.KillAll();
    }

}
