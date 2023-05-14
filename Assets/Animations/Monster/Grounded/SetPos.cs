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

    [SerializeField] GameObject rightArm;
    [SerializeField] GameObject rightArm_Hurtbox;

    [SerializeField] GameObject leftLeg;
    [SerializeField] GameObject leftLeg_Hurtbox;

    [SerializeField] GameObject rightLeg;
    [SerializeField] GameObject rightLeg_Hurtbox;

    [SerializeField] GameObject body;
    [SerializeField] GameObject body_Hurtbox;

    [SerializeField] GameObject head;
    [SerializeField] GameObject head_Hurtbox;

    [SerializeField] GameObject tail;
    [SerializeField] GameObject tail_Hurtbox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        leftArm_Hurtbox.transform.position = leftArm.transform.position;
        rightArm_Hurtbox.transform.position = rightArm.transform.position;

        leftLeg_Hurtbox.transform.position = leftLeg.transform.position;
        rightLeg_Hurtbox.transform.position = rightLeg.transform.position;

        body_Hurtbox.transform.position = body.transform.position;
        tail_Hurtbox.transform.position = tail.transform.position;
        head_Hurtbox.transform.position = head.transform.position;
    }

}    