using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JazzyLucas.Core.Input;
using JazzyLucas.Core.Utils;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class PerspectiveController : Controller
    {
        [field: SerializeField] public Transform FirstPerson_ViewTransform { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public float ThirdPersonDistance { get; private set; } = 5f;
        [field: SerializeField] public LayerMask CameraDistanceCollision_LayerMask { get; private set; } = ~0;
        [field: Header("Visuals")]
        [field: SerializeField] public Transform ThirdPersonVisualsRoot { get; private set; }
        [field: Header("Init Control")]
        [field: SerializeField] public bool ThirdPersonOnInit { get; private set; } = false;

        [field: HideInInspector] public bool IsThirdPerson { get; private set; }

        public override void Init()
        {
            base.Init();
            IsThirdPerson = ThirdPersonOnInit;
            InitCamera();
        }

        protected override void Process()
        {
            var input = inputPoller.PollInput();
            
            if (input.R == InputState.Pressed)
            {
                IsThirdPerson = !IsThirdPerson;

                ToggleThirdPersonVisuals(IsThirdPerson);
                
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

        private void InitCamera()
        {
            if (!MainCamera)
                MainCamera = Camera.main;
            // ReSharper disable once PossibleNullReferenceException
            MainCamera.transform.SetParent(FirstPerson_ViewTransform);
            MainCamera.transform.localPosition = Vector3.zero;
            ToggleThirdPersonVisuals(IsThirdPerson);
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

        public void ToggleThirdPersonVisuals(bool value)
        {
            VisualUtils.ToggleVisuals(ThirdPersonVisualsRoot, value);
        }
    }
}
