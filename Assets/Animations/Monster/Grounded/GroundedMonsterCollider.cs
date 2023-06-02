using UnityEngine;

public class GroundedMonsterCollider : MonoBehaviour
{

    [SerializeField] GameObject player;
    private PlayerStats playerStats;
    private Animator playerAnimator;

    [SerializeField] GameObject monster;
    private Animator monsterAnimator;

    private GroundedMonsterFD frameData;
    
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
    }

    void OnCollisionEnter()
    {

        // TODO: add a hitbox which should be bigger than monster's hitbox

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
            frameData.currentFrame < frameData.startUpFrames + frameData.activeFrames){

            playerStats.isHit = true;

            playerStats.SetIFrames();

            playerStats.health -= 20;
        }
    }

}
