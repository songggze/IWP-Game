using UnityEngine;

public class GroundedMonsterCollider : MonoBehaviour
{

    [SerializeField] GameObject player;
    private PlayerStats playerStats;

    [SerializeField] GameObject monster;
    private Animator monsterAnimator;

    int isAttackingHash;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        monsterAnimator = monster.GetComponent<Animator>();

        isAttackingHash = Animator.StringToHash("isAttack");
    }

    void OnTriggerStay()
    {

        // TODO: add a hitbox which should be bigger than monster's hitbox

        bool isAttack = monsterAnimator.GetBool(isAttackingHash);
        if (!playerStats.isHit){

            Debug.Log("testing");
            playerStats.isHit = true;
            playerStats.SetIFrames();

            playerStats.health -= 20;
        }
    }
}
