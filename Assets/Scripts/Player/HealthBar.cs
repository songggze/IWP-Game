using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] GameObject healthbar;
    private Slider healthbarSlider;

    PlayerStats playerStats;

    // For animation
    private float currentValue;

    private bool healthbarMoveUp;
    private float healthbarTransform;
    private Vector3 healthbarStartPos;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = this.GetComponent<PlayerStats>();

        healthbarSlider = healthbar.GetComponent<Slider>();
        healthbarSlider.maxValue = playerStats.health;
        currentValue = healthbarSlider.maxValue;

        healthbarMoveUp = true;
        healthbarTransform = 0;
        healthbarStartPos = healthbar.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentValue > playerStats.health)
        {
            currentValue -= 50 * Time.deltaTime;
            if (currentValue < playerStats.health)
            {
                currentValue = playerStats.health;
                healthbarTransform = 0;
            }
            else
            {
                // Handle health bar shaking animation
                if (healthbarMoveUp)
                {
                    healthbarTransform += 200 * Time.deltaTime;

                    if (healthbarTransform >= 5)
                    {
                        healthbarMoveUp = false;
                    }
                }
                else
                {
                    healthbarTransform -= 200 * Time.deltaTime;

                    if (healthbarTransform <= -5)
                    {
                        healthbarMoveUp = true;
                    }
                }
            }


            healthbar.transform.position = healthbarStartPos + (new Vector3 (0, healthbarTransform, 0));
        }
        else
        {
            currentValue += 50 * Time.deltaTime;
            if (currentValue > playerStats.GetComponent<PlayerStats>().health)
            {
                currentValue = playerStats.GetComponent<PlayerStats>().health;
            }
        }
        healthbarSlider.value = currentValue;

    }
}
