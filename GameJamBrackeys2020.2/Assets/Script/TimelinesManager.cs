using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    E_IDLE,
    E_RIGHT,
    E_DEAD,
}
public class TimelinesManager : MonoBehaviour
{
    #region LevelState
    bool isLevelFinished = false;
    bool isLevelPaused = false;
    public bool IsLevelPaused
    {
        set
        {
            isLevelPaused = value;

            if (isLevelPaused)
            {
                Time.timeScale = 0;
                canvasAnim.SetBool("isPaused", true);
            }
            else
            {
                canvasAnim.SetBool("isPaused", false);
                if(currentState != PlayerState.E_DEAD)
                    Time.timeScale = 1;
            }
        }
    }
    #endregion

    #region Canvas
    [Header("Canvas")]
    [SerializeField] GameObject canvas = null;
    Animator canvasAnim = null;
    [SerializeField] GameObject logoMagnet = null;
    [SerializeField] GameObject logoMovingPlatform = null;
    [SerializeField] GameObject logoFireball = null;
    #endregion

    #region Post Process
    [Header("Post Process")]
    [SerializeField] GameObject postProcessStandard = null;
    [SerializeField] GameObject postProcessPL = null;
    [SerializeField] GameObject postProcessLD = null;
    [SerializeField] GameObject postProcessDeath = null;
    [SerializeField] GameObject PanelDeath = null;
    #endregion

    #region Player
    [Header("Player Timeline")] 
    [SerializeField] Image playerSlider = null;
    [SerializeField] LineRenderer playerLine = null;
    [SerializeField] float playerLengthOfTimeline = 0;
    [SerializeField] float playerTimeOnTheTimeline = 0f;/////////////////////////////////
    [SerializeField] float playerRewindScale = 0f;
    [SerializeField] float playerCoolDownForRewind = 0f;
    float playerRemainingCoolDownForRewind = 0f;
    bool playerIsRewinding = false;
    public bool PlayerIsRewinding
    {
        get => playerIsRewinding;

        set
        {
            if(playerIsRewinding != value)
            {
                playerIsRewinding = value;
                playerBoxCollider.isTrigger = value;

                Blurr.SetActive(playerIsRewinding);

                postProcessPL.SetActive(playerIsRewinding);
                postProcessStandard.SetActive(!playerIsRewinding);


                if (currentState == PlayerState.E_DEAD)
                    CurrentState = (playerNumberTriggersPassed > 1) ? playerTriggers[playerNumberTriggersPassed - 1] : PlayerState.E_RIGHT;

            }
        }
    }

    [SerializeField] PlayerState currentState = PlayerState.E_RIGHT; /////////////////////////////////
    public PlayerState CurrentState
    {
        set
        {
            if (currentState == PlayerState.E_DEAD)
            {
                playerRb.isKinematic = false;

                postProcessDeath.SetActive(false);
                PanelDeath.SetActive(false);
            }

            currentState = value;

            if (currentState == PlayerState.E_DEAD)
            {
                playerRemainingCoolDownForRewind = 0;
                Time.timeScale = 0;
                playerRb.isKinematic = true;

                postProcessDeath.SetActive(true);
                PanelDeath.SetActive(true);
                postProcessStandard.SetActive(false);
            }

            if (currentState == PlayerState.E_RIGHT)
                playerAnimator.SetBool("isMoving", true);
            else
                playerAnimator.SetBool("isMoving", false);
        }
    }
    [SerializeField] Transform player = null;
    [SerializeField] GameObject Blurr = null;
    [SerializeField] float playerSpeed = 0f;
    BoxCollider2D playerBoxCollider = null;
    Rigidbody2D playerRb = null;
    Animator playerAnimator = null;
    Vector2 playerVelocitySaved = Vector2.zero;
    float playerGravitySaved = 0f;
    [SerializeField] bool canStopRewind = true; /////////////////////////////////
    public bool CanStopRewind
    {
        set => canStopRewind = value;
    }

    List<Vector3> positions = new List<Vector3>();
    List<Vector2> velocitys = new List<Vector2>();


    [SerializeField] PlayerState[] playerTriggers = null;
    [SerializeField] float[] playerTimeForTriggers = null;
    int playerNumberTriggersPassed = 0;



    #endregion

    #region LD
    [Header("LD Timeline")]
    [SerializeField] Slider lDSlider = null;
    [SerializeField] float lDLengthOfTimeline = 0;
    public float LDLengthOfTimeline { get => lDLengthOfTimeline; }
    [SerializeField] float lDTimeOnTheTimeline = 0f; /////////////////////////////////
    public float LDTimeOnTheTimeline { get => lDTimeOnTheTimeline; }
    [SerializeField] float lDRewindScale = 0f;
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
                changeRewindDelegate?.Invoke(lDIsRewinding);
                postProcessLD.SetActive(lDIsRewinding);

