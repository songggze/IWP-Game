using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] public float maxStamina = 100;

    [SerializeField] float faintsLife = 100;

    public float health;
    public float stamina;
    [SerializeField] float iFrames = 1;
    public float iFrameTimer;

    public bool isHit;
    public bool playHurtAnimation;

    private Animator animator;
    private PlayerAttack playerAttack;

    int isAttackingHash;
    int isWalkingHash;
    int isRunningHash;
    int isDodgingHash;
    void Start()
    {
        health = maxHealth;
        stamina = maxStamina;

        iFrameTimer = 0;
        isHit = false;
        playHurtAnimation = false;
        
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        isAttackingHash = Animator.StringToHash("isAttack");
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isDodgingHash = Animator.StringToHash("isDodging");
    }

    void Update()
    {
        
        if (iFrameTimer > 0){
            iFrameTimer -= Time.deltaTime;
        }
        else{
            isHit = false;
            playHurtAnimation = false;
        }
    }


    public void SetIFrames()
    {
        iFrameTimer = iFrames;
        if (!playHurtAnimation){

            // Set player hurt animation
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
}
