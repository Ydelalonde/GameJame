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



    public void TriggerInTime(bool isRewinding)
    {
        timeForward = !isRewinding;
    }

    void Start ()
    {
        GetDirection();
    }


    void Update()
    {
        Debug.Log(timeForward);

        if (!ReachDestination())
            transform.Translate(direction * speed * Time.deltaTime);
        else
        {
            //check limits
            if ( (!timeForward && currentDest == 0) || (timeForward && currentDest == wayPoints.Length - 1) )
            {
                Debug.Log("STUCK");
            }
            else
            { 
                if (waitedTime >= timeToWait)
                    GoToNextDestination();
                else
                    waitedTime += Time.deltaTime;
            }

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
