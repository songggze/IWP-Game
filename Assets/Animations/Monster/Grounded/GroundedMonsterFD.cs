using UnityEngine;

public class GroundedMonsterFD : MonoBehaviour
{
    //
    //  Frame data for Grounded Monster. 
    //

    public double currentFrame;
    public double startUpFrames;
    public double activeFrames;
    private double totalFrames;
    public double delayFrames;
    public double resetFrame;

    // Frame indicator when to start/stop movement during animation
    public double movementStart;
    public double movementStop;
    [SerializeField] double framesPerSecond = 60;

    // Handle damage data
    private float attackModifier;

    public bool isActive;
    public bool playAnimation;
    public bool isHit;
    public bool isMultiHit;

    // Debugging
    GameObject hitText;
    GameObject hitBoxDisplay;
    //[SerializeField] GameObject playerDamageText;

    // Start is called before the first frame update
    void Start()
    {
        
        // init values
        currentFrame = 0;
        startUpFrames = 0;
        activeFrames = 0;

        delayFrames = 0;
        resetFrame = 0;
        
        attackModifier = 0;
        isActive = false;
        isHit = false;
        isMultiHit = false;

    }


    // Update is called once per frame
    void Update()
    {
        
        if (playAnimation){
            currentFrame += Time.deltaTime * framesPerSecond;
        }

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

                movementStart = 50;
                movementStop = 75; 
                isMultiHit = false;
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
    }
}
