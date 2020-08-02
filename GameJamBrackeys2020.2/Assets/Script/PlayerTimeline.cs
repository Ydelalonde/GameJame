using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BottomAction
{
    E_IDLE,
    E_RIGHT,
    E_FINNISH,
}


public class PlayerTimeline : MonoBehaviour
{
    #region variables

    [SerializeField] Slider playerSlider = null;
    [SerializeField] Transform player = null;
    [SerializeField] float playerSpeed = 0f;
    BoxCollider2D playerBoxCollider = null;
    Rigidbody2D playerRb = null;

    int numberOfTheState = 0;
    [SerializeField] BottomAction[] states = null;
    BottomAction currentState = BottomAction.E_IDLE;
    public BottomAction CurrentState
    {
        set => currentState = value;
    }
    [SerializeField] float[] timeAtStates = null;
    float maximumLength = 0f;
    float timeToWait = 0f;
    float waitedTime = 0f;

    bool isRewinding = false;
    List<Vector3> positions = new List<Vector3>();
    #endregion


    void Start()
    {
        SetStateAndTime();

        playerBoxCollider = player.GetComponent<BoxCollider2D>();
        playerRb = player.GetComponent<Rigidbody2D>();

        for (int i = 0; i < timeAtStates.Length; ++i)
            maximumLength += timeAtStates[i];
    }


    void Update()
    {

        UpdateSlider();
        
        if (Input.GetKey(KeyCode.A))
            StartRewind();
        else
            StopRewind();

        if (currentState == BottomAction.E_FINNISH || isRewinding)
            return;

        if (waitedTime >= timeToWait)
        {
            numberOfTheState++;
            SetStateAndTime();
        }
        else
            waitedTime += Time.deltaTime;
    }

    void FixedUpdate()
    {
        //Rewind
        if (isRewinding)
        {
            SetStateAndTimeDuringRewind();

            if (positions.Count > 0)
            {
                player.position = positions[0];
                positions.RemoveAt(0);
            }
        }
        else
        {
            //Record
            if (currentState != BottomAction.E_FINNISH)
                positions.Insert(0, player.position);
            //E_Right
            if (currentState == BottomAction.E_RIGHT)
                player.Translate(player.right * playerSpeed * Time.deltaTime);
        }

    }

    void UpdateSlider()
    {
        float temp = waitedTime;

        for (int i = 0; i < numberOfTheState; ++i)
            temp += timeAtStates[i];

        playerSlider.value = temp / maximumLength;
    }

    void SetStateAndTime()
    {
        if (numberOfTheState == states.Length)
        {
            currentState = BottomAction.E_FINNISH;
            numberOfTheState--;
            return;
        }

        waitedTime = Mathf.Max(0, waitedTime - timeToWait);
        currentState = states[numberOfTheState];
        timeToWait = timeAtStates[numberOfTheState];
    }

    void SetStateAndTimeDuringRewind()
    {
        Debug.Log(numberOfTheState);
        if (Time.fixedDeltaTime <= waitedTime)
        {
            waitedTime -= Time.fixedDeltaTime;
            currentState = states[numberOfTheState];
            timeToWait = timeAtStates[numberOfTheState];
            return;
        }
        else if (numberOfTheState == 0)
        {
            waitedTime = 0;
            return;
        }
        else
        {
            numberOfTheState--;
            currentState = states[numberOfTheState];
            timeToWait = timeAtStates[numberOfTheState];

            waitedTime = waitedTime - Time.fixedDeltaTime + timeToWait;

            //rewindedTime -= waitedTime;
            //waitedTime = timeToWait;
            //SetStateAndTimeDuringRewind(rewindedTime);
        }
    }


    void StartRewind()
    {
        isRewinding = true;
        playerBoxCollider.isTrigger = true;
        playerRb.isKinematic = true;
    }
    void StopRewind()
    {
        isRewinding = false;
        playerBoxCollider.isTrigger = false;
        playerRb.isKinematic = false;
    }
}
