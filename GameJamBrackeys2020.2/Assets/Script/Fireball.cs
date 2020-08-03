using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, ITriggerInTime
{
    [SerializeField] float speedForChilds = 0f;
    [SerializeField] GameObject[] babyFireballs = null;


    private void Start()
    {
        for (int i = 0; i < babyFireballs.Length; ++i)
            babyFireballs[i].GetComponent<BabyFireball>().Speed = speedForChilds;
    }


    public void TriggerInTime()
    {
        for (int i = 0; i < babyFireballs.Length; ++i)
            babyFireballs[i].SetActive(!babyFireballs[i].activeSelf);
    }
}
