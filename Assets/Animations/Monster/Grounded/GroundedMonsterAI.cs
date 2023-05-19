using UnityEngine;
using UnityEngine.AI;

public class GroundedMonsterAI : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;
    GroundedMonsterFD frameData;

    [SerializeField] float setStoppingDistance = 10;

    int isWalkingHash;
    int isAttackingHash;
    
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
        // TODO: change this to include hitbox gameobject later
        frameData = GetComponent<GroundedMonsterFD>();

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
        if (isWalking && !isAttack && startAttack){
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

        // Horn Attack
        animator.SetTrigger("Horn Attack");
        frameData.SetValues("Horn Attack");
        animator.SetBool(isAttackingHash, true);

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

        switch (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name)
        {
            case "Horn Attack":
                velocity += transform.forward * 50 * Time.deltaTime;
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
