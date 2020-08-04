using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertGravity : MonoBehaviour
{ 
    Rigidbody2D rbToRevert = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (rbToRevert == null)
                rbToRevert = collision.gameObject.GetComponent<Rigidbody2D>();

            rbToRevert.gravityScale *= -1;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (rbToRevert == null)
                rbToRevert = collision.gameObject.GetComponent<Rigidbody2D>();

            rbToRevert.gravityScale *= -1;
        }
    }
}
