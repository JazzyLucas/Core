using System;
using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core.Input;
using JazzyLucas.Core.Utils;
using UnityEngine;

namespace JazzyLucas.Core
{
    public class PerspectiveController : MonoBehaviour
    {
        [field: SerializeField] public Transform FirstPerson_ViewTransform { get; private set; }
        [field: SerializeField] public Transform ThirdPerson_ViewTransform { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }

        public bool IsFirstPerson { get; private set; }

        private InputPoller inputPoller;
        private void Awake()
        {
            inputPoller = new();
            if (!MainCamera)
                MainCamera = Camera.main;
        }

        private void Update()
        {
            Process();
        }

        public void Process()
        {
            var input = inputPoller.PollInput();
            if (input.R)
            {
                MainCamera.transform.SetParent(IsFirstPerson ? FirstPerson_ViewTransform : ThirdPerson_ViewTransform);
                MainCamera.transform.localPosition = Vector3.zero;
                IsFirstPerson = !IsFirstPerson;
            }
        }
    }
}