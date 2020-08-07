using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopElement : MonoBehaviour, ITriggerInTime
{ 
    [SerializeField] GameObject[] whatToPop = null;
    AudioSource audioSource = null;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void TriggerInTime()
    {
        foreach(GameObject G in whatToPop)
        {
            audioSource.Play();
            G.SetActive(!G.activeSelf);
        }
    }

    public string GetName()
    {
        return "Magnet";
    }
}
