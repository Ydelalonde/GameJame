using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    Animator anim = null;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }


    public void AnimRestartLevel()
    {
        anim.SetTrigger("ReloadingLevel");
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AnimGoToTitleScreen()
    {
        anim.SetTrigger("LoadingTitleScreen");
    }

    void GoToTitleScreen()
    {
        SceneManager.LoadScene(0);
    }

}
