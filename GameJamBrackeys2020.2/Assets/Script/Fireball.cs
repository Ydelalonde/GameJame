using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, ITriggerInTime
{
    bool hasSpawnChilds = false;
    bool goingForward = true;

    [SerializeField] GameObject Blurr = null;
    [SerializeField] float speedForChilds = 0f;
    [SerializeField] GameObject[] babyFireballs = null;


    private void Start()
    {
        for (int i = 0; i < babyFireballs.Length; ++i)
            babyFireballs[i].GetComponent<BabyFireball>().Speed = speedForChilds;

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>().changeRewindDelegate += OnChangeRewind;
    }

    private void Update()
    {
        if (!goingForward && hasSpawnChilds)
            Blurr.SetActive(true);
        else
            Blurr.SetActive(false);

    }

    public void TriggerInTime()
    {
        hasSpawnChilds = !hasSpawnChilds;

        for (int i = 0; i < babyFireballs.Length; ++i)
            babyFireballs[i].SetActive(hasSpawnChilds);
    }

    void OnChangeRewind(bool isRewind)
    {
        goingForward = !isRewind;
    }
}
