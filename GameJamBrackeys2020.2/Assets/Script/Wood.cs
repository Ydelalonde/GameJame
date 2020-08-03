using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{

    LDTimeline timelineLD = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireBall"))
            timelineLD.AddTemporaryObjectsToReactivate(gameObject);
    }

    void Start()
    {
        timelineLD = GameObject.FindGameObjectWithTag("GameManager").GetComponent<LDTimeline>();
    }
}
