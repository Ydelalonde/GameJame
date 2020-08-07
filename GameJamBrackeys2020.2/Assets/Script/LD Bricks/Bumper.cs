using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float jumpThrust = 0f;
    Rigidbody2D playerRb = null;

    Animator anim = null;
    AudioSource sound = null;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        sound = gameObject.GetComponent<AudioSource>();

        //GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>().changeRewindDelegate += OnChangeRewind;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (playerRb == null)
                playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpThrust);

            anim.SetTrigger("isTriggered");
            sound.Play();
        }
    }

    /*
    void OnChangeRewind(bool isRewind)
    {
        anim.SetBool("isReversed", isRewind);
    }*/
}
