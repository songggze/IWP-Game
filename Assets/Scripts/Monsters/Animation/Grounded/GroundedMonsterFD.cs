using UnityEngine;

public class GroundedMonsterFD : MonoBehaviour
{
    //
    //  Frame data for Grounded Monster. 
    //

    public double currentFrame;
    public double startUpFrames;
    public double activeFrames;
    public double totalFrames;
    public double delayFrames;

    // Frame indicator when to start/stop movement during animation
    public double movementStart;
    public double movementStop;
    [SerializeField] double framesPerSecond = 1000;

    // For attack animations that multi phases
    public double repeatFrames;
    public double repeatTimer;

    // Handle damage data
    public float damage;

    public bool isActive;
    public bool playAnimation;
    public bool isHit;
    public bool isMultiHit;
    public bool isRepeat;

    Animator animator;
    GroundedMonster monster;

    // Debugging
    [SerializeField] GameObject hitboxTextDisplay;
    //GameObject hitText;
    //GameObject hitBoxDisplay;
    //[SerializeField] GameObject playerDamageText;

    // Start is called before the first frame update
    void Start()
    {
        
        // init values
        currentFrame = 0;
        startUpFrames = 0;
        activeFrames = 0;
        playAnimation = false;

        delayFrames = 0;
        repeatTimer = 0;
        repeatFrames = 0;
        
        damage = 0;
        isActive = false;
        isHit = false;
        isMultiHit = false;
        isRepeat = false;

        hitboxTextDisplay.SetActive(false);
        animator = GetComponent<Animator>();
        monster = GetComponent<GroundedMonster>();
    }


    // Update is called once per frame
    void Update()
    {
        
        if (playAnimation){
            currentFrame += Time.deltaTime * framesPerSecond * animator.speed;
        }

        if (isRepeat && currentFrame > startUpFrames){
            repeatTimer += Time.deltaTime * framesPerSecond * animator.speed;

            if (repeatTimer > repeatFrames){
                repeatTimer = 0;
            }
        }

        // For debugging
        ShowHitboxDisplay();

        //if (currentFrame > totalFrames && playAnimation){

            //Debug.Log("Attack ended");
            //playAnimation = false;
        //}
    }

    public void SetValues(string animationName)
    {
        switch(animationName){
            case "Horn Attack":
                damage = 20;

                startUpFrames = 50;
                activeFrames = 25;
                delayFrames = 25;

                // Attack has movement
                movementStart = 50;
                movementStop = 80; 
                isMultiHit = false;
                isRepeat = false;
                break;

            case "Jump":
                damage = 30;

                startUpFrames = 100;
                activeFrames = 6;
                delayFrames = 25;

                isMultiHit = false;
                isRepeat = false;
                break;

            case "Bite":
                damage = 15;

                startUpFrames = 53;
                activeFrames = 10;
                delayFrames = 25;

                isMultiHit = false;
                isRepeat = false;
                break;

            case "Rush":
                damage = 35;

                startUpFrames = 80;
                activeFrames = 500;
                delayFrames = 25;

                // Attack has movement
                movementStart = 80;
                movementStop = 660; 
                isMultiHit = false;
                isRepeat = true;
                repeatFrames = 130;
                break;

            case "Claw Attack":
                damage = 20;

                startUpFrames = 83;
                activeFrames = 32;
                delayFrames = 5;

                movementStart = 83;
                movementStop = 93; 

                isMultiHit = false;
                isRepeat = false;
                
                // if (monster.isEnraged){
                //     animator.speed = 0.9f;
                // }
                break;

            default:
                Debug.Log("Attack not found!!");
                break;
         }

        totalFrames = startUpFrames + activeFrames;
        currentFrame = 0;
        isHit = false;
        isActive = false;
        playAnimation = true;
        repeatTimer = 0;
    }

    void ShowHitboxDisplay(){

        if (currentFrame > startUpFrames &&
            currentFrame < startUpFrames + activeFrames)
        {
            hitboxTextDisplay.SetActive(true);
        }
        else{
            hitboxTextDisplay.SetActive(false);
        }
    }
}
