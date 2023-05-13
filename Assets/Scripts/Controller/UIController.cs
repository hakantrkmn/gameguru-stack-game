using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject levelWinPanel;
    public GameObject retryPanel;

    private void OnEnable()
    {
        EventManager.LevelFail += LevelFail;
        EventManager.LevelWin += LevelWin;
        EventManager.PlayerCanContinue += PlayerCanContinue;
        EventManager.ContinueButtonClicked += ContinueButtonClicked;
    }

    private void ContinueButtonClicked()
    {
        continueButton.SetActive(false);
    }

    private void PlayerCanContinue()
    {
        continueButton.SetActive(true);
        
    }

    private void LevelWin()
    {
        levelWinPanel.SetActive(true);
        
    }

    private void LevelFail()
    {
        retryPanel.SetActive(true);
    }

    private void OnDisable()
    {
        EventManager.LevelFail -= LevelFail;
        EventManager.LevelWin -= LevelWin;
        EventManager.PlayerCanContinue -=  PlayerCanContinue;
        EventManager.ContinueButtonClicked -= ContinueButtonClicked;
    }
}