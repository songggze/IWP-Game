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
    private float attackModifier;

    public bool isActive;
    public bool playAnimation;
    public bool isHit;
    public bool isMultiHit;
    public bool isRepeat;

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
        
        attackModifier = 0;
        isActive = false;
        isHit = false;
        isMultiHit = false;
        isRepeat = false;

        hitboxTextDisplay.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
        if (playAnimation){
            currentFrame += Time.deltaTime * framesPerSecond;
        }

        if (isRepeat){
            repeatTimer += Time.deltaTime * framesPerSecond;
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
                startUpFrames = 50;
                activeFrames = 25;
                attackModifier = 0;
                delayFrames = 25;

                // Attack has movement
                movementStart = 50;
                movementStop = 80; 
                isMultiHit = false;
                isRepeat = false;
                break;

            case "Jump":
                startUpFrames = 100;
                activeFrames = 13;
                attackModifier = 0;
                delayFrames = 25;

                isMultiHit = false;
                isRepeat = false;
                break;

            case "Bite":
                startUpFrames = 40;
                activeFrames = 15;
                attackModifier = 0;
                delayFrames = 25;

                isMultiHit = false;
                isRepeat = false;
                break;

            case "Rush":
                startUpFrames = 80;
                activeFrames = 500;
                attackModifier = 0;
                delayFrames = 25;

                // Attack has movement
                movementStart = 80;
                movementStop = 580; 
                isMultiHit = false;
                isRepeat = true;
                repeatFrames = 150;
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
