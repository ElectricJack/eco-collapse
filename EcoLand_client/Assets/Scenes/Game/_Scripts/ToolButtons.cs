using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolButtons : MonoBehaviour
{
    public Button FloraButton;

    public void OnEnable()
    {
        FloraButton.onClick.AddListener(FloraPress);
    }

    public void OnDisable()
    {
        FloraButton.onClick.RemoveAllListeners();
    }

    private void FloraPress()
    {
        // On press open menu and initialize popoout.
    }
}
