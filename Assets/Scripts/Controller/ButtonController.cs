using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public ButtonTypes buttonType;

    private void OnEnable()
    {
        EventManager.RetryButtonClicked += () => SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        EventManager.RetryButtonClicked -= () => SceneManager.LoadScene(0);
    }

    public void ButtonClicked()
    {
        switch (buttonType)
        {
            case ButtonTypes.Continue:
                EventManager.ContinueButtonClicked();
                break;
            case ButtonTypes.Retry:
                EventManager.RetryButtonClicked();
                break;
                
        }
    }
}
