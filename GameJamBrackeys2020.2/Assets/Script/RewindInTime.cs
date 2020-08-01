using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindInTime : MonoBehaviour
{
    [SerializeField]  BottomTimeline bottomTimelineScript = null;
    bool isRewinding = false;
    public bool IsRewinding
    {
        get => isRewinding;
    }

    float rewindedTime = 0f;
    [SerializeField] float maxRewindTime = 10f;
    List<Vector3> positions = new List<Vector3>();

    [SerializeField] SpriteRenderer spriteToChange = null;
    [SerializeField] Sprite spriteNormal = null;
    [SerializeField] Sprite spriteRewind = null;

    private void Start()
    {
        positions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRewinding)
            rewindedTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            StartRewind();
        else
            StopRewind();
    }

    private void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else if (bottomTimelineScript.CurrentState != BottomAction.E_FINNISH)
            Record();
    }

    void StartRewind()
    {
        spriteToChange.sprite = spriteRewind;
        isRewinding = true;
    }
    void StopRewind()
    {
        spriteToChange.sprite = spriteNormal;
        isRewinding = false;

        if(rewindedTime > 0)
        {
            bottomTimelineScript.SetStateAndTimeAfterRewind(rewindedTime);
            rewindedTime = 0f;
        }
    }

    void Rewind()
    {
        if (positions.Count > 0)
        {
            transform.position = positions[0];
            positions.RemoveAt(0);
        }
        else
            StopRewind();
    }
    void Record()
    {
        if (positions.Count > Mathf.Round(maxRewindTime / Time.fixedDeltaTime) )
            positions.RemoveAt(positions.Count - 1);
        
        positions.Insert(0, transform.position);
    }

}
