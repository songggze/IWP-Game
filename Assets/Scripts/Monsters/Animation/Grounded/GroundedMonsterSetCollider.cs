using UnityEngine;

public class GroundedMonsterSetCollider : MonoBehaviour
{
    // Player
    [SerializeField] GameObject player;
    private PlayerStats playerStats;

    // Monster
    [SerializeField] GameObject monster;

    // Animator
    Animator monsterAnimator;
    GroundedMonsterFD frameData;
    int isAttackingHash;

    // Hitboxes
    [SerializeField] GameObject jump_hitbox;
    [SerializeField] GameObject lunge_hitbox;
    [SerializeField] GameObject running_hitbox;
    [SerializeField] GameObject bite_hitbox;
    [SerializeField] GameObject claw_hitbox;

    // Particle Effects
    bool playParticleEffect;
    [SerializeField] ParticleEffect particleEffect;

    [System.Serializable]
    public class ParticleEffect
    {
        [SerializeField] public GameObject jump_effect;
    };

    // Start is called before the first frame update
    void Start()
    {
        jump_hitbox.SetActive(false);
        lunge_hitbox.SetActive(false);
        running_hitbox.SetActive(false);
        bite_hitbox.SetActive(false);
        claw_hitbox.SetActive(false);

        playerStats = player.GetComponent<PlayerStats>();
        monsterAnimator = monster.GetComponent<Animator>();
        frameData = monster.GetComponent<GroundedMonsterFD>();
        isAttackingHash = Animator.StringToHash("isAttack");
        
        playParticleEffect = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        bool isAttack = monsterAnimator.GetBool(isAttackingHash);
        if (isAttack &&
            frameData.currentFrame > frameData.startUpFrames &&
            frameData.currentFrame < frameData.startUpFrames + frameData.activeFrames){
            
            SetCollider();
        }
        else{
            
            DisableCollider();
        }
    }

    void SetCollider()
    {
        //Debug.Log(monsterAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        switch (monsterAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name)
        {
            case "Jump":

                jump_hitbox.SetActive(true);

                // Play particle effect
                if (!playParticleEffect){
                    GameObject temp = GameObject.Instantiate(particleEffect.jump_effect);
                    temp.transform.position = monster.transform.position;
                    playParticleEffect = true;
                }

                break;
            case "Horn Attack":

                lunge_hitbox.SetActive(true);

                break;

            case "Rush":

                running_hitbox.SetActive(true);

                break;

            case "Bite":

                bite_hitbox.SetActive(true);

                break;
            case "Claw Attack":
                claw_hitbox.SetActive(true);

                break;
        }
    }

    void DisableCollider()
    {
        jump_hitbox.SetActive(false);
        lunge_hitbox.SetActive(false);
        running_hitbox.SetActive(false);
        bite_hitbox.SetActive(false);
        claw_hitbox.SetActive(false);
        playParticleEffect = false;
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
