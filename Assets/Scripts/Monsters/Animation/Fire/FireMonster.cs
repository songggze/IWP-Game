using UnityEngine;
using UnityEngine.AI;

public class FireMonster : MonoBehaviour
{

    // Stats
    [SerializeField] public int health = 100;
    [SerializeField] float walkSpeed = 2;

    // Enrage Stats
        public bool isEnraged = false;
        public double enrageTimer = 0;
        public float enrageThreshold = 0;

        // constants
        public double set_enrageTimer = 10;
        public float set_enrageThreshold = 100;

        // Speed/attack modifers when enraged
        public float enrageSpeedModifier = 1.25f;
        public float enrageAttackModifier = 1.4f;
    
    // Stagger Stats
        public float set_staggerCounter = 300;
        public float staggerCounter = 300;

        public float set_flyStaggerCounter = 150;
        public float flyStaggerCounter = 150;

    // Trapped Stats
        public float trappedTimer = 0;
        public float set_trappedTimer = 5;

    [SerializeField] GameObject model;
    GameObject player;
    Vector3 direction;
    NavMeshAgent navMeshAgent;
    CharacterController controller;

    public Animator animator;
    int isDeadHash, isAttackingHash, isTrappedHash, isWalkingHash, isFlyingHash;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player Character");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.speed = 0.75f;

        enrageThreshold = set_enrageThreshold;
        isDeadHash = Animator.StringToHash("isDead");
        isAttackingHash = Animator.StringToHash("isAttack");
        isTrappedHash = Animator.StringToHash("isTrapped");
        isWalkingHash = Animator.StringToHash("isWalking");
        isFlyingHash = Animator.StringToHash("isFlying");

        staggerCounter = set_staggerCounter;
        flyStaggerCounter = set_flyStaggerCounter;

        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // Set new position to track
        navMeshAgent.SetDestination(player.transform.position);

        // Check for enraged status
        HandleEnragedStatus();

        // Handle stagger
        if (animator.GetBool("isFlying")){
            HandleFlyingStagger();
        }
        else{
            HandleStagger();
        }

        bool isTrapped = animator.GetBool("isTrapped");
        if (isTrapped){
            
            if (trappedTimer >= 0){
                trappedTimer -= Time.deltaTime;
            }
            else{
                animator.SetBool(isTrappedHash, false);
                // Reset to normal value to enable movement again
                navMeshAgent.stoppingDistance = GetComponent<GroundedMonsterAI>().setStoppingDistance;
            }
        }
    }

    public float GetHitzoneModifier(string part)
    {
        switch (part)
        {
            case "Head":
                return 2;

            case "Arm":
                return 1.4f;

            case "Limb":
                return 1f;

            case "Body":
                return 0.7f;

            case "Tail":
                return 1;
            
            default:
                Debug.Log("Body Part does not exist!!");
                return 0;
        }
    }

    void HandleEnragedStatus()
    {
        bool isDead = animator.GetBool(isDeadHash);

        // Enrage when player does enough damage
        if (!isEnraged && enrageThreshold <= 0 && !isDead){

            animator.SetBool(isAttackingHash, false);

            isEnraged = true;
            enrageTimer = set_enrageTimer;
            foreach (var trigger in animator.parameters)
            {
                if (trigger.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(trigger.name);
                }
            }
            animator.Play("Roar");
            animator.speed = enrageSpeedModifier;
        }

        // Enrage timer ticking
        if (isEnraged & enrageTimer > 0){

            enrageTimer -= Time.deltaTime;

            // Disable enrage status
            if (enrageTimer <= 0){
                isEnraged = false;
                enrageThreshold = set_enrageThreshold;
                animator.speed = 0.75f;
            }
            // model.GetComponent<SkinnedMeshRenderer>().material.color = Color.Lerp(model.GetComponent<SkinnedMeshRenderer>().material.color, Color.red, 2 * Time.deltaTime);
        }
        // else{

        //     model.GetComponent<SkinnedMeshRenderer>().material.color = Color.Lerp(model.GetComponent<SkinnedMeshRenderer>().material.color, Color.white, 2 * Time.deltaTime);
        // }
    }

    void HandleStagger()
    {
        // Play staggered animation if staggerCounter reaches 0
        if (staggerCounter <= 0){

            animator.SetBool(isAttackingHash, false);
            animator.Play("Stagger");

            staggerCounter = set_staggerCounter;            
        }
    }

    void HandleFlyingStagger()
    {
        // Play staggered animation if staggerCounter reaches 0
        if (flyStaggerCounter <= 0){

            Debug.Log("A TOPPLE ANIMATION IS SUPPOSED TO PLAY HERE");
            animator.SetBool(isAttackingHash, false);
            animator.SetBool(isFlyingHash, false);

            flyStaggerCounter = set_flyStaggerCounter;            
            animator.Play("Fall");
        }
    }

    public void HandleTrapped()
    {

        animator.SetBool(isAttackingHash, false);
        animator.SetBool(isWalkingHash, false);
        animator.SetBool(isTrappedHash, true);
        animator.Play("Trapped");
        navMeshAgent.stoppingDistance = 999;

        trappedTimer = set_trappedTimer;
    }
}
