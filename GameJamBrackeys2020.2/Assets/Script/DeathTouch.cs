using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTouch : MonoBehaviour
{
    TimelinesManager timelinesManager = null;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timelinesManager == null)
                timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();

            timelinesManager.CurrentState = PlayerState.E_FINNISH;
        }
    }
}
