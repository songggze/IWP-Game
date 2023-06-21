using UnityEngine;

public class MonsterAttackTrigger : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerStats playerStats;

    [SerializeField] GameObject monster;
    private GroundedMonsterFD frameData;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        playerStats = player.GetComponent<PlayerStats>();
        frameData = monster.GetComponent<GroundedMonsterFD>();
    }

    void OnTriggerEnter()
    {
        if (!playerStats.isHit){

            Debug.Log("attack collision engage");
            playerStats.isHit = true;
            playerStats.SetIFrames();

            playerStats.health -= frameData.damage;
        }
    }
}
