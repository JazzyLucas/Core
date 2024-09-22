using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core;
using JazzyLucas.Core.Input;
using UnityEngine;

namespace JazzyLucas.Sample
{
    public class EscapeMenuTest : UIMonoBehavior
    {
        [field: SerializeField] public bool OpenEscapeMenuOnAwake { get; private set; } = false;
    
        private InputPoller inputPoller;
    
        protected override void OnAwake()
        {
            base.OnAwake();
            inputPoller = new();

            Cursor.lockState = OpenEscapeMenuOnAwake ? CursorLockMode.None : CursorLockMode.Locked;
            SetVisibility(OpenEscapeMenuOnAwake);
        }

        private void Update()
        {
            var input = inputPoller.PollInput();

            if (input.PauseEscape == InputState.Pressed)
            {
                SetVisibility(!IsVisible);
                Cursor.lockState = IsVisible ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
    }

}
