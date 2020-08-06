using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertGravity : MonoBehaviour
{
    List<Collider2D> triggersList = new List<Collider2D>();

    Transform playerTransform = null;
    Rigidbody2D playerRb = null;

    bool gameObjectEnabled = true;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the object is not already in the list
        if (!triggersList.Contains(collision))
            triggersList.Add(collision);

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
        //if the object is in the list
        if (triggersList.Contains(collision))
            triggersList.Remove(collision);

        if (collision.CompareTag("Player"))
            RevertPlayer();
    }
    /*
    private void Update()
    {
        foreach (Collider2D C in triggersList)
            Debug.Log(C.gameObject.name);


        //change bool only on a state change
        if (gameObjectEnabled != gameObject.activeSelf)
        {
            gameObjectEnabled = gameObject.activeSelf;

            //if get desactivate, check if there was a player
            if (!gameObjectEnabled)
            {
                foreach (Collider2D C in triggersList)
                    if (C.gameObject.CompareTag("Player"))
                        RevertPlayer();

                triggersList.Clear();

            }
        }
    }
    */
    void RevertPlayer()
    {
        playerRb.gravityScale *= -1;
        playerTransform.localScale = new Vector3(playerTransform.localScale.x, -playerTransform.localScale.y, playerTransform.localScale.z);
    
        Debug.Log(playerRb.gravityScale);
    }


 
}
