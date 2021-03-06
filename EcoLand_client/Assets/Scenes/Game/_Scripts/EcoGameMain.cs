﻿using System.Collections;
using System.Collections.Generic;
using Josh;
using UnityEngine;

public class EcoGameMain : MonoBehaviour
{
    public WorldStepper Stepper;

    public GameObject EndGameScreen;

    public const float GameLength = 60f * 3f;
    public float TimeRemainging => Stepper.isReady ? GameLength - (Time.timeSinceLevelLoad - TimeAtStart) : GameLength;
    private float TimeAtStart = 0f;
    
    public void Start()
    {
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        while (!Stepper.isReady)
        {
            yield return null;
        }

        TimeAtStart = Time.timeSinceLevelLoad + 1f;

        StartCoroutine(PlayGame());
    }

    public IEnumerator PlayGame()
    {

        while (Time.timeSinceLevelLoad - TimeAtStart < GameLength)
        {
            yield return null;
        }
        Stepper.enabled = false;
        // Game Over
        // Show Gameover Dialog and Look at Ecosystem
        
        EndGameScreen.SetActive(true);
    }
}

// Enable Buttons
// On click open sub menu
