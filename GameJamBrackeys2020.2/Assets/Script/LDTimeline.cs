using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LDTimeline : MonoBehaviour
{
    [SerializeField] Slider LDSlider = null;
    [SerializeField] float lengthOfTimeline = 0;
    float timeOnTheTimeline = 0f;
    bool isRewinding = false;

    [SerializeField] Transform[] objectToTrigger = null;
    List<ITriggerInTime> triggers = new List<ITriggerInTime>();
    [SerializeField] float[] timeForTrigger = null;

    [SerializeField] Transform[] platformToTrigger = null;
    List<ITriggerInTime> platforms = new List<ITriggerInTime>();
    [SerializeField] float[] timeForPlatforms = null;


    private void Start()
    {
        for(int i = 0; i < objectToTrigger.Length; ++i)
            if(objectToTrigger[i].GetComponent<ITriggerInTime>() != null)
                triggers.Add(objectToTrigger[i].GetComponent<ITriggerInTime>());

        for (int i = 0; i < platformToTrigger.Length; ++i)
            if (platformToTrigger[i].GetComponent<ITriggerInTime>() != null)
                platforms.Add(platformToTrigger[i].GetComponent<ITriggerInTime>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
            isRewinding = true;
        else
            isRewinding = false;

        UpdateTimeline();
        UpdateSlider();
        UpdateTriggers();
        UpdatePlatforms();
    }


    void UpdateTimeline()
    {
        timeOnTheTimeline += (isRewinding)? -Time.deltaTime : Time.deltaTime;
        timeOnTheTimeline = Mathf.Clamp(timeOnTheTimeline, 0, lengthOfTimeline);
    }


    void UpdateSlider()
    {
        LDSlider.value = timeOnTheTimeline / lengthOfTimeline;
    }

    void UpdateTriggers()
    {
        for (int i = 0; i < triggers.Count; ++i)
            if (timeForTrigger[i] - Time.deltaTime < timeOnTheTimeline &&  timeOnTheTimeline < timeForTrigger[i] + Time.deltaTime)
                triggers[i].TriggerInTime(isRewinding);
    }

    void UpdatePlatforms()
    {
        for (int i = 0; i < platforms.Count; ++i)
            if (timeOnTheTimeline > timeForPlatforms[i])
                platforms[i].TriggerInTime(isRewinding);
    }

}
