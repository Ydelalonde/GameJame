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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && goingForward)
            timelinesManager.AddTemporaryObjectsToReactivate(gameObject);
    }

    void Start()
    {
        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
        timelinesManager.changeRewindDelegate += OnChangeRewind;
    }

    void Update()
    {
        if (timelinesManager.PlayerIsRewinding)
            return;

        Vector3 translation = Vector3.up * speed * Time.deltaTime;

        if (!goingForward)
            translation *= -1;

        transform.Translate(translation);
    }

    void OnChangeRewind(bool isRewind)
    {
        goingForward = !isRewind;
    }
}
