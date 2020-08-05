using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertGravity : MonoBehaviour
{
    Transform playerTransform = null;
    Rigidbody2D playerRb = null;

    bool gameObjectEnabled = true;
    public bool GameObjectEnabled
    {
        set
        {
            if (gameObjectEnabled != value)
            {
                gameObjectEnabled = value;
                if (!gameObjectEnabled)
                    RevertPlayer();
            }
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (playerTransform == null)
            {
                playerTransform = collision.transform;
                playerRb = playerTransform.GetComponent<Rigidbody2D>();
            }

            RevertPlayer();
        }    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            RevertPlayer();
    }
    private void Update()
    {
        GameObjectEnabled = gameObject.activeSelf;
    }

    void RevertPlayer()
    {
        playerRb.gravityScale *= -1;
        playerTransform.localScale = new Vector3(playerTransform.localScale.x, -playerTransform.localScale.y, playerTransform.localScale.z);
    }

}
