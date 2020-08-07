using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyFireball : MonoBehaviour
{

    [SerializeField] AudioClip SFX_Impact = null;
    [SerializeField] AudioClip SFX_Destruction = null;
    
    AudioSource audioSource = null;

    bool goingForward = true;
    float speed = 0f;
    public float Speed
    {
        set => speed = value;
    }
    TimelinesManager timelinesManager = null;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Wood>() != null)
            audioSource.PlayOneShot(SFX_Destruction);
        else
            audioSource.PlayOneShot(SFX_Impact);

        if (!collision.gameObject.CompareTag("Player") && goingForward)
            timelinesManager.AddTemporaryObjectsToReactivate(gameObject);
    }

    void Start()
    {
        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
        timelinesManager.changeRewindDelegate += OnChangeRewind;
        audioSource = gameObject.GetComponentInParent<AudioSource>();
    }

    void Update()
    {
        if (timelinesManager.PlayerIsRewinding)
            return;

        Vector3 translation = Vector3.up * speed * Time.deltaTime;

        if (!goingForward)
            translation *= -1;

        transform.Translate(translation);
    }

    void OnChangeRewind(bool isRewind)
    {
        goingForward = !isRewind;
    }
}
