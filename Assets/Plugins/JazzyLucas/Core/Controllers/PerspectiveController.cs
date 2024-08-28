using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JazzyLucas.Core.Input;
using JazzyLucas.Core.Utils;

namespace JazzyLucas.Core
{
    public class PerspectiveController : MonoBehaviour
    {
        [field: SerializeField] public Transform FirstPerson_ViewTransform { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public float ThirdPersonDistance { get; private set; } = 5f;
        [field: SerializeField] public LayerMask CameraDistanceCollision_LayerMask { get; private set; } = ~0;
        [field: SerializeField] public bool ThirdPersonOnAwake { get; private set; } = false;
        
        [field: Header("Visuals")]
        [field: SerializeField] public Transform ThirdPersonVisualsRoot { get; private set; }

        public bool IsThirdPerson { get; private set; }

        private InputPoller inputPoller;

        private void Awake()
        {
            inputPoller = new();
            if (!MainCamera)
                MainCamera = Camera.main;
            IsThirdPerson = ThirdPersonOnAwake;
            ToggleThirdPersonVisuals(IsThirdPerson);
        }

        private void Update()
        {
            var input = inputPoller.PollInput();
            Process(input);
        }

        private void Process(InputData input)
        {
            if (input.R)
            {
                IsThirdPerson = !IsThirdPerson;

                if (IsThirdPerson)
                {
                    AdjustThirdPersonCamera();
                }
                else
                {
                    MainCamera.transform.SetParent(FirstPerson_ViewTransform);
                    MainCamera.transform.localPosition = Vector3.zero;
                }

                ToggleThirdPersonVisuals(IsThirdPerson);
            }

            if (IsThirdPerson)
            {
                AdjustThirdPersonCamera();
            }
        }

        private void AdjustThirdPersonCamera()
        {
            var desiredPosition = FirstPerson_ViewTransform.position - FirstPerson_ViewTransform.forward * ThirdPersonDistance;
            var directionToCamera = desiredPosition - FirstPerson_ViewTransform.position;

            if (Physics.Raycast(FirstPerson_ViewTransform.position, directionToCamera, out RaycastHit hit, ThirdPersonDistance, CameraDistanceCollision_LayerMask))
            {
                MainCamera.transform.position = hit.point - directionToCamera.normalized * 0.1f;
            }
            else
            {
                MainCamera.transform.position = desiredPosition;
            }

            MainCamera.transform.LookAt(FirstPerson_ViewTransform);
            MainCamera.transform.SetParent(null);
        }

        public void ToggleThirdPersonVisuals(bool value)
        {
            VisualUtils.ToggleVisuals(ThirdPersonVisualsRoot, value);
        }
    }
}
