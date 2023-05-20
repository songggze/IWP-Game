using UnityEngine;
using TMPro;

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
    GameObject hitBoxDisplay;
    [SerializeField] GameObject playerDamageText;

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
        hitBoxDisplay = GameObject.Find("Sword Hitbox Display");
        hitBoxDisplay.SetActive(false);
    }

    void Update()
    {

        if (playAnimation){
            currentFrame += Time.deltaTime * framesPerSecond;
            delayFrames -= Time.deltaTime * framesPerSecond;

            CheckActive();
            HandleMultiHit();
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
            // Left click attacks
            case "Slash 1":
                startUpFrames = 18;
                activeFrames = 18;
                attackModifier = 0;
                delayFrames = 25;
                isMultiHit = false;
                break;

            case "Slash 2":
                startUpFrames = 22;
                activeFrames = 30;
                attackModifier = 0;
                delayFrames = 38;
                isMultiHit = false;
                break;

            case "Slash 3":
                startUpFrames = 60;
                activeFrames = 30;
                attackModifier = 0;
                delayFrames = 0;
                isMultiHit = false;
                break;
            
            // Right click attacks
            case "Right 1":
                startUpFrames = 25;
                activeFrames = 60;
                attackModifier = 0;
                delayFrames = 64;
                isMultiHit = true;
                resetFrame = 55;
                break;

            case "Right 2":
                startUpFrames = 30;
                activeFrames = 38;
                attackModifier = 0;
                delayFrames = 0;
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

    // Collsion checking
    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Enemy" && isActive && !isHit){

            // do damage

            // For debugging

            // Rendering damage text on canvas
            GameObject damageText = Instantiate(playerDamageText);
            damageText.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            damageText.transform.Rotate(0, 0, Random.Range(-10f, 0f));

            // Setting text this way because there are two texts in 'damageTxt'
            TextMeshProUGUI[] setText;
            setText = damageText.GetComponentsInChildren<TextMeshProUGUI>();
            setText[0].SetText("Hit!");
            setText[1].SetText("Hit!");
            isHit = true;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && isActive && !isHit){

            // do damage

            // For debugging

            // Rendering damage text on canvas
            GameObject damageText = Instantiate(playerDamageText);
            damageText.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            damageText.transform.Rotate(0, 0, Random.Range(-10f, 0f));

            // Setting text this way because there are two texts in 'damageTxt'
            TextMeshProUGUI[] setText;
            setText = damageText.GetComponentsInChildren<TextMeshProUGUI>();
            setText[0].SetText("Hit!");
            setText[1].SetText("Hit!");
            isHit = true;
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        
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

    void HandleMultiHit()
    {
        if (isMultiHit && currentFrame > resetFrame)
        {
            isMultiHit = false;
            isHit = false;
        }
    }
}
