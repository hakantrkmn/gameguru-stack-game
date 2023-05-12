using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class StackCube : MonoBehaviour
{
    public Rigidbody rb;
    private Sequence movement;

    private void Start()
    {
    }

    public void StartMovement()
    {
        movement = DOTween.Sequence();

        movement.Append(transform.DOLocalMoveX(-1, 2f));
        movement.Append(transform.DOLocalMoveX(1, 2f));
        movement.SetLoops(-1, LoopType.Yoyo);
    }

    public void StopMovement()
    {
        movement.Kill();
    }
    public void CutCube(Vector3 lastStackScale,float correction,GameObject stackPrefab)
    {
        if (transform.localPosition.x>=0)
        {
            transform.localScale = new Vector3((lastStackScale.x - correction)/lastStackScale.x, 1, 1) ;
            transform.localPosition -= new Vector3((correction / lastStackScale.x) / 2, 0, 0);

        
            var fallObj = Instantiate(stackPrefab, transform.position, quaternion.identity,
                transform.parent).transform;
            fallObj.localScale = new Vector3((1 - transform.localScale.x), 1, 1);
            fallObj.transform.localPosition =
                new Vector3(transform.localPosition.x + .5f , 0,transform.localPosition.z );

            fallObj.GetComponent<Rigidbody>().isKinematic = false;

        }
        else
        {
            transform.localScale = new Vector3((lastStackScale.x - correction)/lastStackScale.x, 1, 1) ;
            transform.localPosition += new Vector3((correction / lastStackScale.x) / 2, 0, 0);

        
            var fallObj = Instantiate(stackPrefab, transform.position, quaternion.identity,
                transform.parent).transform;
            fallObj.localScale = new Vector3((1 - transform.localScale.x), 1, 1);
            fallObj.transform.localPosition =
                new Vector3(transform.localPosition.x - .5f , 0,transform.localPosition.z );
            fallObj.GetComponent<Rigidbody>().isKinematic = false;


        }
        
    }
}
