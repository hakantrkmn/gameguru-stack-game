using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ScriptableManager : MonoBehaviour
{
    [SerializeField] GameData gameData;


    //-------------------------------------------------------------------
    void Awake()
    {
        SaveManager.LoadGameData(gameData);

        Scriptable.GameData = GetGameData;
    }


    //-------------------------------------------------------------------
    GameData GetGameData() => gameData;


    //-------------------------------------------------------------------

}



public static class Scriptable
{
    public static Func<GameData> GameData;
}