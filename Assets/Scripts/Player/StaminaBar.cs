using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] GameObject staminabar;
    private Slider staminabarSlider;

    PlayerStats playerStats;

    // For animation
    //private float currentValue;

    //private bool healthbarMoveUp;
    //private float healthbarTransform;
    //private Vector3 healthbarStartPos;

    void Start()
    {
        playerStats = this.GetComponent<PlayerStats>();

        staminabarSlider = staminabar.GetComponent<Slider>();
        staminabarSlider.maxValue = playerStats.maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        staminabarSlider.value = playerStats.stamina;
    }
}
