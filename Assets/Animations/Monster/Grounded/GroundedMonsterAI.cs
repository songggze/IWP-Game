using UnityEngine;
using UnityEngine.AI;

public class GroundedMonsterAI : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;

    [SerializeField] float setStoppingDistance = 10;

    int isWalkingHash;
    int isAttackingHash;
    
    float timerDelay;
    Vector3 velocity;



    //TODO: raycast may not initate attacks?




    // Will only initiate attacks when player is in line of sight
    public bool startAttack;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isAttackingHash = Animator.StringToHash("isAttack");

        navMeshAgent.stoppingDistance = setStoppingDistance;
        velocity = Vector3.zero;

        timerDelay = 0;
        startAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Convert hash into bool
        bool isAttack = animator.GetBool(isAttackingHash);

        if (!isAttack){
            HandleMovement();
        }

        bool isWalking = animator.GetBool(isWalkingHash);
        if (!isWalking && !isAttack && startAttack){
            HandleAttacks();
        }

        // Applying movement to attack animations since root motion is disabled for the controller
        HandleAnimations();

        Debug.Log(isAttack);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !isWalking && timerDelay <= 0){
            animator.SetBool(isAttackingHash, false);

            // Reset to normal value to enable movement again
            navMeshAgent.stoppingDistance = setStoppingDistance;
            velocity = Vector3.zero;

            startAttack = false;

            // Reset triggers in case it gets buffered during animation
            animator.ResetTrigger("Horn Attack");
        }

        if (timerDelay > 0){
            timerDelay -= Time.deltaTime;
        }
    }

    void HandleAttacks()
    {
        // Setting stoppingDistance to 999 as for some reason enabling/disabling the agent breaks the navmesh 
        navMeshAgent.stoppingDistance = 999;

        // Horn Attack
        animator.SetTrigger("Horn Attack");
        animator.SetBool(isAttackingHash, true);

        // To prevent idle animation when transitioning to an attack animation
        timerDelay = 0.3f;
    }

    void HandleMovement()
    {
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance){
            animator.SetBool(isWalkingHash, true);
        }
        else{
            animator.SetBool(isWalkingHash, false);
        }

    }

    void HandleAnimations()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return;

        switch (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name)
        {
            case "Horn Attack":
                velocity += transform.forward * 10 * Time.deltaTime;
                navMeshAgent.Move(velocity * Time.deltaTime);
                break;
        }
    }
}
