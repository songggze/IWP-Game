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
    [SerializeField] double framesPerSecond = 60;

    // Handle damage data
    private float attackModifier;

    public bool isActive;
    public bool playAnimation;

    // Start is called before the first frame update
    void Start()
    {
        
        // init values
        currentFrame = 0;
        startUpFrames = 0;
        activeFrames = 0;

        // TODO: replace attackdelay with recoveryFrames
        recoveryFrames = 0;
        
        attackModifier = 0;
        isActive = false;

        //SetValues();
    }

    void Update()
    {

        if (playAnimation){
            currentFrame += Time.deltaTime * framesPerSecond;

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
                startUpFrames = 8;
                activeFrames = 18;
                recoveryFrames = 0;
                attackModifier = 0;
                break;

            case "Slash 2":
                startUpFrames = 0;
                activeFrames = 0;
                recoveryFrames = 0;
                attackModifier = 0;
                break;

            case "Slash 3":
                startUpFrames = 0;
                activeFrames = 0;
                recoveryFrames = 0;
                attackModifier = 0;
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

    void OnCollisonEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && isActive){

            // TODO: collision doesnt work.


            // do damage
            Debug.Log("oansteioantsoiant");
        }
    }
    
    void CheckActive()
    {
        // activate the thing
        if (currentFrame > startUpFrames &&
            currentFrame < startUpFrames + activeFrames)
        {
            Debug.Log("Hbox active");
            isActive = true;
        }
        else
        {
            isActive = false;
        }
    }
}
