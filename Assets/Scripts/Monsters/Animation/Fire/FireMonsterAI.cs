using UnityEngine;
using UnityEngine.AI;

public class FireMonsterAI : MonoBehaviour
{
    public enum AIState{
        Close,
        Ranged,
    }

    Animator animator;
    public NavMeshAgent navMeshAgent;
    FireMonsterFD frameData;
    FireMonster monsterStats;

    [SerializeField] public float setStoppingDistance = 10;
    public AIState currentState;

    int isWalkingHash;
    int isAttackingHash;
    int isDeadHash;
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

    // Prefabs
    [SerializeField] GameObject fireball_prefab;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        frameData = GetComponent<FireMonsterFD>();
        monsterStats = GetComponent<FireMonster>();
        playerStats = player.GetComponent<PlayerStats>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isAttackingHash = Animator.StringToHash("isAttack");
        isDeadHash = Animator.StringToHash("isDead");
        isTrappedHash = Animator.StringToHash("isTrapped");

        navMeshAgent.stoppingDistance = setStoppingDistance;
        velocity = Vector3.zero;

        timerDelay = 0;
        startAttack = false;

        // Set AI state
        SetState();
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
            SetState();
            // navMeshAgent.stoppingDistance = setStoppingDistance;
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
                rand = 1;
                // rand = Random.Range(5, 7);
            }
            else{
                rand = 1;

            }
        }
        else{
            // Debug Stuff
            rand = PressAttack();
        }

        switch(rand){
            case 1:
                // Fireball shooting attack
                animator.SetTrigger("Shoot Fireball");
                frameData.SetValues("Shoot Fireball");
                animator.SetBool(isAttackingHash, true);

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
            case "Shoot Fireball":
                if (frameData.isMultiHit && frameData.isRepeat){

                    GameObject fireBall = Instantiate(fireball_prefab, gameObject.transform);
                    fireBall.transform.SetParent(null);
                    fireBall.transform.position += new Vector3(0, 2, 0) - (transform.forward * 3);
                    fireBall.GetComponent<Rigidbody>().velocity = transform.forward * 10;
                    frameData.isRepeat = false;
                }
                break;
        }
    }

    private void FaceTarget()
    {
        animator.SetBool(isWalkingHash, false);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4);
    }

    private void SetState()
    {
        // Set AI state
        int rand = Random.Range(0, System.Enum.GetValues(typeof(AIState)).Length);
        currentState = (AIState) rand;

        switch (currentState)
        {
            case AIState.Close:
                Debug.Log(currentState);
                navMeshAgent.stoppingDistance = setStoppingDistance;
                break;
            case AIState.Ranged:
                Debug.Log(currentState);
                navMeshAgent.stoppingDistance = 30;
                break;
        }
    }
}
