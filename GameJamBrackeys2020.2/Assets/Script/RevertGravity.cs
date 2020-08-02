using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertGravity : MonoBehaviour, ITriggerInTime
{ 

    bool timeForward = true;
    [SerializeField] Rigidbody2D whatToRevert = null;


    public void TriggerInTime(bool isRewinding)
    {
        if (timeForward != isRewinding)
        {
            timeForward = !timeForward;
            whatToRevert.gravityScale *= -1;
        }
    }

}
