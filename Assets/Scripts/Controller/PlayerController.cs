using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public ForwardMovement forwardMovement;
    public GameStates gameState;

    private void Update()
    {
        if (gameState==GameStates.Run)
        {
            if (transform.position.y < -.5f)
            {
                EventManager.LevelFail();
                EventManager.ChangeGameState(GameStates.Fail);
            }
        }
        else if (gameState==GameStates.Wait)
        {
            if (Input.GetMouseButtonDown(0))
            {
                forwardMovement.canControl = true;
                animator.SetBool("run",true);
                EventManager.ChangeGameState(GameStates.Run);
            }
        }
        
    }

    #region events
    private void OnEnable()
    {
        EventManager.ChangeGameState += states => gameState = states;
        EventManager.StackCubePlaced += StackCubePlaced;
        EventManager.ContinueButtonClicked += StartWithNewFinish;
    }
    private void OnDisable()
    {
        EventManager.ChangeGameState -= states => gameState = states;
        EventManager.StackCubePlaced -= StackCubePlaced;
        EventManager.ContinueButtonClicked -= StartWithNewFinish;

    }
    

    #endregion
   
    private void StackCubePlaced(float arg1, Transform arg2)
    {
        transform.DOMoveX(arg2.position.x, .1f);
    }

   

    private void StartWithNewFinish()
    {
        EventManager.ChangeGameState(GameStates.Wait);
        animator.SetBool("dance",false);
        animator.SetBool("run",false);

    }

    void PlayerHitTheEnd(Transform finishTransform)
    {
        transform.DOMove(finishTransform.position, .1f);
        animator.SetBool("dance",true);
        forwardMovement.canControl = false;
        EventManager.PlayerHitFinish();
        EventManager.ChangeGameState(GameStates.Dance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Finish>())
        {
            PlayerHitTheEnd(collision.transform);
        }
    }
}
