using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    [SerializeField] public bool isRotating;
    [SerializeField] public bool isHorizontal;
    [SerializeField] private bool isEnabled;

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
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float curCoyoteTime;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float curJumpBufferTime;


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
        isEnabled = true;
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
        if(Input.GetKeyDown(KeyCode.Alpha0)) SceneManager.LoadScene(0);
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
            // else if(Input.GetKeyDown(KeyCode.V)){
            //     StartCoroutine(liquidDrop2());
            // }
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
            // else if(Input.GetKeyDown(KeyCode.V)){
            //     StartCoroutine(liquidDrop2());
            // }
        }
        if(isEnabled){
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
            //// counter
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

            // coyoteTime

            switch(PlayerData.Pd.state){
                case State.Solid:
                    if(checkGrounded()){
                        curCoyoteTime = coyoteTime;
                    }else{
                        curCoyoteTime -= Time.deltaTime;
                    }
                    break;
                case State.Gas:
                    if(checkCeiling()){
                        curCoyoteTime = coyoteTime;
                    }else{
                        curCoyoteTime -= Time.deltaTime;
                    }
                    break;
            }

            // jumpbuffer
            if(Input.GetKeyDown(KeyCode.Space)){
                curJumpBufferTime = jumpBufferTime;
            }else{
                curJumpBufferTime -= Time.deltaTime;
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
    }


    public State temperatureToState(float temp){
        if(PlayerData.Pd.temperature <= solidToLiquid){
            return State.Solid;
        }
        else if (PlayerData.Pd.temperature > solidToLiquid && PlayerData.Pd.temperature <= liquidToGas){
            return State.Liquid;
        }
        else if (PlayerData.Pd.temperature > liquidToGas){
            return State.Gas;
        }
        return State.Solid;
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
            case State.Solid:
                _ani.SetTrigger("StartScene");
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

        // audio
        ManagerAudio.Instance.PlaySFX("solid");

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
            case State.Liquid:
                _ani.SetTrigger("StartScene");
                break;
        }

        
        _col.offset = new Vector2(0,-0.27f);
        _col.size = new Vector2(0.64f,0.41f);

        _rb.isKinematic = false;

        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if(previousState == State.Gas) StartCoroutine(liquidRaise());
        else if(previousState == State.Solid) StartCoroutine(liquidDrop2());
        
        previousState = State.Liquid;

        // audio
        ManagerAudio.Instance.PlaySFX("liquid");
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
            case State.Gas:
                _ani.SetTrigger("StartScene");
                break;

        }
        previousState = State.Gas;

        _col.offset = new Vector2(0,-0.06f);
        _col.size = new Vector2(0.6f, 0.46f);
        
        _rb.isKinematic = false;
        _rb.gravityScale = -PlayerData.Pd.gravityScale;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isHorizontal = true;

        // audio
        ManagerAudio.Instance.PlaySFX("gas");

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
        if(curCoyoteTime > 0f && curJumpBufferTime > 0f){
            _ani.SetBool("Grounded", isAttached);
            _ani.SetTrigger("Jump");
            //Debug.Log("solid is grounded and gonna jump");
            _rb.velocity += new Vector2(0, PlayerData.Pd.jumpForce);
            ManagerAudio.Instance.PlaySFX("jump");
            curJumpBufferTime = 0f;
        }
        if(Input.GetKeyUp(KeyCode.Space) && _rb.velocity.y > 0f){
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            curCoyoteTime = 0f;
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
        if(curCoyoteTime > 0f && curJumpBufferTime > 0f){
            _ani.SetBool("Grounded", isCeiling);
            _ani.SetTrigger("Jump");
            _rb.velocity += new Vector2(0, -PlayerData.Pd.jumpForce);
            curJumpBufferTime = 0f;
        }
        if(Input.GetKeyUp(KeyCode.Space) && _rb.velocity.y < 0f){
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.5f);
            ManagerAudio.Instance.PlaySFX("jump");
            curCoyoteTime = 0f;
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

    // check grounded using two raycast, the width of the box should based on the collider width
    bool checkGrounded(){
        // get the width of the collider
        float width = GetComponent<BoxCollider2D>().bounds.extents.x;
        // get the position of the left and right edge of the collider
        Vector2 leftPos = new Vector2(feet.position.x - width, feet.position.y);
        Vector2 rightPos = new Vector2(feet.position.x + width, feet.position.y);
        // check if the left or right edge is touching the ground
        bool isGrounded = Physics2D.Raycast(leftPos, -transform.up, 0.05f, groundLayer) || Physics2D.Raycast(rightPos, -transform.up, 0.05f, groundLayer);
        return isGrounded;
    }

    // check ceiling using two raycast, the width of the box should based on the collider width
    bool checkCeiling(){
        // get the width of the collider
        float width = GetComponent<BoxCollider2D>().bounds.extents.x;
        // get the position of the left and right upper corner of the collider
        Vector2 leftPos = new Vector2(head.position.x - width, head.position.y);
        Vector2 rightPos = new Vector2(head.position.x + width, head.position.y);
        // check if the left or right edge is touching the ground
        isCeiling = Physics2D.Raycast(leftPos, transform.up, 0.05f, groundLayer) || Physics2D.Raycast(rightPos, transform.up, 0.05f, groundLayer);
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

    public IEnumerator liquidRaise(){
        if(!isFalling){
            isFalling = true;
            Debug.Log("inverse");
            transform.position -= new Vector3(0, distanceToSurface, 0);
            transform.rotation = Quaternion.Euler(0, 0, 180f);
            isHorizontal = true;
            _rb.gravityScale = -PlayerData.Pd.gravityScale;
            yield return new WaitUntil(()=>(checkAttached()));
            _rb.gravityScale = 0f;
            isFalling = false;
        }
    }

    public IEnumerator die(){
        // audio
        ManagerAudio.Instance.PlaySFX("death");
        //_player.GetComponent<PlayerController>().enabled = false;
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        _col.enabled = false;
        isEnabled = false;

        _sr.color = Color.red;
        _sr
        .DOFade(
            0,
            1f
        );

        transform
        .DOLocalJump(
            Vector3.right * (Random.Range(0, 1) == 1? 1 : -1),
            1,
            1,
            1f
        )
        .SetRelative();

        yield return new WaitForSeconds(1.1f);
        transform.position = PlayerData.Pd.lastCheckedPosition;

        // after moving
        transform.rotation = Quaternion.identity;
        PlayerData.Pd.temperature = PlayerData.Pd.lastCheckedTemperature;
        PlayerData.Pd.state = temperatureToState(PlayerData.Pd.temperature);
        switch (PlayerData.Pd.state){
            case State.Solid:
                solidInit();
                break;
            case State.Liquid:
                liquidInit();
                break;
            case State.Gas:
                gasInit();
                break;
        }
        _ani.SetTrigger("StartScene");
        _sr.color = Color.white;
        _col.enabled = true;
        

        for (int i = 0; i < 3; i++)
        {
            _sr.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.2f);
            _sr.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.2f);
        }
        isEnabled = true;
        _sr.color = new Color(1, 1, 1, 1);
        _rb.isKinematic = false;
        // enable
        _rb.velocity = Vector2.zero;
        if(PlayerData.Pd.state == State.Liquid) StartCoroutine(liquidDrop2());
    }




    private void OnDestroy() {
        DOTween.KillAll();
    }

}