                if(lDIsRewinding)
                {
                    postProcessStandard.SetActive(false);
                    Time.timeScale = lDRewindScale;
                
                    //block Player in the air if LDIsRewinding
                    playerVelocitySaved = playerRb.velocity;
                    playerRb.velocity = Vector2.zero;
                    playerGravitySaved = playerRb.gravityScale;
                    playerRb.gravityScale = 0;
                }
                else
                {
                    //Unblock Player in the air if LDIsRewinding
                    playerRb.velocity = playerVelocitySaved;
                    playerRb.gravityScale = playerGravitySaved;

                    if (!playerIsRewinding)
                    {
                        Time.timeScale = 1;
                        postProcessStandard.SetActive(true);
                    }
                }
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
        Time.timeScale = 1;
        
        //Player
        playerBoxCollider = player.GetComponent<BoxCollider2D>();
        playerRb = player.GetComponent<Rigidbody2D>();
        playerAnimator = player.GetComponent<Animator>();
        CurrentState = PlayerState.E_RIGHT;

        //LD
        for (int i = 0; i < lDObjectsToTrigger.Length; ++i)
            lDTriggers.Add(lDObjectsToTrigger[i].GetComponent<ITriggerInTime>());

        //Canvas
        canvasAnim = canvas.GetComponent<Animator>();
        Rect sliderRect = lDSlider.GetComponent<RectTransform>().rect;
        float sliderRatioXPerS = sliderRect.width  / lDLengthOfTimeline;
        Vector3 startingPosition = new Vector3(lDSlider.transform.position.x - sliderRect.width / 2, lDSlider.transform.position.y + sliderRect.height / 2, lDSlider.transform.position.z);

        for (int i = 0; i < lDTriggers.Count; ++i)
        {
            GameObject gameObjectToInstantiate = GetWhichGameObjectInstantiate(lDTriggers[i].GetName());

            Instantiate(gameObjectToInstantiate, startingPosition + Vector3.right * (lDTimeForTriggers[i] * sliderRatioXPerS), Quaternion.identity, canvas.transform);
        }

    }
    GameObject GetWhichGameObjectInstantiate(string triggerName)
    {
        switch (triggerName)
        {
            case "Magnet":
                return logoMagnet;
            case "MovingPlatform":
                return logoMovingPlatform;
            case "Fireball":
                return logoFireball;
            default:
                return null;
        }
    }

    void Update()
    {
        if (isLevelFinished)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            IsLevelPaused = !isLevelPaused;

        if (isLevelPaused)
            return;

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
        else if (playerIsRewinding && canStopRewind)
        {
            playerRemainingCoolDownForRewind = playerCoolDownForRewind;
            PlayerIsRewinding = false;
        }

        //LD
        if (Input.GetKey(KeyCode.E) && lDRemainingCoolDownForRewind < 0 && !playerIsRewinding && currentState != PlayerState.E_DEAD)
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
            else if(currentState != PlayerState.E_DEAD)
                playerTimeOnTheTimeline += (playerIsRewinding) ? -Time.unscaledDeltaTime * playerRewindScale : Time.unscaledDeltaTime;

            playerTimeOnTheTimeline = Mathf.Clamp(playerTimeOnTheTimeline, 0, playerLengthOfTimeline);
        }


        //LD
        if (!playerIsRewinding)
        {
            lDTimeOnTheTimeline += (lDIsRewinding) ? -Time.deltaTime : Time.deltaTime;
            lDTimeOnTheTimeline = Mathf.Clamp(lDTimeOnTheTimeline, 0, lDLengthOfTimeline);
        }
    }

    void UpdateSliders()
    {
        //Player
        playerSlider.fillAmount = playerTimeOnTheTimeline / playerLengthOfTimeline;
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
                    CurrentState = playerTriggers[i];
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
            CurrentState = PlayerState.E_DEAD;
            return;
        }

        //Rewind
        if (playerIsRewinding)
        {
            if (positions.Count >= playerRewindScale)
                for (int i = 0; i < playerRewindScale; ++i)
                {
                    player.position = positions[0];
                    positions.RemoveAt(0);

                    playerRb.velocity = velocitys[0];
                    velocitys.RemoveAt(0);


                    playerLine.positionCount--;
                }
        }
        else if (!lDIsRewinding)
        {
            //Record
            if (currentState != PlayerState.E_DEAD)
            {
                positions.Insert(0, player.position);
                velocitys.Insert(0, playerRb.velocity);
                playerLine.positionCount++;
                playerLine.SetPosition(playerLine.positionCount - 1, player.position);
            }
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
            if (lDTimeOnTheTimeline <= lDTimeForObjectsToReactivate[i])
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


    public void OnEndLevel()
    {
        isLevelFinished = true;
        Time.timeScale = 0;
    }

}
