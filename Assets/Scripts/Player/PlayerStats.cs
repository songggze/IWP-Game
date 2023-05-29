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

            animator.SetTrigger("Hurt");
            animator.SetBool(isAttackingHash, false);
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, false);
            animator.SetBool(isDodgingHash, false);
            playHurtAnimation = true;
        }
    }
}
