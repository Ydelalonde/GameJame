using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    E_IDLE,
    E_RIGHT,
    E_FINNISH,
}
public class TimelinesManager : MonoBehaviour
{
    #region Post Process
    [Header("Post Process")]
    [SerializeField] GameObject postProcessStandard = null;
    [SerializeField] GameObject postProcessPL = null;
    [SerializeField] GameObject postProcessLD = null;
    #endregion

    #region Player
    [Header("Player Timeline")] 
    [SerializeField] Slider playerSlider = null;
    [SerializeField] float playerLengthOfTimeline = 0;
    [SerializeField] float playerTimeOnTheTimeline = 0f;/////////////////////////////////
    [SerializeField] float playerRewindScale = 0f;
    [SerializeField] float playerCoolDownForRewind = 0f;
    float playerRemainingCoolDownForRewind = 0f;
    bool playerIsRewinding = false;
    public bool PlayerIsRewinding
    {
        set
        {
            playerIsRewinding = value;
            playerBoxCollider.isTrigger = value;

            Time.timeScale = (playerIsRewinding) ? 0 : 1;
            Blurr.SetActive(playerIsRewinding);

            postProcessPL.SetActive(playerIsRewinding);
            postProcessStandard.SetActive(!playerIsRewinding);


            if (currentState == PlayerState.E_FINNISH)
                CurrentState = (playerNumberTriggersPassed > 1) ? playerTriggers[playerNumberTriggersPassed - 1] : PlayerState.E_RIGHT;
        }
    }

    [SerializeField] PlayerState currentState = PlayerState.E_RIGHT; /////////////////////////////////
    public PlayerState CurrentState
    {
        set
        {
            if (currentState == PlayerState.E_FINNISH)
                playerRb.isKinematic = false;

            currentState = value;
            if (currentState == PlayerState.E_FINNISH)
            {
                playerRemainingCoolDownForRewind = 0;
                Time.timeScale = 0;
                playerRb.isKinematic = true;
            }
        }
    }
    [SerializeField] Transform player = null;
    [SerializeField] GameObject Blurr = null;
    [SerializeField] float playerSpeed = 0f;
    BoxCollider2D playerBoxCollider = null;
    Rigidbody2D playerRb = null;
    List<Vector3> positions = new List<Vector3>();

    [SerializeField] PlayerState[] playerTriggers = null;
    [SerializeField] float[] playerTimeForTriggers = null;
    int playerNumberTriggersPassed = 0;



    #endregion

    #region LD
    [Header("LD Timeline")]
    [SerializeField] Slider lDSlider = null;
    [SerializeField] float lDLengthOfTimeline = 0;
    [SerializeField] float lDTimeOnTheTimeline = 0f; /////////////////////////////////
    [SerializeField] float lDRewindScale = 0f;
    public float LDRewindScale
    {
        get => lDRewindScale;
    }
    [SerializeField] float lDCoolDownForRewind = 0f;
    float lDRemainingCoolDownForRewind = 0f;
    bool lDIsRewinding = false;
    public bool LDIsRewinding
    {
        set
        {
            if (lDIsRewinding != value)
            {
                lDIsRewinding = value;

                postProcessLD.SetActive(lDIsRewinding);
                if(lDIsRewinding)
                    postProcessStandard.SetActive(false);
                else if (!playerIsRewinding)
                    postProcessStandard.SetActive(true);
                
                changeRewindDelegate?.Invoke(lDIsRewinding);
            }
        }
    }

    [SerializeField] Transform[] lDObjectsToTrigger = null;
    List<ITriggerInTime> lDTriggers = new List<ITriggerInTime>();
    [SerializeField] float[] lDTimeForTriggers = null;
    int lDNumberTriggersPassed = 0;
    List<GameObject> lDTemporaryObjectsToReactivate = new List<GameObject>();
    List<float> lDTimeForObjectsToReactivate = new List<float>();

    public delegate void OnChangeRewind(bool value);
    public event OnChangeRewind changeRewindDelegate;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Player
        playerBoxCollider = player.GetComponent<BoxCollider2D>();
        playerRb = player.GetComponent<Rigidbody2D>();

