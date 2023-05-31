using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public NewFinishTypes finishType; 
    Vector3 firstStackScale;
    [HideInInspector] public GameStates gameState;
    [HideInInspector] public GameObject stackPrefab;
    [HideInInspector] public List<StackCube> stackList;
    [HideInInspector] public StackCube currentStackObj;
    [HideInInspector]public Transform nextStackPosition;


    public List<Material> stackMaterials;

    private int _materialIndex;
    private int _side = 1;

    #region events

    private void OnEnable()
    {
        EventManager.GetStackSize += () => firstStackScale;
        EventManager.ChangeGameState += states => gameState=states;
        EventManager.ContinueButtonClicked += StartWithNewFinish;
        EventManager.PlayerCanContinue += PlayerCanContinue;
    }

    private void OnDisable()
    {
        EventManager.GetStackSize -= () => firstStackScale;
        EventManager.ChangeGameState -= states => gameState=states;
        EventManager.ContinueButtonClicked -= StartWithNewFinish;
        EventManager.PlayerCanContinue -= PlayerCanContinue;
    }

    #endregion
    


    private void StartWithNewFinish() //stackleri finish noktasına taşıyıp ordan oyuna devam etma
    {
        firstStackScale = Scriptable.GetStackSettings().firstStackScale;
        var firstStack = Instantiate(stackPrefab, Vector3.zero, quaternion.identity, transform);
        firstStack.transform.position = nextStackPosition.position;
        currentStackObj = firstStack.GetComponent<StackCube>();

        if (stackList.Count != 0 && finishType == NewFinishTypes.ContinueWithLastScale)
            currentStackObj.transform.localScale = stackList.Last().transform.lossyScale;
        else
            currentStackObj.transform.localScale = firstStackScale;

        stackList.Add(currentStackObj);
        nextStackPosition.position = currentStackObj.transform.position + new Vector3(0, 0, firstStackScale.z);
        currentStackObj.SetMaterial(stackMaterials[_materialIndex]);

        SetNewMaterial();

        SpawnNextStack();
    }

    void SetNewMaterial()
    {
        _materialIndex++;
        if (_materialIndex >= stackMaterials.Count)
            _materialIndex = 0;
    }


    private void PlayerCanContinue()
    {
        nextStackPosition.position = EventManager.GetFinishPosition() + new Vector3(0, -.5f, firstStackScale.z / 2);
        stackList.First().transform.parent = null;
    }

    private void Start()
    {
        StartWithNewFinish();
    }

    void SpawnNextStack()
    {
        if (!(nextStackPosition.position.z + 1 <= EventManager.GetFinishPosition().z))
        {
            EventManager.ChangeGameState(GameStates.StackReachedFinish);
            return;
        }
        
        var obj = Instantiate(stackPrefab, Vector3.zero, quaternion.identity, stackList.Last().transform);
        obj.transform.position = nextStackPosition.position;
        currentStackObj = obj.GetComponent<StackCube>();
        currentStackObj.StartMovement(_side);
        _side *= -1;
        currentStackObj.SetMaterial(stackMaterials[_materialIndex]);
       
        SetNewMaterial();
    }

    void CutStack()
    {
        currentStackObj.StopMovement();
        var lastStack = stackList.Last().transform;
        var correction = (Mathf.Abs(currentStackObj.transform.localPosition.x) *
                          stackList.Last().transform.lossyScale.x);
        var percent = 100 - ((100 * correction) / lastStack.lossyScale.x);
        if (percent > 0)
        {
            currentStackObj.CutCube(lastStack.lossyScale, correction, stackPrefab);
            EventManager.StackCubePlaced(percent, currentStackObj.transform);

            stackList.Add(currentStackObj);

            nextStackPosition.position = currentStackObj.transform.position + new Vector3(0, 0, firstStackScale.z);

            SpawnNextStack();
        }
    }


    private void Update()
    {
        if (gameState==GameStates.Run || gameState == GameStates.Wait)
        {
            if (Input.GetMouseButtonDown(0))
                CutStack();
        }
        
    }
}