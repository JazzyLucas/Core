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
        [field: SerializeField] public bool FirstPersonOnAwake { get; private set; } = true;
        
        [field: Header("Visuals")]
        [field: SerializeField] public Transform ThirdPersonVisualsRoot { get; private set; }
        [field: SerializeField] public Transform FirstPersonVisualsRoot { get; private set; }

        public bool IsFirstPerson { get; private set; }

        private InputPoller inputPoller;

        private void Awake()
        {
            inputPoller = new();
            if (!MainCamera)
                MainCamera = Camera.main;
            IsFirstPerson = FirstPersonOnAwake;
            AdjustVisibility();
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
                IsFirstPerson = !IsFirstPerson;

                if (IsFirstPerson)
                {
                    MainCamera.transform.SetParent(FirstPerson_ViewTransform);
                    MainCamera.transform.localPosition = Vector3.zero;
                }
                else
                {
                    AdjustThirdPersonCamera();
                }
                AdjustVisibility();
            }

            if (!IsFirstPerson)
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

        private void AdjustVisibility()
        {
            VisualUtils.ToggleVisuals(FirstPersonVisualsRoot, IsFirstPerson);
            VisualUtils.ToggleVisuals(ThirdPersonVisualsRoot, !IsFirstPerson);
        }
    }
}
