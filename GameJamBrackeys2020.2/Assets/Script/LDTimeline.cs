using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LDTimeline : MonoBehaviour
{
    [SerializeField] Slider LDSlider = null;
    [SerializeField] float lengthOfTimeline = 0;
    [SerializeField] float timeOnTheTimeline = 0f; /////////////////////////////////
    [SerializeField] float rewindScale = 0f;
    public float RewindScale
    {
        get => rewindScale;
    }
    [SerializeField] float coolDownForRewind = 0f;
    float remainingCoolDownForRewind = 0f;
    bool isRewinding = false;

    [SerializeField] Transform[] objectToTrigger = null;
    List<ITriggerInTime> triggers = new List<ITriggerInTime>();
    [SerializeField] float[] timeForTrigger = null;
    List<float> timeForUnTrigger = new List<float>();


    private void Start()
    {
        //fill triggers
        for (int i = 0; i < objectToTrigger.Length; ++i)
        {
            triggers.Add(objectToTrigger[i].GetComponent<ITriggerInTime>());
            timeForUnTrigger.Add(timeForTrigger[i] + triggers[i].AdditionnalTime());
        }
    }

    // Update is called once per frame
    void Update()
    {
        remainingCoolDownForRewind -= Time.deltaTime;

        if (Input.GetKey(KeyCode.E) && remainingCoolDownForRewind < 0)
            isRewinding = true;
        else if (isRewinding)
        { 
            remainingCoolDownForRewind = coolDownForRewind;
            isRewinding = false;
        }

        UpdateTimeline();
        UpdateSlider();
        UpdateTriggers();
    }


    void UpdateTimeline()
    {
        timeOnTheTimeline += (isRewinding) ? -Time.deltaTime * rewindScale : Time.deltaTime;
        timeOnTheTimeline = Mathf.Clamp(timeOnTheTimeline, 0, lengthOfTimeline);
    }


    void UpdateSlider()
    {
        LDSlider.value = timeOnTheTimeline / lengthOfTimeline;
    }

    void UpdateTriggers()
    {
        for (int i = 0; i < triggers.Count; ++i)
            if ( (!isRewinding && timeOnTheTimeline > timeForTrigger[i]) || (isRewinding && timeOnTheTimeline < timeForUnTrigger[i]) )
                triggers[i].TriggerInTime(isRewinding);               
    }

}
