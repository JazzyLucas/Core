using System;
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
        [field: SerializeField] public LayerMask CollisionLayers { get; private set; } = ~0;

        public bool IsFirstPerson { get; private set; }

        private InputPoller inputPoller;

        private void Awake()
        {
            inputPoller = new InputPoller();
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

            if (Physics.Raycast(FirstPerson_ViewTransform.position, directionToCamera, out RaycastHit hit, ThirdPersonDistance, CollisionLayers))
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
    }
}
