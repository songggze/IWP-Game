using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float maxStamina = 100;

    private float health;
    private float stamina;

    void Start()
    {
        health = maxHealth;
        stamina = maxStamina;
    }

    void Update()
    {
        
    }
}
