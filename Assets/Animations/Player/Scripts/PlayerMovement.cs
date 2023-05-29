using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;

    [SerializeField] float walkSpeed = 2.0f;
    [SerializeField] float runSpeed = 4.0f;
    [SerializeField] float rollSpeed = 7.0f;
    [SerializeField] float setRollTimer = 0.7f;

    int isWalkingHash;
    int isRunningHash;
    int isAttackHash;
    int isDodgingHash;

    // Using Unity's input system
    PlayerInput input;

    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;
    bool rollPressed;

    // Timer for translating character
    float rollActiveTimer;

    float timerDelay;
    CharacterController controller;
    Vector3 gravity;

    // Player Stats
    private PlayerStats playerStats;

    // Called when script instance is called
    void Awake()
    {
        input = new PlayerInput();

        // Get vector2 value and bool on movement key press
        input.PlayerMovement.Move.performed += ctx => {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.PlayerMovement.Move.canceled += ctx => {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        
        // 'Toggleable' inputs
        input.PlayerMovement.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.PlayerMovement.Run.canceled += ctx => runPressed = ctx.ReadValueAsButton();
        input.PlayerMovement.Roll.performed += ctx => rollPressed = ctx.ReadValueAsButton();
        input.PlayerMovement.Roll.canceled -= ctx => rollPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        timerDelay = 0;

        playerStats = GetComponent<PlayerStats>();

        gravity = Vector3.zero;

        rollActiveTimer = 0;

        // For input system
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isAttackHash = Animator.StringToHash("isAttack");
        isDodgingHash = Animator.StringToHash("isDodging");
    }

    void Update()
    {
        HandleMovement();

        if (timerDelay > 0){
            timerDelay -= Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isAttack = animator.GetBool(isAttackHash);
        bool isDodging = animator.GetBool(isDodgingHash);

        // Player is damaged
        if (playerStats.isHit){

            return;
        }

        // Stamina Management
        if (isRunning || isDodging){
            // Stamina consumption when running
            if (isRunning && !isAttack && !isDodging && playerStats.stamina > 0){
                playerStats.stamina -= 18 * Time.deltaTime;
            }
        }
        else {
            // Recover stamina when not running or rolling
            if (playerStats.stamina < playerStats.maxStamina)
            {
                RecoverStamina();
            }
        }

        Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        // when rolling animations ends
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Standard Idle") && isDodging && timerDelay <= 0){
            Debug.Log("Returning to idle state (From roll)");
            animator.SetBool(isDodgingHash, false);

            // Reset triggers in case it gets buffered during animation
            input.FindAction("Roll").Enable();
        }
        
        // update position when rolling
        if (rollActiveTimer > 0){
            HandleRolling();
            rollActiveTimer -= Time.deltaTime;
        }

        // gravity when player is not grounded
        HandleGravity();
         
        // disable movement when attacking
        if (isAttack || isDodging){
            return;
        }

        // set booleans based on keyboard press
        SetMovementBool(isWalking, isRunning);

        // bool for rolling
        if (rollPressed && !isDodging && playerStats.stamina > 20) {
            animator.SetTrigger("Roll");
            animator.SetBool(isDodgingHash, true);
            rollPressed = false;
            rollActiveTimer = setRollTimer;

            input.FindAction("Roll").Disable();
            timerDelay = 0.4f;

            playerStats.stamina -= 20;
        }
        
        if (!movementPressed)
            return;
        
        // Moves character based on camera space
        Vector3 direction = GetDirection();
        // Rotates character based on camera space
        HandleRotation(direction);

        // Update position
        if (animator.GetBool(isRunningHash)){
            controller.Move(direction * Time.deltaTime * runSpeed);
        }
        else if (animator.GetBool(isWalkingHash)){
            controller.Move(direction * Time.deltaTime * walkSpeed);
        }
    }
     
    private Vector3 GetDirection()
    {
        // Getting direction input from keyboard
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Moves according to camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardProduct = move.z * cameraForward;
        Vector3 cameraRightProduct = move.x * cameraRight;

        Vector3 direction = cameraForwardProduct + cameraRightProduct;
        return direction;
    }

    void HandleGravity()
    {
        if (!controller.isGrounded){
            gravity.y += -10 * Time.deltaTime;
            controller.Move(gravity * Time.deltaTime);
        }
        else{
            gravity.y = 0;
        }
    }

    private void HandleRolling()
    {
        controller.Move(transform.forward * Time.deltaTime * rollSpeed);
    }

    private void HandleRotation(Vector3 direction)
    {
        Quaternion currentRotation = transform.rotation;

        Vector3 targetPosition;
        targetPosition.x = direction.x;
        targetPosition.y = 0;
        targetPosition.z = direction.z;

        Quaternion targetRotation = Quaternion.LookRotation(targetPosition);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, 10 * Time.deltaTime);

    }

    private void SetMovementBool(bool isWalking, bool isRunning)
    {

        // bool for Walking
        if (movementPressed && !isWalking){
            animator.SetBool(isWalkingHash, true);
        }
        if (!movementPressed && isWalking){
            animator.SetBool(isWalkingHash, false);
        }

        // bool for Running
        if ((movementPressed && runPressed) && !isRunning && playerStats.stamina > 0){

            animator.SetBool(isRunningHash, true);
        }
        if ((!movementPressed || !runPressed) && isRunning  || playerStats.stamina <= 0){
            animator.SetBool(isRunningHash, false);
        }
    }

    private void RecoverStamina()
    {
        playerStats.stamina += 20 * Time.deltaTime;
    }

    private void OnEnable()
    {
        input.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        input.PlayerMovement.Disable();
    }
}
