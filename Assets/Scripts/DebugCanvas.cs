using UnityEngine;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] GameObject monster;
    GroundedMonster monsterData;
    GroundedMonsterFD frameData;

    [SerializeField] TextMeshProUGUI anim_name;
    [SerializeField] TextMeshProUGUI total_text;
    [SerializeField] TextMeshProUGUI active_text;
    [SerializeField] TextMeshProUGUI current_text;
    [SerializeField] TextMeshProUGUI dps_text;

    [SerializeField] GameObject gameManagerObj;
    GameManager gameManager;

    float monsterMaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        frameData = monster.GetComponent<GroundedMonsterFD>();        
        monsterData = monster.GetComponent<GroundedMonster>();
        gameManager = gameManagerObj.GetComponent<GameManager>();

        monsterMaxHealth = monsterData.health;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (monsterData.animator.GetCurrentAnimatorClipInfo(0).Length == 0){
            anim_name.text = "??????????";
        }
        else{

            anim_name.text = monsterData.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        }
        total_text.text = "Total: " + frameData.totalFrames.ToString();
        active_text.text = "Active: " + frameData.activeFrames.ToString();
        current_text.text = "Current: " +  frameData.currentFrame.ToString();

        // Display dps text
        dps_text.text = "DPS: " + ((monsterMaxHealth - monsterData.health) / ((gameManager.timeMinute * 60) + gameManager.timeSecond)).ToString();
    }
}
