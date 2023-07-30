using UnityEngine;
using TMPro;

public class DebugCanvas : MonoBehaviour
{
    [SerializeField] GameObject monster;
    GroundedMonster monsterData;
    GroundedMonsterFD frameData;
    FireMonster fireMonsterData;
    FireMonsterFD fireFrameData;

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
        switch(monster.transform.root.name)
        {
            case "Grounded Monster":
                frameData = monster.GetComponent<GroundedMonsterFD>();
                monsterData = monster.GetComponent<GroundedMonster>();
                monsterMaxHealth = monsterData.health;
                break;

            case "Fire Monster":
                fireFrameData = monster.GetComponent<FireMonsterFD>();
                fireMonsterData = monster.GetComponent<FireMonster>();
                monsterMaxHealth = fireMonsterData.health;
                break;
        }
        gameManager = gameManagerObj.GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        // Display current animation name
        if (!CheckAnimationValid()){
            anim_name.text = "??????????";
        }
        else{

            switch (monster.transform.root.name)
            {
                case "Grounded Monster":
                    anim_name.text = monsterData.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    break;

                case "Fire Monster":
                    anim_name.text = fireMonsterData.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    break;
            }
        }

        // Display frame datas
        switch (monster.transform.root.name)
        {
            case "Grounded Monster":
                total_text.text = "Total: " + frameData.totalFrames.ToString();
                active_text.text = "Active: " + frameData.activeFrames.ToString();
                current_text.text = "Current: " + frameData.currentFrame.ToString();

                // Display dps text
                dps_text.text = "DPS: " + ((monsterMaxHealth - monsterData.health) / ((gameManager.timeMinute * 60) + gameManager.timeSecond)).ToString();
                break;

            case "Fire Monster":
                total_text.text = "Total: " + fireFrameData.totalFrames.ToString();
                active_text.text = "Active: " + fireFrameData.activeFrames.ToString();
                current_text.text = "Current: " + fireFrameData.currentFrame.ToString();

                // Display dps text
                dps_text.text = "DPS: " + ((monsterMaxHealth - fireMonsterData.health) / ((gameManager.timeMinute * 60) + gameManager.timeSecond)).ToString();
                break;
        }

    }

    bool CheckAnimationValid()
    {

        switch (monster.transform.root.name)
        {
             case "Grounded Monster":
                if (monsterData.animator.GetCurrentAnimatorClipInfo(0).Length == 0)
                    return false;
                break;

             case "Fire Monster":
                if (fireMonsterData.animator.GetCurrentAnimatorClipInfo(0).Length == 0)
                    return false;
                break;
        }

        return true;
    }
}
