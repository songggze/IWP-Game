using UnityEngine;

public class AutoCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position += Camera.main.transform.right;
    }
}
