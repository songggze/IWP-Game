using UnityEngine;

public class GroundedMonsterRaycast : MonoBehaviour
{
    [SerializeField] GameObject monster;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            Debug.Log("iretnisrnortnostnrnoisrt");
            monster.GetComponent<GroundedMonsterAI>().startAttack = true;
        }
    }
}
