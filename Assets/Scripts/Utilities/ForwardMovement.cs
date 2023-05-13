using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovement : MonoBehaviour
{
    public bool canControl;
    public float speed;

    void Update()
    {
        if (!canControl)
            return;

        transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
    }
}