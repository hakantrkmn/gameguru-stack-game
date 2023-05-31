using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinishManager : MonoBehaviour
{
    private List<Finish> _finishes;
    public int objectAmountBetweenFinishes;

    private void OnValidate()
    {
        _finishes = new List<Finish>();
        _finishes.Clear();
        foreach (var finish in GetComponentsInChildren<Finish>())
            _finishes.Add(finish);
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
        _finishes.First().transform.position = new Vector3(0, 0, zPos);
        for (int i = 1; i < _finishes.Count; i++)
            _finishes[i].transform.position = new Vector3(0, 0,
                (objectAmountBetweenFinishes * size.z + (finishZSize * 2)) + _finishes[i - 1].transform.position.z);
    }

    private void PlayerHitFinish()
    {
        if (_finishes.Count == 0)
            EventManager.LevelWin();
        else
        {
            EventManager.PlayerCanContinue();
            _finishes.Remove(_finishes.First());
        }
    }

    private void OnDisable()
    {
        EventManager.PlayerHitFinish -= PlayerHitFinish;
        EventManager.GetFinishPosition -= GetFinishPosition;
    }

    private Vector3 GetFinishPosition()
    {
        return _finishes.First().stackPos.position;
    }
}