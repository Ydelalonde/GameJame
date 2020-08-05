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

    void Start ()
    {
        if (startingMode)
            isActive = true;

        TimelinesManager timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
        timelinesManager.changeRewindDelegate += OnChangeRewind;

        GetDirection();
    }

    void Update()
    {
        if (!isActive)
            return;
        
        if (!ReachDestination())
        {
            Vector2 translation = direction * speed * Time.deltaTime;
            transform.Translate(translation);
        }
        else
        {
            if (waitedTime >= timeToWait)
            {
                //check limits
                if ((!goingForward && currentDest > 0) || (goingForward && currentDest < wayPoints.Length - 1))
                    GoToNextDestination();
            }
            else
                waitedTime += (goingForward)? Time.deltaTime : Time.deltaTime;
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

    void OnChangeRewind(bool isRewind)
    {
        goingForward = !goingForward;


        if (!isActive)
            return;

        Blurr.SetActive(!goingForward);

        //if in-Between
        if (!ReachDestination())
            GoToNextDestination();

        //to wait the same time as already waited before going back
        if (!goingForward)
            timeToWait = waitedTime * 2;
        

    }
}
