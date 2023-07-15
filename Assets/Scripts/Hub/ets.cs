using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ets : MonoBehaviour
{
    public float delay = 0;
    public float setDelay = 0.15f;

    // Update is called once per frame
    void Update()
    {

        if (delay <= 0){
            if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.K))
            {
                Debug.Log("Both");
                delay = setDelay;
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                Debug.Log("l");
                delay = setDelay;
            }
            else if (Input.GetKeyUp(KeyCode.K))
            {

                Debug.Log("r");
                delay = setDelay;
            }
        }
        else{
            delay -= Time.deltaTime;
        }
    }
}
