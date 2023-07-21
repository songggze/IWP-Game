using UnityEngine;

public class MonsterAttackTrigger : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerStats playerStats;

    [SerializeField] GameObject monster;
    private GroundedMonster monsterStats;
    private GroundedMonsterFD frameData;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        playerStats = player.GetComponent<PlayerStats>();
        monsterStats = monster.GetComponent<GroundedMonster>();
        frameData = monster.GetComponent<GroundedMonsterFD>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!playerStats.isHit && other.gameObject.tag == "Player"){

            Debug.Log("attack collision engage");
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
    }
}
