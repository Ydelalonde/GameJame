using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectIfPlayerCanStopRewind : MonoBehaviour
{

    PlayerTimeline timelinePlayer = null;

    private void Start()
    {
        timelinePlayer = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerTimeline>();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        timelinePlayer.CanStopRewind = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Ground")
            timelinePlayer.CanStopRewind = false;
    }

}
