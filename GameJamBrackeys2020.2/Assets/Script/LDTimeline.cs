using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LDTimeline : MonoBehaviour
{
    [SerializeField] Slider LDSlider = null;
    [SerializeField] float lengthOfTimeline = 0;
    float timeOnTheTimeline = 0f;
    [SerializeField] float timeScale = 0f;
    float deltaTime = 0f;
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
            Debug.Log(timeForTrigger[i] + triggers[i].AdditionnalTime());
        }
            

    }

    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.deltaTime * timeScale;

        if (Input.GetKey(KeyCode.E))
            isRewinding = true;
        else
            isRewinding = false;

        UpdateTimeline();
        UpdateSlider();
        UpdateTriggers();
    }


    void UpdateTimeline()
    {
        timeOnTheTimeline += (isRewinding)? -deltaTime : deltaTime;
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
