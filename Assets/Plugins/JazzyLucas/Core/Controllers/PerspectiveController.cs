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
            SetupCamera();
        }

        private void Update()
        {
            var input = inputPoller.PollInput();
            Process(input);
        }

        private void Process(InputData input)
        {
            if (input.R == InputState.Pressed)
            {
                IsThirdPerson = !IsThirdPerson;

                ToggleThirdPersonVisuals();
                
                if (IsThirdPerson)
                {
                    AdjustThirdPersonCamera();
                }
                else
                {
                    MainCamera.transform.localPosition = Vector3.zero;
                }
            }

            if (IsThirdPerson)
            {
                AdjustThirdPersonCamera();
            }
        }

        private void SetupCamera()
        {
            MainCamera.transform.SetParent(FirstPerson_ViewTransform);
            MainCamera.transform.localPosition = Vector3.zero;
            IsThirdPerson = ThirdPersonOnAwake;
            ToggleThirdPersonVisuals();
        }
        
        private void AdjustThirdPersonCamera()
        {
            var desiredPosition = FirstPerson_ViewTransform.position - FirstPerson_ViewTransform.forward * ThirdPersonDistance;
            var directionToCamera = desiredPosition - FirstPerson_ViewTransform.position;

            MainCamera.transform.position = Physics.Raycast(FirstPerson_ViewTransform.position, directionToCamera, out RaycastHit hit, ThirdPersonDistance, CameraDistanceCollision_LayerMask) ? 
                Vector3.Lerp(MainCamera.transform.position, hit.point - directionToCamera.normalized * 0.1f, Time.deltaTime * 10f) : 
                Vector3.Lerp(MainCamera.transform.position, desiredPosition, Time.deltaTime * 10f);

            MainCamera.transform.LookAt(FirstPerson_ViewTransform);
        }


        public void ToggleThirdPersonVisuals()
        {
            VisualUtils.ToggleVisuals(ThirdPersonVisualsRoot, IsThirdPerson);
        }
    }
}
