using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    public int gold = 200;

    // Start is called before the first frame update
    void Start()
    {
        
        // Singleton
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        }
        else{
            Destroy(this);
        }
    }

}
