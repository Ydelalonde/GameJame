using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BabyFireball : MonoBehaviour
{

    [SerializeField] AudioClip SFX_Impact = null;
    [SerializeField] AudioClip SFX_Destruction = null;

    [SerializeField] AudioMixerGroup sound = null;
    [SerializeField] AudioMixerGroup toHigh = null;

    [SerializeField] GameObject prefabParticleSystem = null;

    AudioSource audioSource = null;

    bool goingForward = true;
    float speed = 0f;
    public float Speed
    {
        set => speed = value;
    }
    Animator anim = null;
    TimelinesManager timelinesManager = null;




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Wood>() != null)
        {
            audioSource.outputAudioMixerGroup = toHigh;
            audioSource.PlayOneShot(SFX_Destruction);
        }
        else
        {
            audioSource.outputAudioMixerGroup = sound;
            audioSource.PlayOneShot(SFX_Impact);
        }

        if (!collision.gameObject.CompareTag("Player") && goingForward)
        {
            Instantiate(prefabParticleSystem, transform.position, Quaternion.identity);
            timelinesManager.AddTemporaryObjectsToReactivate(gameObject);
        }
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponentInParent<AudioSource>();
        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
        timelinesManager.changeLDRewindDelegate += OnChangeLDRewind;
        timelinesManager.changePlayerRewindDelegate += OnChangePlayerRewind;
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

    void OnChangeLDRewind(bool Rewind)
    {
        goingForward = !goingForward;
    }

    void OnChangePlayerRewind(bool Rewind)
    {
        anim.SetBool("isPlayerRewinding", Rewind);
    }
}
