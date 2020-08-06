using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTouch : MonoBehaviour
{
    TimelinesManager timelinesManager = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("Player"))
        {
            if (timelinesManager == null)
                timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();

            timelinesManager.CurrentState = PlayerState.E_DEAD;
        }
        
    }
}
