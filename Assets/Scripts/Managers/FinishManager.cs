using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    public List<Finish> finishes;
    private int finishIndex;
    public int objectAmountBetweenFinishes;
    private void OnValidate()
    {
        finishes.Clear();
        foreach (var finish in GetComponentsInChildren<Finish>())
            finishes.Add(finish);
    }

    private void OnEnable()
    {
        EventManager.PlayerHitFinish += PlayerHitFinish;
        EventManager.GetFinishPosition += GetFinishPosition;
    }

    private void Start()
    {
        PlaceFinishes();
    }

     void PlaceFinishes()
    {
        var size = EventManager.GetStackSize();
        var finishZSize = 1.80f / 2;
        var zPos = objectAmountBetweenFinishes * size.z + size.z / 2 + finishZSize;
        finishes.First().transform.position = new Vector3(0, 0, zPos);
        for (int i = 1; i < finishes.Count; i++)
            finishes[i].transform.position =new Vector3(0, 0, (objectAmountBetweenFinishes * size.z + (finishZSize*2))+finishes[i-1].transform.position.z);
        

    }

    private void PlayerHitFinish()
    {
        if (finishIndex+1>=finishes.Count)
            EventManager.LevelWin();
        else
        {
            EventManager.PlayerCanContinue();
            finishIndex++;
        }
            
    }

    private void OnDisable()
    {
        EventManager.PlayerHitFinish -= PlayerHitFinish;
        EventManager.GetFinishPosition -= GetFinishPosition;
    }

    private Vector3 GetFinishPosition()
    {
        return finishes[finishIndex].stackPos.position;
    }
}