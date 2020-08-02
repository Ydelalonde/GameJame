using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevertGravity : MonoBehaviour
{ 
    bool active = false;

    [SerializeField] Rigidbody2D whatToRevert = null;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            active = !active;

            if (active)
                whatToRevert.gravityScale = -1;
            else
                whatToRevert.gravityScale = 1;
        }

    }
}
