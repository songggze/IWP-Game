using UnityEngine;

public class GroundedMonsterFD : MonoBehaviour
{
    //
    //  Frame data for Grounded Monster. 
    //

    private double currentFrame;
    private double startUpFrames;
    private double activeFrames;
    private double totalFrames;
    public double delayFrames;
    public double resetFrame;
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

        // Debugging
        hitText = GameObject.Find("Debug Hit Text");
        hitText.SetActive(false);

        hitBoxDisplay = GameObject.Find("Grounded Monster Hitbox Display");
        hitBoxDisplay.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues(string animationName)
    {
        switch(animationName){
            case "Horn Attack":
                startUpFrames = 18;
                activeFrames = 18;
                attackModifier = 0;
                delayFrames = 25;
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
