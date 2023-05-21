using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera gameCam;
    public CinemachineFreeLook danceCam;
    public GameStates gameState;

    #region events

    private void OnEnable()
    {
        EventManager.LevelWin += LevelWin;
        EventManager.ContinueButtonClicked += ContinueButtonClicked;
        EventManager.ChangeGameState += ChangeGameState;
        EventManager.PlayerHitFinish += PlayerHitFinish;
    }


    private void OnDisable()
    {
        EventManager.LevelWin -= LevelWin;
        EventManager.ContinueButtonClicked -= ContinueButtonClicked;
        EventManager.ChangeGameState -= ChangeGameState;
        EventManager.PlayerHitFinish -= PlayerHitFinish;
    }

    #endregion


    private void Start()
    {
        var player = GameObject.FindObjectOfType<PlayerController>().transform;
        gameCam.Follow = player;
        gameCam.LookAt = player;
        danceCam.Follow = player;
        danceCam.LookAt = player;
    }

    private void LevelWin()
    {
        gameCam.Priority = 1;
        danceCam.Priority = 10;
    }

    private void ChangeGameState(GameStates obj)
    {
        gameState = obj;
    }

    private void ContinueButtonClicked()
    {
        gameCam.Priority = 10;
        danceCam.Priority = 1;
        EventManager.ChangeGameState(GameStates.Run);
    }


    private void PlayerHitFinish()
    {
        gameCam.Priority = 1;
        danceCam.Priority = 10;
    }


    private void Update()
    {
        if (gameState == GameStates.Dance)
            danceCam.m_XAxis.Value += 20 * Time.deltaTime;
    }
}