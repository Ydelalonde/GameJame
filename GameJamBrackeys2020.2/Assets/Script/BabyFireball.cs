using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyFireball : MonoBehaviour
{
    bool goingForward = true;
    float speed = 0f;
    public float Speed
    {
        set => speed = value;
    }
    TimelinesManager timelinesManager = null;
    float rewindScale = 0f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        timelinesManager.AddTemporaryObjectsToReactivate(gameObject);
    }

    void Start()
    {
        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
        rewindScale = timelinesManager.LDRewindScale;
        timelinesManager.changeRewindDelegate += OnChangeRewind;
    }

    void Update()
    {
        Vector3 translation = Vector3.up * speed * Time.deltaTime;

        if (!goingForward)
            translation *= -rewindScale;

        transform.Translate(translation);
    }

    void OnChangeRewind(bool isRewind)
    {
        goingForward = !isRewind;
    }
}
