using UnityEngine;

public class FireMonsterCollider : MonoBehaviour
{

    [SerializeField] GameObject player;
    private PlayerStats playerStats;
    private Animator playerAnimator;

    [SerializeField] GameObject monster;
    private FireMonster monsterStats;
    private Animator monsterAnimator;

    private FireMonsterFD frameData;
    [SerializeField] public string bodyType = "NULL";
    
    int isAttackHash;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        playerAnimator = player.GetComponent<Animator>();
        monsterStats = player.GetComponent<FireMonster>();
        monsterAnimator = monster.GetComponent<Animator>();

        frameData = monster.GetComponent<FireMonsterFD>();

        isAttackHash = Animator.StringToHash("isAttack");
    }
    
    // For debugging
    void Update()
    {
        bool isAttack = monsterAnimator.GetBool(isAttackHash);
        if (isAttack &&
            frameData.currentFrame > frameData.startUpFrames &&
            frameData.currentFrame < frameData.startUpFrames + frameData.activeFrames){
            //Debug.Log("Monster hitbox active");
        }

        playerStats.prevPosition = player.transform.position;
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player"){

            Debug.Log(frameData.damage);

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

                if (monsterStats.isEnraged)
                {
                    Debug.Log("enraged collider");
                    playerStats.health -= frameData.damage * monsterStats.enrageAttackModifier;
                }
                else
                {
                    playerStats.health -= frameData.damage;
                }
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

            player.transform.position -= player.transform.forward * 10 * Time.deltaTime;
            player.transform.position = new Vector3(player.transform.position.x, -0.57f, player.transform.position.z);
        }
        // TODO: player teleporting above monster....
    }

    void OnCollisionExit(Collision other)
    {

        player.transform.position = new Vector3(player.transform.position.x, -0.57f, player.transform.position.z);
    }

}
