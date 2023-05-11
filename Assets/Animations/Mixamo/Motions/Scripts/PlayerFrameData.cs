using UnityEngine;

public class PlayerFrameData : MonoBehaviour
{
    //public enum AttackType{
        //Slash_1,
        //Slash_2,
        //Slash_3
    //}

    // Frame data for attack collisions
    private double currentFrame;
    private double startUpFrames;
    private double activeFrames;
    private double recoveryFrames;
    private double totalFrames;
    public double delayFrames;
    [SerializeField] double framesPerSecond = 60;

    // Handle damage data
    private float attackModifier;

    public bool isActive;
    public bool playAnimation;

    // Debugging
    GameObject hitText;
    GameObject hitBoxDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
        // init values
        currentFrame = 0;
        startUpFrames = 0;
        activeFrames = 0;

        // TODO: replace attackdelay with recoveryFrames
        recoveryFrames = 0;
        delayFrames = 0;
        
        attackModifier = 0;
        isActive = false;

        // Debugging
        hitText = GameObject.Find("Debug Hit Text");
        hitText.SetActive(false);

        hitBoxDisplay = GameObject.Find("Sword Hitbox Display");
        hitBoxDisplay.SetActive(false);
    }

    void Update()
    {

        if (playAnimation){
            currentFrame += Time.deltaTime * framesPerSecond;
            delayFrames -= Time.deltaTime * framesPerSecond;

            CheckActive();
        }

        if (currentFrame > totalFrames && playAnimation){

            Debug.Log("Attack ended");
            playAnimation = false;
        }
        
        // Checks when to activate hitboxes during animations
    }

    public void SetValues(string animationName)
    {
        switch(animationName){
            case "Slash 1":
                startUpFrames = 18;
                activeFrames = 18;
                recoveryFrames = 0;
                attackModifier = 0;
                delayFrames = 25;
                break;

            case "Slash 2":
                startUpFrames = 22;
                activeFrames = 30;
                recoveryFrames = 0;
                attackModifier = 0;
                delayFrames = 38;
                break;

            case "Slash 3":
                startUpFrames = 60;
                activeFrames = 60;
                recoveryFrames = 0;
                attackModifier = 0;
                delayFrames = 0;
                break;

            default:
                Debug.Log("Attack not found!!");
                break;
         }

        totalFrames = startUpFrames + activeFrames + recoveryFrames;
        currentFrame = 0;
        isActive = true;
        playAnimation = true;
    }

    // Collsion checking
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && isActive){

            // do damage

            // For debugging
            hitText.SetActive(true);
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        
        if (other.gameObject.tag == "Enemy"){


            hitText.SetActive(false);
        }
    }
    
    void CheckActive()
    {
        // Set active hitbox for sword
        if (currentFrame > startUpFrames &&
            currentFrame < startUpFrames + activeFrames)
        {
            Debug.Log("Hbox active");
            isActive = true;

            // Hitbox display for debugging
            hitBoxDisplay.SetActive(true);
        }
        else
        {
            isActive = false;
            hitBoxDisplay.SetActive(false);
        }
    }
}
