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
    public Vector3 firstStackScale;
    [HideInInspector]
    public GameStates gameState;
    public GameObject stackPrefab;
    [HideInInspector]
    public List<StackCube> stackList;
    [HideInInspector]
    public StackCube currentStackObj;
    
    
    public Transform nextStackPosition;
    
    
    public List<Material> stackMaterials;
    
    bool canCut;
    private int materialIndex;
    private int side = 1;

    #region events
    private void OnEnable()
    {
        EventManager.GetStackSize += GetStackSize;
        EventManager.ChangeGameState += ChangeGameState;
        EventManager.ContinueButtonClicked += StartWithNewFinish;
        EventManager.PlayerCanContinue += PlayerCanContinue;
    }
    private void OnDisable()
    {
        EventManager.GetStackSize -= GetStackSize;
        EventManager.ChangeGameState -= ChangeGameState;
        EventManager.ContinueButtonClicked -= StartWithNewFinish;
        EventManager.PlayerCanContinue -= PlayerCanContinue;
    }
    

    #endregion
  
    private Vector3 GetStackSize()
    {
        return firstStackScale;
    }

    private void ChangeGameState(GameStates obj)
    {
        gameState = obj;
    }

    private void StartWithNewFinish() //stackleri finish noktasına taşıyıp ordan oyuna devam etma
    {
        
        canCut = true;
        var obj = Instantiate(stackPrefab, Vector3.zero, quaternion.identity, transform);
        obj.transform.position = nextStackPosition.position;
        currentStackObj = obj.GetComponent<StackCube>();
        
        if (stackList.Count!=0 && finishType==NewFinishTypes.ContinueWithLastScale)
            currentStackObj.transform.localScale = stackList.Last().transform.lossyScale;
        else
            currentStackObj.transform.localScale = firstStackScale;
        
        stackList.Add(currentStackObj);
        nextStackPosition.position = currentStackObj.transform.position + new Vector3(0, 0, firstStackScale.z);
        currentStackObj.SetMaterial(stackMaterials[materialIndex]);

        materialIndex++;
        if (materialIndex >= stackMaterials.Count)
            materialIndex = 0;

        SpawnNextStack();
    }

    

    private void PlayerCanContinue()
    {
        nextStackPosition.position = EventManager.GetFinishPosition() + new Vector3(0, -.5f, firstStackScale.z / 2);

        stackList.First().transform.parent = null;

    }

    private void Start()
    {
        StartWithNewFinish();
        EventManager.ChangeGameState(GameStates.Run);
    }

    void SpawnNextStack()
    {
        if (nextStackPosition.position.z + 1 <= EventManager.GetFinishPosition().z)
        {
            canCut = true;
            var obj = Instantiate(stackPrefab, Vector3.zero, quaternion.identity, stackList.Last().transform);
            obj.transform.position = nextStackPosition.position;
            currentStackObj = obj.GetComponent<StackCube>();
            currentStackObj.StartMovement(side);
            side *= -1;
            currentStackObj.SetMaterial(stackMaterials[materialIndex]);
            materialIndex++;

            if (materialIndex >= stackMaterials.Count)
                materialIndex = 0;
        }
        else
        {
            canCut = false;
        }
    }

    void CutStack()
    {
        if (canCut)
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
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameState == GameStates.Run)
            CutStack();
    }
}