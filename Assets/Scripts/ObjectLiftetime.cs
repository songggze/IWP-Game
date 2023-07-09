using UnityEngine;

public class ObjectLiftetime : MonoBehaviour
{
    // For objects which appear for a set amount of time

    public float lifetime = 0;

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0){
            Destroy(gameObject);
        }        
    }
}
