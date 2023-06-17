using UnityEngine;
using TMPro;

public class PlayerDamageText : MonoBehaviour
{
    private double timer;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.5;
        pos = transform.position;
        pos.x += Random.Range(-50, 50);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0.4f)
        {
            pos.y += 440 * Time.deltaTime;
            transform.position = pos;
        }
        
        if (timer < 0.3f)
        {
            foreach (Transform child in transform){
                Debug.Log(child.GetComponent<TextMeshProUGUI>().alpha);
                child.GetComponent<TextMeshProUGUI>().alpha -= 1.5f * Time.deltaTime;
            }
        }

        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
