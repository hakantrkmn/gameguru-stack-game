using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class StackCube : MonoBehaviour
{
    private Sequence _movement;

    public void StartMovement(int side)
    {
        transform.localPosition += new Vector3(side * 2, 0, 0);
        
        _movement = DOTween.Sequence();

        _movement.Append(transform.DOLocalMoveX(-side * 2, Scriptable.GetStackSettings().stackSpeed));
        _movement.Append(transform.DOLocalMoveX(side * 2, Scriptable.GetStackSettings().stackSpeed));
        _movement.SetLoops(-1, LoopType.Yoyo);
        
    }

    public void SetMaterial(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
    }

    public void StopMovement()
    {
        _movement.Kill();
    }

    public void CutCube(Vector3 lastStackScale, float correction, GameObject stackPrefab)
    {
        if (lastStackScale.x - correction > 0)
        {
            if (transform.localPosition.x >= 0)
            {
                transform.localScale = new Vector3((lastStackScale.x - correction) / lastStackScale.x, 1, 1);
                transform.localPosition -= new Vector3((correction / lastStackScale.x) / 2, 0, 0);


                var fallObj = Instantiate(stackPrefab, transform.position, quaternion.identity,
                    transform.parent).transform;
                fallObj.localScale = new Vector3((1 - transform.localScale.x), 1, 1);
                fallObj.transform.localPosition =
                    new Vector3(transform.localPosition.x + .5f, 0, transform.localPosition.z);
                fallObj.GetComponent<StackCube>().SetMaterial(GetComponent<MeshRenderer>().material);
                fallObj.GetComponent<Rigidbody>().isKinematic = false;
                fallObj.GetComponent<Rigidbody>().AddForce(new Vector3(fallObj.transform.localPosition.x,.5f,0)*100);

                Destroy(fallObj.gameObject,2);
            }
            else
            {
                transform.localScale = new Vector3((lastStackScale.x - correction) / lastStackScale.x, 1, 1);
                transform.localPosition += new Vector3((correction / lastStackScale.x) / 2, 0, 0);


                var fallObj = Instantiate(stackPrefab, transform.position, quaternion.identity,
                    transform.parent).transform;
                fallObj.localScale = new Vector3((1 - transform.localScale.x), 1, 1);
                fallObj.transform.localPosition =
                    new Vector3(transform.localPosition.x - .5f, 0, transform.localPosition.z);
                fallObj.GetComponent<Rigidbody>().isKinematic = false;
                fallObj.GetComponent<StackCube>().SetMaterial(GetComponent<MeshRenderer>().material);
                fallObj.GetComponent<Rigidbody>().AddForce(new Vector3(fallObj.transform.localPosition.x,.5f,0)*100);

                Destroy(fallObj.gameObject,2);

            }

        }
       
        
    }
}