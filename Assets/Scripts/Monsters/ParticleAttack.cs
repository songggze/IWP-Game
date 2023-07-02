using UnityEngine;

public class ParticleAttack : MonoBehaviour
{

    [SerializeField] GameObject monster;
    Animator animator;

    [SerializeField] GameObject claw_particle;

    // Start is called before the first frame update
    void Start()
    {
        animator = monster.GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Claw Attack")){
            claw_particle.SetActive(true);
        }
        else{
            claw_particle.SetActive(false);
        }
    }
}
