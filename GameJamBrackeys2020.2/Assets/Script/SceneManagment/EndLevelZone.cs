﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelZone : MonoBehaviour
{
    [SerializeField] Animator animPauseMenu = null;
    TimelinesManager timelinesManager = null;
    Animator anim = null;

    private void Start()
    {
        timelinesManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimelinesManager>();
        anim = gameObject.GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            timelinesManager.OnEndLevel();
            anim.SetBool("ReachEndLevel", true);
        }
    }

    void StartAnimLoadingNextLevel()
    {
        animPauseMenu.SetTrigger("LoadingNextLevel");
    }
}
