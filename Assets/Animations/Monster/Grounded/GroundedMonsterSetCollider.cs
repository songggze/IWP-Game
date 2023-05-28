using UnityEngine;

public class GroundedMonsterSetCollider : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerStats playerStats;

    [SerializeField] GameObject monster;

    Animator monsterAnimator;
    GroundedMonsterFD frameData;
    int isAttackingHash;

    [SerializeField] GameObject jump_hitbox;


    // Start is called before the first frame update
    void Start()
    {
        jump_hitbox.SetActive(false);

        playerStats = player.GetComponent<PlayerStats>();
        monsterAnimator = monster.GetComponent<Animator>();
        frameData = monster.GetComponent<GroundedMonsterFD>();
        isAttackingHash = Animator.StringToHash("isAttack");
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
        Debug.Log(monsterAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        switch (monsterAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name){
            case "Jump":
                jump_hitbox.SetActive(true);
                break;
        }
    }

    void DisableCollider()
    {
        jump_hitbox.SetActive(false);
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