        //LD
        for (int i = 0; i < lDObjectsToTrigger.Length; ++i)
            lDTriggers.Add(lDObjectsToTrigger[i].GetComponent<ITriggerInTime>());

    }


    void Update()
    {
        playerRemainingCoolDownForRewind -= Time.deltaTime;
        lDRemainingCoolDownForRewind -= Time.deltaTime;

        CheckInput();
        UpdateTimeline();
        UpdateSliders();

        UpdateTriggersPlayer();
        UpdateTriggersLD();

        UpdatePlayer();
        UpdateObjectsToReactivate();
    }

    void CheckInput()
    {
        //Player
        if (Input.GetKey(KeyCode.A) && playerRemainingCoolDownForRewind <= 0)
            PlayerIsRewinding = true;
        else if (playerIsRewinding)
        {
            playerRemainingCoolDownForRewind = playerCoolDownForRewind;
            PlayerIsRewinding = false;
        }

        //LD
        if (Input.GetKey(KeyCode.E) && lDRemainingCoolDownForRewind < 0 && !playerIsRewinding)
            LDIsRewinding = true;
        else if (lDIsRewinding)
        {
            lDRemainingCoolDownForRewind = lDCoolDownForRewind;
            LDIsRewinding = false;
        }

    }

    void UpdateTimeline()
    {
        //Player
        if(!lDIsRewinding)
        {        
            if(Time.timeScale == 1)
                playerTimeOnTheTimeline += (playerIsRewinding) ? -Time.deltaTime * playerRewindScale : Time.deltaTime;
            else if(currentState != PlayerState.E_FINNISH)
                playerTimeOnTheTimeline += (playerIsRewinding) ? -Time.unscaledDeltaTime * playerRewindScale : Time.unscaledDeltaTime;

            playerTimeOnTheTimeline = Mathf.Clamp(playerTimeOnTheTimeline, 0, playerLengthOfTimeline);
        }


        //LD
        lDTimeOnTheTimeline += (lDIsRewinding) ? -Time.deltaTime * LDRewindScale : Time.deltaTime;
        lDTimeOnTheTimeline = Mathf.Clamp(lDTimeOnTheTimeline, 0, lDLengthOfTimeline);
    }

    void UpdateSliders()
    {
        //Player
        playerSlider.value = playerTimeOnTheTimeline / playerLengthOfTimeline;
        //LD
        lDSlider.value = lDTimeOnTheTimeline / lDLengthOfTimeline;
    }

    void UpdateTriggersPlayer()
    {

        //Going Forward
        if (!playerIsRewinding)
        {
            for (int i = playerNumberTriggersPassed; i < playerTriggers.Length; ++i)
            {
                if (playerTimeOnTheTimeline > playerTimeForTriggers[i])
                {
                    currentState = playerTriggers[i];
                    playerNumberTriggersPassed++;
                }
                else
                    return;
            }
        }
        //Going Backward
        else
        {
            for (int i = playerNumberTriggersPassed; i > 0; --i)
            {
                if (playerTimeOnTheTimeline < playerTimeForTriggers[i - 1])
                {
                    playerNumberTriggersPassed--;
                    CurrentState = (playerNumberTriggersPassed > 0) ? playerTriggers[playerNumberTriggersPassed - 1] : PlayerState.E_RIGHT;
                }
                else
                    return;
            }
        }
    }

    void UpdateTriggersLD()
    {
        //Going Forward
        if (!lDIsRewinding)
        {
            for (int i = lDNumberTriggersPassed; i < lDTriggers.Count; ++i)
            {
                if (lDTimeOnTheTimeline > lDTimeForTriggers[i])
                {
                    lDTriggers[i].TriggerInTime();
                    lDNumberTriggersPassed++;
                }
                else
                    return;
            }
        }
        //Going Backward
        else
        {
            for (int i = lDNumberTriggersPassed; i > 0; --i)
            {
                if (lDTimeOnTheTimeline < lDTimeForTriggers[i - 1])
                {
                    lDTriggers[i - 1].TriggerInTime();
                    lDNumberTriggersPassed--;
                }
                else
                    return;
            }
        }
    }

    void UpdatePlayer()
    {

        //if reach end
        if (playerTimeOnTheTimeline >= playerLengthOfTimeline && !playerIsRewinding)
        {
            CurrentState = PlayerState.E_FINNISH;
            return;
        }

        //Rewind
        if (playerIsRewinding)
        {
            if (positions.Count > 0)
                for (int i = 0; i < playerRewindScale; ++i)
                {
                    player.position = positions[0];
                    positions.RemoveAt(0);
                }
        }
        else if (!lDIsRewinding)
        {
            //Record
            if (currentState != PlayerState.E_FINNISH)
                positions.Insert(0, player.position);
            //E_Right
            if (currentState == PlayerState.E_RIGHT)
                player.Translate(player.right * playerSpeed * Time.deltaTime);
        }
    }

    void UpdateObjectsToReactivate()
    {
        if (!lDIsRewinding)
            return;

        for (int i = 0; i < lDTemporaryObjectsToReactivate.Count; ++i)
            if (lDTimeOnTheTimeline < lDTimeForObjectsToReactivate[i])
            {
                lDTemporaryObjectsToReactivate[i].SetActive(true);

                //delete the object and the timer from the lists
                lDTemporaryObjectsToReactivate.RemoveAt(i);
                lDTimeForObjectsToReactivate.RemoveAt(i);
            }
    }
    public void AddTemporaryObjectsToReactivate(GameObject objectToAdd)
    {
        lDTemporaryObjectsToReactivate.Add(objectToAdd);
        lDTimeForObjectsToReactivate.Add(lDTimeOnTheTimeline);
        objectToAdd.SetActive(false);
    }

}
