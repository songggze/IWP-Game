using UnityEngine;

public class GroundedMonsterCollider : MonoBehaviour
{

    [SerializeField] GameObject player;
    private PlayerStats playerStats;
    private Animator playerAnimator;

    [SerializeField] GameObject monster;
    private Animator monsterAnimator;

    private GroundedMonsterFD frameData;
    [SerializeField] public string bodyType = "NULL";
    
    int isAttackHash;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        playerAnimator = player.GetComponent<Animator>();
        monsterAnimator = monster.GetComponent<Animator>();

        frameData = monster.GetComponent<GroundedMonsterFD>();

        isAttackHash = Animator.StringToHash("isAttack");
    }
    
    // For debugging
    void Update()
    {
        bool isAttack = monsterAnimator.GetBool(isAttackHash);
        if (isAttack &&
            frameData.currentFrame > frameData.startUpFrames &&
            frameData.currentFrame < frameData.startUpFrames + frameData.activeFrames){
            Debug.Log("Monster hitbox active");
        }

        playerStats.prevPosition = player.transform.position;
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player"){

            bool isAttack = monsterAnimator.GetBool(isAttackHash);

            // To be able hit the player:
            //---------------------------------------------------------------------------------
            // - The player is not in invincibility state
            // - The monster is in an attacking state
            // - The attack animation as be within the "active frames" period, which determines
            //   how long the hitboxes last
            //----------------------------------------------------------------------------------
            if (!playerStats.isHit && isAttack &&
                frameData.currentFrame > frameData.startUpFrames &&
                frameData.currentFrame < frameData.startUpFrames + frameData.activeFrames)
            {

                playerStats.isHit = true;

                playerStats.SetIFrames();

                playerStats.health -= 20;
            }

            Debug.Log("Position is reverted");
            player.transform.position = playerStats.prevPosition;

            if (playerStats.isHit)
            {

                player.transform.position -= player.transform.forward * 10 * Time.deltaTime;
            }
        }
    }

    void OnCollisionStay(Collision other)
    {  
        if (other.gameObject.tag == "Player"){

            Debug.Log("Inside");
            player.transform.position = playerStats.prevPosition;

            if (playerStats.isHit)
            {

                player.transform.position -= player.transform.forward * 10 * Time.deltaTime;
            }
        }
    }


}
