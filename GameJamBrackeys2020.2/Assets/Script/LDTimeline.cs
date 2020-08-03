using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LDTimeline : MonoBehaviour
{
    public delegate void OnChangeRewind(bool value);
    public event OnChangeRewind changeRewindDelegate;

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
    public bool IsRewinding
    {
        set
        {
            if(isRewinding != value)
            {
                isRewinding = value;
                changeRewindDelegate?.Invoke(isRewinding);
            }
        }
    }

    [SerializeField] Transform[] objectToTrigger = null;
    List<ITriggerInTime> triggers = new List<ITriggerInTime>();
    [SerializeField] float[] timeForTrigger = null;
    int numberOfObjectToTriggerPassed = 0;


    List<GameObject> temporaryObjectsToReactivate = new List<GameObject>();
    List<float> timeForObjectsToReactivate = new List<float>();


    private void Start()
    {
        //fill triggers
        for (int i = 0; i < objectToTrigger.Length; ++i)
            triggers.Add(objectToTrigger[i].GetComponent<ITriggerInTime>());
    }

    // Update is called once per frame
    void Update()
    {
        remainingCoolDownForRewind -= Time.deltaTime;

        if (Input.GetKey(KeyCode.E) && remainingCoolDownForRewind < 0)
            IsRewinding = true;
        else if (isRewinding)
        { 
            remainingCoolDownForRewind = coolDownForRewind;
            IsRewinding = false;
        }

        UpdateTimeline();
        UpdateSlider();
        UpdateTriggers();
        if (isRewinding)
            UpdateObjectsToReactivate();
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
        if(!isRewinding)
        {
            for (int i = numberOfObjectToTriggerPassed; i < triggers.Count; ++i)
            {
                if (timeOnTheTimeline > timeForTrigger[i])
                {
                    triggers[i].TriggerInTime();
                    numberOfObjectToTriggerPassed++;
                }
                else
                    return;
            }
        }
        else
        {
            for (int i = numberOfObjectToTriggerPassed; i > 0; --i)
            {
                if (timeOnTheTimeline < timeForTrigger[i - 1])
                {
                    triggers[i - 1].TriggerInTime();
                    numberOfObjectToTriggerPassed--;
                }
                else
                    return;
            }
        }
             
    }

    void UpdateObjectsToReactivate()
    {
        for (int i = 0; i < temporaryObjectsToReactivate.Count; ++i)
            if (timeOnTheTimeline < timeForObjectsToReactivate[i])
            {
                temporaryObjectsToReactivate[i].SetActive(true);

                //delete the object and the timer from the lists
                temporaryObjectsToReactivate.RemoveAt(i);
                timeForObjectsToReactivate.RemoveAt(i);
            }
    }

    public void AddTemporaryObjectsToReactivate(GameObject objectToAdd)
    {
        temporaryObjectsToReactivate.Add(objectToAdd);
        timeForObjectsToReactivate.Add(timeOnTheTimeline);
        objectToAdd.SetActive(false);
    }
}
