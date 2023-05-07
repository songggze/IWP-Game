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

    CharacterController controller;

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
        input.PlayerMovement.Roll.canceled += ctx => rollPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

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
    }

    private void HandleMovement()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isAttack = animator.GetBool(isAttackHash);
        bool isDodging = animator.GetBool(isDodgingHash);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Standard Idle") && isDodging){
            Debug.Log("Returning to idle state (From roll)");
            animator.SetBool(isDodgingHash, false);
        }
        
        // update position when rolling
        if (rollActiveTimer > 0){
            HandleRolling();
            rollActiveTimer -= Time.deltaTime;
        }
         
        // disable movement when attacking
        if (isAttack || isDodging){
            Debug.Log("Cancelling");
            // disable animations 
            return;
        }

        // bool for Walking
        if (movementPressed && !isWalking){
            animator.SetBool(isWalkingHash, true);
        }
        if (!movementPressed && isWalking){
            animator.SetBool(isWalkingHash, false);
        }

        // bool for Running
        if ((movementPressed && runPressed) && !isRunning){
            animator.SetBool(isRunningHash, true);
        }
        if ((!movementPressed || !runPressed) && isRunning){
            animator.SetBool(isRunningHash, false);
        }

        // bool for rolling
        if (rollPressed && !isDodging) {
            animator.SetTrigger("isRolling");
            animator.SetBool(isDodgingHash, true);
            rollPressed = false;
            rollActiveTimer = setRollTimer;
        }
        
        if (!movementPressed)
            return;
        
        // Moves character based on camera space
        Vector3 direction = Move();
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
     
    private Vector3 Move()
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

    private void OnEnable()
    {
        input.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        input.PlayerMovement.Disable();
    }
}
