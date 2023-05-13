using System;
using UnityEngine;


public static class EventManager
{


    public static Action<float,Transform> StackCubePlaced;
    public static Func<Vector3> GetFinishPosition;
    public static Action PlayerHitFinish;
    public static Action<GameStates> ChangeGameState;
    public static Action ContinueButtonClicked;
    public static Action RetryButtonClicked;

    public static Action StartWithNewFinish;
    public static Action PlayerCanContinue;

    public static Action LevelWin;
    public static Action LevelFail;
    public static Func<Vector3> GetStackSize;



}