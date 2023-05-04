using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    Animator animator;

    int isAttackHash;

    PlayerInput input;

    bool leftAttackPressed;

    CharacterController controller;
     
    string currentAnimationName;

    // Called when script instance is called
    void Awake()
    {
        input = new PlayerInput();

        input.PlayerAttack.LeftAttack.performed += ctx => leftAttackPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        // For input system
        isAttackHash = Animator.StringToHash("isAttack");
    }

    // Update is called once per frame
    void Update()
    {
        currentAnimationName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        HandleAttack();
    }
     
    void HandleAttack()
    {
        bool isAttack = animator.GetBool(isAttackHash);
        // FIX: attacking animation sometimes allows movement

        // allows movement when entering idle state (what it defaults to from any attack state)
        if (currentAnimationName == "Standard Idle" && isAttack){
            Debug.Log("Attack ended.");
            animator.SetBool(isAttackHash, false);
        }

        if (leftAttackPressed && !isAttack) {
            animator.SetTrigger("leftAttack");
            animator.SetBool(isAttackHash, true);
            leftAttackPressed = false;
        }
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
