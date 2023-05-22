using UnityEngine;

public class MonsterAttackTrigger : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);

        playerStats = player.GetComponent<PlayerStats>();
    }

    void OnTriggerEnter()
    {
        if (!playerStats.isHit){

            Debug.Log("attack collision engage");
            playerStats.isHit = true;
            playerStats.SetIFrames();

            playerStats.health -= 20;
        }
    }
}
