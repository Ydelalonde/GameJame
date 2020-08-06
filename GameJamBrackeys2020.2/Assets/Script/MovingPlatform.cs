using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour,ITriggerInTime
{
    [SerializeField] bool startingMode = false;
    [SerializeField] float speed = 2.0f;
    [SerializeField] GameObject Blurr = null;
    [SerializeField] Transform[] wayPoints = null;
    [SerializeField] float[] timeAtWayPoints = null;
    TimelinesManager timelinesManager = null;

    bool isActive = false;
    bool goingForward = true;

    int currentDest = 0;
    Vector2 direction;
    float waitedTime = 0;
    float timeToWait = 0;


    public void TriggerInTime()
    {
        isActive = !isActive;
        if (!isActive)
            Blurr.SetActive(false);
    }

    public string GetName()
    {
        return "MovingPlatform";
    }

    void OnChangeRewind(bool isRewind)
    {
        goingForward = !isRewind;


        if (!isActive)
            return;

        Blurr.SetActive(!goingForward);

        //if in-Between
        if (!ReachDestination())
            GoToNextDestination();
        //to wait the same time as already waited before going back
        else if (!goingForward)
            timeToWait = waitedTime * 2;
        else if (currentDest == wayPoints.Length - 1) //if At End
            waitedTime = timeToWait - waitedTime;
    }

    void Start ()
    {
        if (startingMode)
            isActive = true;

        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
        timelinesManager.changeRewindDelegate += OnChangeRewind;

        GetDirection();
    }

    void Update()
    {
        if (!isActive || timelinesManager.PlayerIsRewinding)
            return;
        
        if (!ReachDestination())
            transform.Translate(direction * speed * Time.deltaTime);
        else if(timelinesManager.LDTimeOnTheTimeline < timelinesManager.LDLengthOfTimeline)
        {
            //if At End
            if (goingForward && currentDest == wayPoints.Length - 1)
                timeToWait += Time.deltaTime;

            if (waitedTime >= timeToWait)
            {
                //check limits
                if ((!goingForward && currentDest > 0) || (goingForward && currentDest < wayPoints.Length - 1))
                    GoToNextDestination();
            }
            else
                waitedTime +=  Time.deltaTime;
        }

    }

    private void GetDirection()
    {
        direction = wayPoints[currentDest].position - transform.position;
        direction.Normalize();
    }

    private bool ReachDestination()
    {
        return ((wayPoints[currentDest].position - transform.position).magnitude < 0.1) ? true : false;
    }

    private void GoToNextDestination()
    {
        waitedTime = 0;
        currentDest = goingForward ? currentDest += 1 : currentDest -= 1;
        timeToWait = timeAtWayPoints[currentDest];
        GetDirection();
    }


}
