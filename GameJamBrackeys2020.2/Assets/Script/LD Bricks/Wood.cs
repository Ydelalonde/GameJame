using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] GameObject prefabParticleSystem = null;
    TimelinesManager timelinesManager = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireBall"))
        {
            Instantiate(prefabParticleSystem, transform.position, Quaternion.identity);
            timelinesManager.AddTemporaryObjectsToReactivate(gameObject);
        }
    }

    void Start()
    {
        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
    }
}
