using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    [SerializeField] private float jumpThrust = 0f;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z))
            rb.velocity = new Vector2(rb.velocity.x, jumpThrust);

        if (Input.GetKey(KeyCode.D))
            transform.Translate(transform.right * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(-transform.right * speed * Time.deltaTime);
    }
}
