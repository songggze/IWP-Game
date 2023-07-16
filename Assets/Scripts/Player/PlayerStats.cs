using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Status
    public float health, stamina;
    public bool isTired;
    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float maxStamina = 100;

    ///////////////////////////////////////////////////
    // Equipment Stats, put in seperate script later
    [SerializeField] public float attack = 8;
    ///////////////////////////////////////////////////

    // Get previous pos for collision checking
    public Vector3 prevPosition;

    // Frame Data
    [SerializeField] float iFrames = 1;
    [SerializeField] float set_healTimer = 2;
    [SerializeField] float set_healAnimationTimer = 2.0f;
    public float iFrameTimer, healTimer, healAnimationTimer;
    public bool healEffect = false;

    // Set animation states
    public bool isHit, playHurtAnimation;
    public bool isDead, playDeadAnimation;
    public bool isHealing, playHealingAnimation;

    private Animator animator;
    private PlayerAttack playerAttack;

    // Animator bool checking
    int isAttackingHash;
    int isWalkingHash;
    int isRunningHash;
    int isDodgingHash;

    void Start()
    {
        health = maxHealth;
        stamina = maxStamina;
        isTired = false;

        prevPosition = gameObject.transform.position;

        // Animations and Timers
        iFrameTimer = 0;
        isHit = false;
        playHurtAnimation = false;
        
        healTimer = 0;
        isHealing = false;
        playHealingAnimation = false;

        isDead = false;
        
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        isAttackingHash = Animator.StringToHash("isAttack");
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isDodgingHash = Animator.StringToHash("isDodging");
    }

    void Update()
    {
        
        // Iframes when hurt
        if (iFrameTimer > 0){
            iFrameTimer -= Time.deltaTime;
        }
        else{
            isHit = false;
            playHurtAnimation = false;
        }


        // Healing animatiion
        if (isHealing){
            HealAnimation();
        }

        if (playHealingAnimation){
            if (healAnimationTimer > 0)
            {
                healTimer -= Time.deltaTime;
                healAnimationTimer -= Time.deltaTime;
                
            }
            else
            {
                isHealing = false;
                playHealingAnimation = false;
            }
        }

        // Check if health == 0
        CheckHealthStatus();
    }

    public void HealAnimation()
    {

        if (!playHealingAnimation){
            // Set player hurt animation
            animator.SetTrigger("Healing");
            animator.Play("Standard Idle");

            animator.SetBool(isAttackingHash, false);
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isDodgingHash, false);
            healTimer = set_healTimer;
            healAnimationTimer = set_healAnimationTimer;
            healEffect = false;
            playHealingAnimation = true;
            playerAttack.finalAttack = false;
        }
    }

    public void SetIFrames()
    {
        iFrameTimer = iFrames;
        if (!playHurtAnimation){

            // Set player hurt animation
            playHealingAnimation = false;
            healEffect = false;
            isHealing = false;

            animator.ResetTrigger("Drinking");
            animator.SetTrigger("Hurt");
            animator.Play("Standard Idle");

            animator.SetBool(isAttackingHash, false);
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isDodgingHash, false);
            playHurtAnimation = true;
            playerAttack.finalAttack = false;

        }
    }

    private void CheckHealthStatus()
    {

        if (health <= 0){
            isDead = true;
            if (!playDeadAnimation){
                animator.Play("Player Death");
            }
        }
    }
}
