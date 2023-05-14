using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPos : MonoBehaviour
{
    // This script is allow easier editing of hit/hurtboxes without
    // going in the model hierarchy as you need the model transforms
    // to reference from

    [SerializeField] GameObject leftArm;
    [SerializeField] GameObject leftArm_Hurtbox;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        leftArm_Hurtbox.transform.position = leftArm.transform.position;
    }
}
