using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestStartedGraphic : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] Sprite image;

    float lifetime = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lifetime > 0){

            lifetime -= Time.deltaTime;
            if (lifetime <= 0){
                Destroy(this);
            }
        }

        text.GetComponent<TextMeshProUGUI>().alpha += Time.deltaTime;
        
    }
}
