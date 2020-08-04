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
            collision.transform.localScale = new Vector3(collision.transform.localScale.x, -collision.transform.localScale.y, collision.transform.localScale.z);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rbToRevert.gravityScale *= -1;
            collision.transform.localScale = new Vector3(collision.transform.localScale.x, -collision.transform.localScale.y, collision.transform.localScale.z);
        }
    }
}
