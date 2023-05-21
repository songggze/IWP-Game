using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float maxStamina = 100;

    [SerializeField] float faintsLife = 100;

    public float health;
    public float stamina;
    [SerializeField] float iFrames = 1;
    public float iFrameTimer;

    public bool isHit;

    void Start()
    {
        health = maxHealth;
        stamina = maxStamina;

        iFrameTimer = 0;
        isHit = false;
    }

    void Update()
    {
        
        if (iFrameTimer > 0){
            iFrameTimer -= Time.deltaTime;
        }
        else{
            isHit = false;
        }
    }

    public void SetIFrames()
    {
        iFrameTimer = iFrames;
    }
}
