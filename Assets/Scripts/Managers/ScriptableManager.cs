using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ScriptableManager : MonoBehaviour
{
    [SerializeField] StackSettings stackSettings;


    //-------------------------------------------------------------------
    void Awake()
    {

        Scriptable.GetStackSettings = GetStackSettings;
    }


    //-------------------------------------------------------------------
    StackSettings GetStackSettings() => stackSettings;


    //-------------------------------------------------------------------

}



public static class Scriptable
{
    public static Func<StackSettings> GetStackSettings;
}