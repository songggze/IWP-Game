using UnityEngine;
using TMPro;

public class PlayerFrameData : MonoBehaviour
{
    //public enum AttackType{
        //Slash_1,
        //Slash_2,
        //Slash_3
    //}
    [SerializeField] GameObject player;
    PlayerStats playerStats;

    [SerializeField] GameObject monster;
    GroundedMonster monsterStats;

    // Frame data for attack collisions
    private double currentFrame;
    private double startUpFrames;
    private double activeFrames;
    private double totalFrames;
    public double delayFrames;
    public double resetFrame;
    [SerializeField] double framesPerSecond = 60;

    float attackModifier;
    public bool isActive;
    public bool playAnimation;
    public bool isHit;
    public bool isMultiHit;

    // Camera shake effect
    [SerializeField] GameObject camera;
    CameraShake vcam;

    // Particle effect when hit enemy
    [SerializeField] GameObject hitEffect;

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

        vcam = camera.GetComponent<CameraShake>();

        playerStats = player.GetComponent<PlayerStats>();
        monsterStats = monster.GetComponent<GroundedMonster>();

        // Debugging
        hitBoxDisplay = GameObject.Find("Sword Hitbox Display");
        hitBoxDisplay.SetActive(false);
    }

    void Update()
    {
        // Attack animation is playing
        if (playAnimation){


            // Cancel animation when player hurt
            if (playerStats.isHit){
                playAnimation = false;
                isActive = false;
                hitBoxDisplay.SetActive(false);
                return;
            }

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
                attackModifier = 1.2f;
                startUpFrames = 18;
                activeFrames = 18;
                delayFrames = 25;
                isMultiHit = false;
                break;

            case "Slash 2":
                attackModifier = 1.3f;
                startUpFrames = 22;
                activeFrames = 30;
                delayFrames = 38;
                isMultiHit = false;
                break;

            case "Slash 3":
                attackModifier = 2.5f;
                startUpFrames = 60;
                activeFrames = 30;
                delayFrames = 0;
                isMultiHit = false;
                break;
            
            // Right click attacks
            case "Right 1":
                attackModifier = 0.8f;
                startUpFrames = 25;
                activeFrames = 60;
                delayFrames = 64;
                isMultiHit = true;
                resetFrame = 55;
                break;

            case "Right 2":
                attackModifier = 1.7f;
                startUpFrames = 30;
                activeFrames = 38;
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

    
    void CheckActive()
    {
        // Set active hitbox for sword
        if (currentFrame > startUpFrames &&
            currentFrame < startUpFrames + activeFrames)
        {
            
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

    // For attack which have multiple hiboxes in a single animation
    void HandleMultiHit()
    {
        if (isMultiHit && currentFrame > resetFrame)
        {
            isMultiHit = false;
            isHit = false;
        }
    }

    // Collsion checking
    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Enemy" && isActive && !isHit){

            // Handle monster damaging and rendering damage numbers
            DamageMonster(other.gameObject);

            // Begin camera shake when hit
            vcam.SetShake();

            // Render particle effects
            GameObject effect = Instantiate(hitEffect);
            effect.transform.position = this.transform.position;
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && isActive && !isHit){

            
            // Handle monster damaging and rendering damage numbers
            DamageMonster(other.gameObject);

            // Begin camera shake when hit
            vcam.SetShake();

            // Render particle effects
            GameObject effect = Instantiate(hitEffect);
            effect.transform.position = this.transform.position;
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        
    }

    private void DamageMonster(GameObject target)
    {
        // Get the attack mulitplier based on what body part is affected
        float partModifier = monsterStats.GetHitzoneModifier(target.GetComponent<GroundedMonsterCollider>().bodyType);

        // Damage calculation, rounded up
        int totalDamage = (int) Mathf.Ceil((playerStats.attack * attackModifier) * partModifier);
        monsterStats.health -= totalDamage;

        // Damage Text rendering
        GameObject damageText = Instantiate(playerDamageText);
        damageText.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        damageText.transform.Rotate(0, 0, Random.Range(-10f, 0f));

        // Setting text this way because there are two texts in 'damageTxt'
        TextMeshProUGUI[] setText;
        setText = damageText.GetComponentsInChildren<TextMeshProUGUI>();
        setText[0].SetText(totalDamage.ToString());
        setText[1].SetText(totalDamage.ToString());
        isHit = true;

        Debug.Log("Monster Health: " + monsterStats.health);
    }
}
