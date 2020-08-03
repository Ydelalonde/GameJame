using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopElement : MonoBehaviour, ITriggerInTime
{ 
    [SerializeField] GameObject whatToPop = null;

    public void TriggerInTime()
    {
        whatToPop.SetActive(!whatToPop.activeSelf);
    }

}
