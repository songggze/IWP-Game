using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviewScript : MonoBehaviour
{
    public float previewTime = 20;


    // Update is called once per frame
    void Update()
    {
        previewTime -= Time.deltaTime;
        if (previewTime <= 0){
            SceneManager.LoadScene("Forest");
        }

        if (Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene("Forest");
        }
    }
}
