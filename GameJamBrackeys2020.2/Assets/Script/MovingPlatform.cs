using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour,ITriggerInTime
{

    [SerializeField] float speed = 2.0f;
    [SerializeField] private Transform[] wayPoints = null;
    [SerializeField] float[] timeAtWayPoints = null;

    bool timeForward = false; 
    int currentDest = 0;
    Vector2 direction;
    float waitedTime = 0;
    float timeToWait = 0;

    public float AdditionnalTime()
    {
        float time = 0f;
        foreach (float F in timeAtWayPoints)
            time += F;

        float distance = 0f;
        for (int i = 0; i < wayPoints.Length - 1; ++i)
            distance += (wayPoints[i + 1].position - wayPoints[i].position).magnitude;

        //waited time + translation time
        return time + distance / speed; 
    }

    public void TriggerInTime(bool isRewinding)
    {
        if (timeForward == isRewinding)
        {
            timeForward = !isRewinding;

            //if in-Between
            if (waitedTime == 0)
                GoToNextDestination();

            if (!timeForward)
                timeToWait = waitedTime * 2;
        }
    }

    void Start ()
    {
        GetDirection();
    }


    void Update()
    {
        if (!ReachDestination())
            transform.Translate(direction * speed * Time.deltaTime);
        else
        {
            if (waitedTime >= timeToWait)
            {
                //check limits
                if ((!timeForward && currentDest > 0) || (timeForward && currentDest < wayPoints.Length - 1))
                    GoToNextDestination();
            }
            else
                waitedTime += Time.deltaTime;
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
        currentDest = timeForward ? currentDest += 1 : currentDest -= 1;
        timeToWait = timeAtWayPoints[currentDest];
        GetDirection();
    }

}
