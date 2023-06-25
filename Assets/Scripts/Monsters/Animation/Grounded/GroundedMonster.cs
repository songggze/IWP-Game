using UnityEngine;
using UnityEngine.AI;

public class GroundedMonster : MonoBehaviour
{
    //enum State{
        //Idle,
        //Walking,
        //Running,
        //Attack,
    //}

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

    GameObject player;
    Vector3 direction;
    NavMeshAgent navMeshAgent;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player Character");
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.speed = 0.8f;

        enrageThreshold = set_enrageThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        // Set new position to track
        navMeshAgent.SetDestination(player.transform.position);

        // Check for enraged status
        HandleEnragedStatus();
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
        // Enrage when player does enough damage
        if (!isEnraged && enrageThreshold <= 0){

            isEnraged = true;
            enrageTimer = set_enrageTimer;
            animator.Play("Roar");
            animator.speed = enrageSpeedModifier;
        }

        // Enrage timer ticking
        if (isEnraged & enrageTimer > 0){

            enrageTimer -= Time.deltaTime;
            Debug.Log(enrageTimer);

            // Disable enrage status
            if (enrageTimer <= 0){
                isEnraged = false;
                enrageThreshold = set_enrageThreshold;
                animator.speed = 1;
            }
        }
    }
}
