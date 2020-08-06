using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertGravity : MonoBehaviour
{
    TimelinesManager timelinesManager = null;
    Transform playerTransform = null;
    Rigidbody2D playerRb = null;


    private void Start()
    {
        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerTransform == null)
            {
                playerTransform = collision.transform;
                playerRb = playerTransform.GetComponent<Rigidbody2D>();
            }

            if (!timelinesManager.LDIsRewinding)
                playerRb.gravityScale = -1;
            playerTransform.localScale = new Vector3(1, -1, 1);
        }    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerRb.gravityScale = 1;
            playerTransform.localScale = new Vector3(1, 1, 1);
        }
    }


}
