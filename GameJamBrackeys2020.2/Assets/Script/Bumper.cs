using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] private float jumpThrust = 0f;
    Rigidbody2D playerRb = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (playerRb == null)
                playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            playerRb.velocity += new Vector2(transform.up.x, transform.up.y) * jumpThrust;
        }
    }
}
