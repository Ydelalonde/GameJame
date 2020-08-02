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

            Vector3 forward = transform.up;
            playerRb.velocity = new Vector2(forward.x, forward.y) * jumpThrust;
        }
    }
}
