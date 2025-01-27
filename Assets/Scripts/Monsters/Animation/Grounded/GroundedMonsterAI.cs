using UnityEngine;
using UnityEngine.AI;

public class GroundedMonsterAI : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;
    GroundedMonsterFD frameData;
    GroundedMonster monsterStats;

    [SerializeField] public float setStoppingDistance = 10;

    int isWalkingHash;
    int isAttackingHash;
    int isDeadHash;
    int isRushingHash;
    int isTrappedHash;
    
    float timerDelay;
    Vector3 velocity;

    [SerializeField] GameObject player;
    PlayerStats playerStats;

    // Will only initiate attacks when player is in line of sight
    public bool startAttack;
    public bool mirrorAttack = false;

    // Debugging
    [SerializeField] bool debugAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        frameData = GetComponent<GroundedMonsterFD>();
        monsterStats = GetComponent<GroundedMonster>();
        playerStats = player.GetComponent<PlayerStats>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isAttackingHash = Animator.StringToHash("isAttack");
        isDeadHash = Animator.StringToHash("isDead");
        isRushingHash = Animator.StringToHash("isRushing");
        isTrappedHash = Animator.StringToHash("isTrapped");

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
        bool isTrapped = animator.GetBool(isTrappedHash);

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

        // Movement
        if (!isAttack && !isTrapped){
            HandleMovement();
        }

        // Attack
        bool isWalking = animator.GetBool(isWalkingHash);
        if (isWalking && !isAttack && startAttack && !isTrapped){
            HandleAttacks();
        }

        // Some attack animatons will have movement
        AttackAnimationMovement();

        // Return to idle state after attack atnimaton ends
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !isWalking && timerDelay <= 0){
            animator.SetBool(isAttackingHash, false);

            // Reset to normal value to enable movement again
            navMeshAgent.stoppingDistance = setStoppingDistance;
            velocity = Vector3.zero;

            if (monsterStats.isEnraged){
                animator.speed = monsterStats.enrageSpeedModifier;
            }
            else{
                animator.speed = 0.75f;
            }

            startAttack = false;
            mirrorAttack = false;

            // Reset triggers in case it gets buffered during animation
            // animator.ResetTrigger("Horn Attack");
            // animator.ResetTrigger("Jump");

            animator.SetBool(isRushingHash, false);
        }

        // brute force thing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
            frameData.currentFrame = 0;
        }
        
        // Monster will rotate towards player when it is close enough before attacking
        if (!startAttack && isWalking && !isAttack && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
            FaceTarget();
        }

        if (mirrorAttack){
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
        else{
            this.transform.localScale = new Vector3(1, 1, 1);
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

        if (playerStats.isDead){
            return;
        }

        // Choose Attacks
        int rand;
        if (!debugAttacking){
            if (monsterStats.isEnraged){
                rand = Random.Range(1, 7);
                // rand = Random.Range(5, 7);
            }
            else{
                rand = Random.Range(1, 3);
            }
        }
        else{
            // Debug Stuff
            rand = PressAttack();
        }

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
                // Rush attack
                animator.SetBool("isRushing", true);
                frameData.SetValues("Rush");
                animator.SetBool(isAttackingHash, true);

                break;
            case 5:
                // Claw attack
                animator.SetTrigger("Claw");
                frameData.SetValues("Claw Attack");
                animator.SetBool(isAttackingHash, true);

                break;
            case 6:
                // Claw attack mirror
                animator.SetTrigger("Claw");
                frameData.SetValues("Claw Attack");
                animator.SetBool(isAttackingHash, true);

                mirrorAttack = true;
                break;
        }

        // To prevent idle animation when transitioning to an attack animation
        timerDelay = 2f;
    }

    // For debugging
    int PressAttack()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            return 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            return 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            return 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            return 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            return 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {

            return 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {

            return 6;
        }

        return -1;
    }

    void HandleMovement()
    {
        animator.SetBool(isWalkingHash, true);
    }

    void AttackAnimationMovement()
    {

        // Stop the rush attack
        if (frameData.currentFrame > frameData.totalFrames)
        {
            animator.SetBool(isRushingHash, false);
        }

        // This will move monster for a specific period of time
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
            frameData.currentFrame < frameData.movementStart ||
            frameData.currentFrame > frameData.movementStop)
        {
            return;
        }

        if (animator.GetCurrentAnimatorClipInfo(0).Length == 0){
            return;
        }

            
        switch (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name){
            case "Horn Attack":
                velocity += transform.forward * 80 * Time.deltaTime * animator.speed;
                navMeshAgent.Move(velocity * Time.deltaTime);
                break;
            case "Claw Attack":
                if (mirrorAttack){
                    velocity += transform.right * 80 * Time.deltaTime;
                }
                else{
                    velocity -= transform.right * 80 * Time.deltaTime;
                }
                velocity += transform.forward * 25 * ClawDistance() * Time.deltaTime;
                navMeshAgent.Move(velocity * Time.deltaTime);
                break;

            case "Rush":

                // Add a velocity limit to the rush attack
                if (velocity.magnitude < 30){
                    velocity += transform.forward * 40 * Time.deltaTime;
                }
                navMeshAgent.Move(velocity * Time.deltaTime);

                // This attack will track the player periodially
                if (frameData.isRepeat && frameData.repeatTimer > frameData.repeatFrames - 20){
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20);
                    velocity = Vector3.zero;

                }
                break;
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1.5f);
    }

    float ClawDistance(){

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist > 7){
            dist = 7;
        }
        return dist;
    }
}
