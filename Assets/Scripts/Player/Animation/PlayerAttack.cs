using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;
    PlayerFrameData attackData;
    PlayerStats playerStats;
    [SerializeField] GameObject swordHitbox;

    // Inputs
    int isAttackHash;
    int isDodgingHash;
    int isRunningHash;

    PlayerInput input;

    private bool leftAttackPressed;
    private bool rightAttackPressed;
    public bool finalAttack;

    // Timers
    float timerDelay;
    [SerializeField] float leftClickDelay = 0.1f;
    float leftClickTimer;
    
    float buttonDelay;
    float setButtonDelay = 0.15f;

    // To switch according to current animation
    string currentAnimationName;

    // Called when script instance is called
    void Awake()
    {
        input = new PlayerInput();

        input.PlayerAttack.LeftAttack.performed += ctx => leftAttackPressed = ctx.ReadValueAsButton();
        //input.PlayerAttack.LeftAttack.canceled -= ctx => leftAttackPressed = ctx.ReadValueAsButton();
        input.PlayerAttack.RightAttack.performed += ctx => rightAttackPressed = ctx.ReadValueAsButton();
        //input.PlayerAttack.RightAttack.canceled -= ctx => rightAttackPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        attackData = swordHitbox.GetComponent<PlayerFrameData>();
        playerStats = GetComponent<PlayerStats>();

        // For input system
        isAttackHash = Animator.StringToHash("isAttack");
        isDodgingHash = Animator.StringToHash("isDodging");
         
        // To prevent going to idle animation when starting an attack
        timerDelay = 0;
        
        // Timer delay till to start an attack again
        leftClickTimer = 0;

        finalAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerStats.isDead){
            HandleAttack();
        }
    }
     
    void HandleAttack()
    {
        
        bool isDodging = animator.GetBool(isDodgingHash);
        if (isDodging || playerStats.isHit){
            return;
        }

        // Handle attack clicking
        if (buttonDelay <= 0){
            if (leftAttackPressed && rightAttackPressed){
                Debug.Log("Both");
                buttonDelay += setButtonDelay;
            }
            else if ((leftAttackPressed && !finalAttack) || Input.GetKeyDown(KeyCode.J)){
                LeftAttack();
                buttonDelay += setButtonDelay;
            }
            else if (rightAttackPressed && !finalAttack){

                RightAttack();
                buttonDelay += setButtonDelay;
            }
        }
        else{
            buttonDelay -= Time.deltaTime;
        }

        

        bool isAttack = animator.GetBool(isAttackHash);
        // allows movement when entering idle state (what it defaults to from any attack state)
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Standard Idle") && isAttack && timerDelay <= 0){
            // Debug.Log("Returning to idle state");
            animator.SetBool(isAttackHash, false);
            leftClickTimer = leftClickDelay;
            finalAttack = false;

            // Reset triggers in case it gets buffered during animation
            animator.ResetTrigger("Slash 1");
            animator.ResetTrigger("Slash 2");
            animator.ResetTrigger("Slash 3");
            animator.ResetTrigger("Right 1");
            animator.ResetTrigger("Right 2");
        }


        //-------------------------------//
        //      Timers  
        //-------------------------------//
        if (timerDelay > 0) {
            timerDelay -= Time.deltaTime;
        }

        if (leftClickTimer > 0){
            leftClickTimer -= Time.deltaTime;
        }

    }

    void LeftAttack()
    {
        bool isAttack = animator.GetBool(isAttackHash);

        //-------------------------------//
        //      Left-click attacks 
        //-------------------------------//
        // Starting attack
        if (!isAttack && leftClickTimer <= 0) {
            animator.SetTrigger("Slash 1");
            attackData.SetValues("Slash 1");
            animator.SetBool(isAttackHash, true);
            leftAttackPressed = false;
            timerDelay = 0.3f;
        }

        // Combo 1 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash 1") && NextAttackCondition(isAttack, leftAttackPressed)){
            animator.SetTrigger("Slash 2");
            attackData.SetValues("Slash 2");
            leftAttackPressed = false;
            timerDelay = 0.15f;
        }

        // Finisher
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash 2") && NextAttackCondition(isAttack, leftAttackPressed)){
            animator.SetTrigger("Slash 3");
            attackData.SetValues("Slash 3");
            leftAttackPressed = false;

            // To prevent going in attack function during transition
            finalAttack = true;
        }
    }

    void RightAttack()
    {

        bool isAttack = animator.GetBool(isAttackHash);

        //-------------------------------//
        //      Right-click attacks 
        //-------------------------------//
        // Starting attack
        if (!isAttack && leftClickTimer <= 0) {
            animator.SetTrigger("Right 1");
            attackData.SetValues("Right 1");
            animator.SetBool(isAttackHash, true);
            rightAttackPressed = false;
            timerDelay = 0.3f;
        }

        // Combo 1 
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash 1") && NextAttackCondition(isAttack)){
            //animator.SetTrigger("Slash 2");
            //attackData.SetValues("Slash 2");
            //leftAttackPressed = false;
            //timerDelay = 0.15f;
        //}

        // Finisher
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Right 1") && NextAttackCondition(isAttack, rightAttackPressed)){
            animator.SetTrigger("Right 2");
            attackData.SetValues("Right 2");
            rightAttackPressed = false;

            // To prevent going in attack function during transition
            finalAttack = true;
        }
    }
    
    // Conditions for next attack of the combo
    bool NextAttackCondition(bool isAttack, bool attackButton)
    {
        if (isAttack && attackButton && attackData.delayFrames <= 0){
            return true;
        }
        return false;
    }

    void OnEnable()
    {
        input.PlayerAttack.Enable();
    }

    void OnDisable()
    {
        input.PlayerAttack.Disable();
    }

    //void OnEnable()
    //{
        //Debug.Log("Attack: " + leftAttackPressed);
        //input.PlayerAttack.LeftAttack.performed += HandleAttack;
    //}

    //void OnDisable()
    //{
        //Debug.Log("Attack: " + leftAttackPressed);
        //input.PlayerAttack.LeftAttack.performed -= HandleAttack;
    //}
}
