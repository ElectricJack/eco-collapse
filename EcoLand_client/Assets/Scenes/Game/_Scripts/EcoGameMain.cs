using System.Collections;
using System.Collections.Generic;
using Josh;
using UnityEngine;

public class EcoGameMain : MonoBehaviour
{
    public WorldStepper Stepper;

    public float TimeRemainging = 1000f * 60f * 5f;
    
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

        StartCoroutine(PlayGame());
    }

    public IEnumerator PlayGame()
    {
        // Enable Buttons
        
        TimeRemainging = 1000f*60f * 5f;
        while (TimeRemainging >= 0)
        {
            yield return null;
            TimeRemainging -= Time.time;
        }

        Stepper.enabled = false;
        // Game Over
        // Show Gameover Dialog and Look at Ecosystem
        
        Debug.LogError("GAME OVER");
    }
}

// Enable Buttons
// On click open sub menu
