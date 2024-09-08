using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using JazzyLucas.Core.Input;
using UnityEngine;

public class EscapeMenuTest : UIMonoBehavior
{
    private InputPoller inputPoller;
    
    protected override void OnAwake()
    {
        inputPoller = new();
        base.OnAwake();
    }

    private void Update()
    {
        var input = inputPoller.PollInput();

        if (input.PauseEscape == InputState.Pressed)
        {
            SetVisibility(!IsVisible);
            Cursor.lockState = IsVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        Debug.Log($"{input.PauseEscape}");
    }
}
