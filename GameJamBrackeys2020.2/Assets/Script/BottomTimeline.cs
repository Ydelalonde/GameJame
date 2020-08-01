using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BottomAction
{
    E_IDLE,
    E_RIGHT,
    E_FINNISH,
}

public class BottomTimeline : MonoBehaviour
{
    [SerializeField]  RewindInTime rewindInTimeScript = null;
    [SerializeField] private float speed = 0f;
    int numberOfTheState = 0;

    [SerializeField] private BottomAction[] states = null;
    [SerializeField] BottomAction currentState = BottomAction.E_IDLE;
    public BottomAction CurrentState
    {
        get => currentState;
    }

    [SerializeField] private float[] timeAtStates = null;
    float timeToWait = 0f;
    float waitedTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        SetStateAndTime();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(numberOfTheState);
        if (currentState == BottomAction.E_FINNISH || rewindInTimeScript.IsRewinding)
            return;

        if (waitedTime >= timeToWait)
        {
            numberOfTheState++;
            SetStateAndTime();
        }
        else
            waitedTime += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (rewindInTimeScript.IsRewinding)
            return;

        switch (currentState)
        {
            case BottomAction.E_RIGHT:
                transform.Translate(transform.right * speed * Time.deltaTime);
                break;
            default:
                break;
        }
    }


    void SetStateAndTime()
    {
        if (numberOfTheState == states.Length)
        {
            currentState = BottomAction.E_FINNISH;
            return;
        }

        currentState = states[numberOfTheState];
        timeToWait = timeAtStates[numberOfTheState];
        waitedTime = 0;
    }

    public void SetStateAndTimeAfterRewind(float rewindedTime)
    {

        if (rewindedTime - waitedTime <= 0)
        {
            waitedTime -= rewindedTime;
            return;
        }
        else
        {
            rewindedTime -= waitedTime;
            numberOfTheState--;
            currentState = states[numberOfTheState];
            timeToWait = timeAtStates[numberOfTheState];
            waitedTime = timeToWait;
            SetStateAndTimeAfterRewind(rewindedTime);
        }
    }
}
