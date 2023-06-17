using UnityEngine;

public class SetPos : MonoBehaviour
{
    // This script is allow easier editing of hit/hurtboxes without
    // going in the model hierarchy as you need the model transforms
    // to reference from

    [SerializeField] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       this.transform.position = target.transform.position; 
    }

}    