using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerStats playerStats;

    [SerializeField] GameObject objectRoot;

    public float damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player Character");
        playerStats = player.GetComponent<PlayerStats>();
    }

    void Update()
    {
        // Apply rotation (if any)
        transform.Rotate(50 * Time.deltaTime, 0, 50 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!playerStats.isHit && other.gameObject.tag == "Player"){
            playerStats.isHit = true;
            playerStats.SetIFrames();
            playerStats.health -= damage;
            Destroy(objectRoot);
        }
    }
}
