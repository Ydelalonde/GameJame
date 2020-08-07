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
    Animator anim = null;


    private void Start()
    {
        for (int i = 0; i < babyFireballs.Length; ++i)
            babyFireballs[i].GetComponent<BabyFireball>().Speed = speedForChilds;

        anim = gameObject.GetComponent<Animator>();
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>().changeLDRewindDelegate += OnChangeLDRewind;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>().changePlayerRewindDelegate += OnChangePlayerRewind;
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

        if (hasSpawnChilds)
            anim.SetTrigger("fire");

        for (int i = 0; i < babyFireballs.Length; ++i)
            babyFireballs[i].SetActive(hasSpawnChilds);
    }

    public string GetName()
    {
        return "Fireball";
    }

    void OnChangeLDRewind(bool Rewind)
    {
        goingForward = !goingForward;
    }

    void OnChangePlayerRewind(bool Rewind)
    {
        anim.SetBool("isPlayerRewinding", Rewind);
    }
}
