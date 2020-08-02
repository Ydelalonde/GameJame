using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertGravity : MonoBehaviour, ITriggerInTime
{ 

    bool timeForward = true;
    [SerializeField] Rigidbody2D whatToRevert = null;

    public float AdditionnalTime()
    {
        return 0;
    }

    public void TriggerInTime(bool isRewinding)
    {
        if (timeForward != isRewinding)
        {
            timeForward = !timeForward;
            whatToRevert.gravityScale *= -1;
        }
    }

}
