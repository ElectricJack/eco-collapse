using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameDialog : MonoBehaviour
{
    public Grader grader;

    public TMPro.TMP_Text text;
    public GameObject[] Descripts;

    public Button LeaveGame;
    
    public void OnEnable()
    {
        var finalGrade = grader.GetFinalGrade();

        foreach (var descript in Descripts)
        {
            descript.SetActive(false);
        }
        
        Descripts[(int)finalGrade].SetActive(true);
        text.text = finalGrade.ToString();
        
        LeaveGame.onClick.AddListener(() => { SceneManager.LoadScene("MainMenu"); });
    }
}

public enum Grade
{
    A = 0,
    B,
    C,
    D,
    F
}