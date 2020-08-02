using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopElement : MonoBehaviour, ITriggerInTime
{ 

    bool timeForward = true;
    [SerializeField] GameObject whatToPop = null;


    public void TriggerInTime(bool isRewinding)
    {
        if (timeForward != isRewinding)
        {
            timeForward = !timeForward;
            whatToPop.SetActive(!whatToPop.activeSelf);
        }
        
    }

}
