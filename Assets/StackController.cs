using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

public class StackController : MonoBehaviour
{
    public GameObject stackPrefab;
    public List<StackCube> stackList;
    public StackCube currentStackObj;
    public float tolerance;

    [Button]
    public void SpawnNextStack()
    {
        var obj = Instantiate(stackPrefab, Vector3.zero, quaternion.identity, stackList.Last().transform);
        obj.transform.localPosition = new Vector3(1, 0, 1);
        currentStackObj = obj.GetComponent<StackCube>();
        currentStackObj.StartMovement();
    }


    [Button]
    public void CutStack()
    {
        currentStackObj.StopMovement();
        var lastStack = stackList.Last().transform;
        var correction = (Mathf.Abs(currentStackObj.transform.localPosition.x) * stackList.Last().transform.lossyScale.x);
        var percent = 100 - ((100 * correction) / lastStack.lossyScale.x);
        EventManager.StackCubePlaced(percent);
        currentStackObj.CutCube(lastStack.lossyScale,correction,stackPrefab);

        stackList.Add(currentStackObj);
    }
}