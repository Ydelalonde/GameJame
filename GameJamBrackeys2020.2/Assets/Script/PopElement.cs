using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopElement : MonoBehaviour, ITriggerInTime
{ 
    [SerializeField] GameObject[] whatToPop = null;

    public void TriggerInTime()
    {
        foreach(GameObject G in whatToPop)
            G.SetActive(!G.activeSelf);
    }

    public string GetName()
    {
        return "Magnet";
    }
}
