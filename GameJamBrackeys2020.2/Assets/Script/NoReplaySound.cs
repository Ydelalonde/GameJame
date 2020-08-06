using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoReplaySound : MonoBehaviour
{

    private int actualSceneIndex;
    static Object instance = null;

    void Awake()
    {
        if (instance != null )
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Update()
    {
        actualSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (actualSceneIndex == 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
