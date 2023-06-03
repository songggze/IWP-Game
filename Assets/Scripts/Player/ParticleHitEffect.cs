using UnityEngine;

public class ParticleHitEffect : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        // Will destroy when particle effect finishes
        if (this.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
