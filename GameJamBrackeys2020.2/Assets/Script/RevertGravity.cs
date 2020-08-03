using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertGravity : MonoBehaviour, ITriggerInTime
{ 
    [SerializeField] Rigidbody2D whatToRevert = null;


    public void TriggerInTime()
    {
        whatToRevert.gravityScale *= -1;
    }

}
