using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTouch : MonoBehaviour
{
    PlayerTimeline playerTimeline = null;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerTimeline == null)
                playerTimeline = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerTimeline>();

           playerTimeline.CurrentState = BottomAction.E_FINNISH;
        }
    }
}
