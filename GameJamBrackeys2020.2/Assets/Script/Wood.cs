using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{

    TimelinesManager timelinesManager = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireBall"))
            timelinesManager.AddTemporaryObjectsToReactivate(gameObject);
    }

    void Start()
    {
        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
    }
}
