using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopElement : MonoBehaviour
{
    bool active = true;

    [SerializeField] GameObject whatToPop = null;

    // Update is called once per frame
    void Update()
    {
        whatToPop.SetActive(active);

        if (Input.GetKeyDown(KeyCode.W))
            active = !active;
    }
}
