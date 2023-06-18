using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] GameObject staminaBar;
    private Slider staminaBarSlider;

    PlayerStats playerStats;

    // Flashing stamina bar when stamina runs out
    private bool flashingBar;
    private bool flashRed;
    private Image barImage;
    private Color barColor;

    private Color setYellow;
    private Color setRed;

    // For animation
    //private float currentValue;

    //private bool healthbarMoveUp;
    //private float healthbarTransform;
    //private Vector3 healthbarStartPos;

    void Start()
    {
        playerStats = this.GetComponent<PlayerStats>();

        staminaBarSlider = staminaBar.GetComponent<Slider>();
        staminaBarSlider.maxValue = playerStats.maxStamina;

        flashingBar = false;
        flashRed = true;

        // Set colors to change to
        setRed = Color.red;
        barImage = staminaBar.transform.Find("Stamina Image").GetComponent<Image>();
        barColor = barImage.color;
        setYellow = barColor;
    }

    // Update is called once per frame
    void Update()
    {
        staminaBarSlider.value = playerStats.stamina;        

        if (playerStats.isTired && !flashingBar){
            flashingBar = true;
        }
        else if (!playerStats.isTired && flashingBar){
            flashingBar = false;
            flashRed = false;
            barColor = setYellow;
        }

        if (flashingBar){
            HandleFlashingBar();
        }
    }


    void HandleFlashingBar()
    {
        Debug.Log(barColor);

        // Flashing will alternate to red and orange colors        
        if (flashRed){
            barColor = Color.Lerp(barColor, setRed, 30 * Time.deltaTime);

            if (barColor == setRed){
                flashRed = false;
            }
        }
        else{
            barColor = Color.Lerp(barColor, setYellow, 30 * Time.deltaTime);

            if (barColor == setYellow){
                flashRed = true;
            }
        }

        barImage.color = barColor;
    }
}
