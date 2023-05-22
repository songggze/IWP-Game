using UnityEngine;
using UnityEngine.AI;

public class GroundedMonsterAI : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;
    GroundedMonsterFD frameData;
    GroundedMonster monsterStats;

    [SerializeField] float setStoppingDistance = 10;

    int isWalkingHash;
    int isAttackingHash;
    int isDeadHash;
    
    float timerDelay;
    Vector3 velocity;

    [SerializeField] GameObject player;

    // Will only initiate attacks when player is in line of sight
    public bool startAttack;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        frameData = GetComponent<GroundedMonsterFD>();
        monsterStats = GetComponent<GroundedMonster>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isAttackingHash = Animator.StringToHash("isAttack");
        isDeadHash = Animator.StringToHash("isDead");

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
        bool isDead = animator.GetBool(isDeadHash);

        // Check if dead
        if (monsterStats.health <= 0 && !isDead){
            animator.Play("Idle");
            animator.SetBool(isDeadHash, true);
        }
        if (isDead){

            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isAttackingHash, false);
            return;
        }

        if (!isAttack){
            HandleMovement();
        }

        bool isWalking = animator.GetBool(isWalkingHash);
        if (isWalking && !isAttack && startAttack){
            HandleAttacks();
        }

        // Applying movement to attack animations since root motion is disabled for the controller
        HandleAnimations();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !isWalking && timerDelay <= 0){
            animator.SetBool(isAttackingHash, false);

            // Reset to normal value to enable movement again
            navMeshAgent.stoppingDistance = setStoppingDistance;
            velocity = Vector3.zero;

            startAttack = false;

            // Reset triggers in case it gets buffered during animation
            animator.ResetTrigger("Horn Attack");
            animator.ResetTrigger("Jump");
        }
        
        // Monster will rotate towards player when it is close enough before attacking
        if (!startAttack && isWalking && !isAttack && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
            FaceTarget();
        }

        if (timerDelay > 0){
            timerDelay -= Time.deltaTime;
        }
    }

    void HandleAttacks()
    {
        animator.SetBool(isWalkingHash, false);
        // To prevent movement while attacking
        // Setting stoppingDistance to 999 as for some reason enabling/disabling the agent breaks the navmesh 
        navMeshAgent.stoppingDistance = 999;


        // TODO: please put this in a function later
        int rand = Random.Range(1, 5);

        switch(rand){
            case 1:
                //Horn Attack
                animator.SetTrigger("Horn Attack");
                frameData.SetValues("Horn Attack");
                animator.SetBool(isAttackingHash, true);
                break;

            case 2:
                // Horn Attack
                animator.SetTrigger("Jump");
                frameData.SetValues("Jump");
                animator.SetBool(isAttackingHash, true);
                break;

            case 3:
                // Bite attack
                animator.SetTrigger("Bite");
                frameData.SetValues("Bite");
                animator.SetBool(isAttackingHash, true);
                break;

            case 4:
                // Bite attack
                animator.SetTrigger("Rush");
                frameData.SetValues("Rush");
                animator.SetBool(isAttackingHash, true);
                break;
        }



        // To prevent idle animation when transitioning to an attack animation
        timerDelay = 0.9f;
    }

    void HandleMovement()
    {
        animator.SetBool(isWalkingHash, true);
    }

    void HandleAnimations()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            frameData.currentFrame < frameData.movementStart ||
            frameData.currentFrame > frameData.movementStop)
            return;

        switch (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name){
            case "Horn Attack":
                velocity += transform.forward * 60 * Time.deltaTime;
                navMeshAgent.Move(velocity * Time.deltaTime);
                break;
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1.5f);
    }
}
