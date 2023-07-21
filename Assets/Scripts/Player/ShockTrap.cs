using UnityEngine;

public class ShockTrap : MonoBehaviour
{

    [SerializeField] GameObject monster;

    // Start is called before the first frame update
    void Start()
    {
        monster = GameObject.Find("Monster1");    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        
        // Destroys the shock trap and starts the trapping animation
        if (other.gameObject.tag == "Enemy"){
            monster.GetComponent<GroundedMonster>().HandleTrapped();
            Debug.Log("trapped");
            Destroy(this.gameObject);
        }
    }



}
