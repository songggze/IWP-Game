using UnityEngine;

public class FireMonsterRaycast : MonoBehaviour
{
    [SerializeField] GameObject monster;
    FireMonsterAI monsterAI;

    void Start()
    {
        monsterAI = monster.GetComponent<FireMonsterAI>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player"
            && monsterAI.navMeshAgent.remainingDistance <= monsterAI.navMeshAgent.stoppingDistance){
            monster.GetComponent<FireMonsterAI>().startAttack = true;
        }
    }
}
