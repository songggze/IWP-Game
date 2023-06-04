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

    GameObject player;
    Vector3 direction;
    [SerializeField] float walkSpeed = 2;

    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("Player Character");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(player.transform.position);

        //Quaternion currentRotation = transform.rotation;

        //Vector3 targetPosition;
        //targetPosition.x = direction.x;
        //targetPosition.y = 0;
        //targetPosition.z = direction.z;

        //Quaternion targetRotation = Quaternion.LookRotation(targetPosition);
        //transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime);
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
                return 1.2f;

            case "Body":
                return 0.7f;

            case "Tail":
                return 1;
            
            default:
                Debug.Log("Body Part does not exist!!");
                return 0;
        }
    }
}
