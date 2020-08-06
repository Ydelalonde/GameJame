using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    Animator anim = null;
    int levelToLoad = 0;


    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void AnimLoadLevelWithIndex(int index)
    {
        levelToLoad = index;
        anim.SetTrigger("LoadingLevel");
    }

    void LoadLevelWithIndex()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
