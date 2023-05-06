using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;

    int isAttackHash;
    int isDodgingHash;

    PlayerInput input;

    bool leftAttackPressed;
    bool rightAttackPressed;

    float timerDelay;
    [SerializeField] float leftClickDelay = 0.3f;
    float leftClickTimer;

    //CharacterController controller;
     
    string currentAnimationName;

    // Called when script instance is called
    void Awake()
    {
        input = new PlayerInput();

        input.PlayerAttack.LeftAttack.performed += ctx => leftAttackPressed = ctx.ReadValueAsButton();
        input.PlayerAttack.LeftAttack.canceled -= ctx => leftAttackPressed = ctx.ReadValueAsButton();
        //input.PlayerAttack.RightAttack.performed += ctx => leftAttackPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        //controller = GetComponent<CharacterController>();

        // For input system
        isAttackHash = Animator.StringToHash("isAttack");
         
        // To prevent going to idle animation when starting an attack
        timerDelay = 0;
        
        // Timer delay till to start an attack again
        leftClickTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
    }
     
    void HandleAttack()
    {
        bool isAttack = animator.GetBool(isAttackHash);


        //-------------------------------//
        //      Left-click attacks 
        //-------------------------------//

        // Starting attack
        if (leftAttackPressed && !isAttack && leftClickTimer <= 0) {
            animator.SetTrigger("Slash 1");
            animator.SetBool(isAttackHash, true);
            leftAttackPressed = false;
            timerDelay = 0.3f;
        }

        // Combo 1 
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash 1") && isAttack && leftAttackPressed){
            animator.SetTrigger("Slash 2");
            leftAttackPressed = false;
            timerDelay = 0.15f;
        }

        // Finisher
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Slash 2") && isAttack && leftAttackPressed){
            animator.SetTrigger("Slash 3");
            leftAttackPressed = false;
        }
        
        // allows movement when entering idle state (what it defaults to from any attack state)
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Standard Idle") && isAttack && timerDelay <= 0){
            Debug.Log("Returning to idle state");
            animator.SetBool(isAttackHash, false);
            leftClickTimer = leftClickDelay;
            
            // Reset triggers in case it gets buffered during animation
            animator.ResetTrigger("Slash 1");
            animator.ResetTrigger("Slash 2");
            animator.ResetTrigger("Slash 3");
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

        //if (rightAttackPressed && !isAttack) {
            //animator.SetTrigger("rightAttack");
            //animator.SetBool(isAttackHash, true);
            //rightAttackPressed = false;
        //}
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
