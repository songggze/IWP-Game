using UnityEngine;

public class GroundedMonsterRaycast : MonoBehaviour
{
    [SerializeField] GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            Debug.Log("iretnisrnortnostnrnoisrt");
            monster.GetComponent<GroundedMonsterAI>().startAttack = true;
        }
    }
}
