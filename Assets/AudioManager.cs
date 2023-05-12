using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float tolerance;

    private void OnEnable()
    {
        EventManager.StackCubePlaced += StackCubePlaced;
    }

    private void OnDisable()
    {
        EventManager.StackCubePlaced -= StackCubePlaced;
    }

    private void StackCubePlaced(float percent)
    {
        audioSource.Play();
        if (percent>tolerance)
        {
            audioSource.pitch += .1f;
        }
        else
        {
            audioSource.pitch = 1;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
           audioSource.Play();
           audioSource.pitch += .1f;
        }
    }
}
