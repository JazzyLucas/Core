using System.Collections;
using System.Collections.Generic;
using JazzyLucas.Core.Input;
using JazzyLucas.Core.Utils;
using UnityEngine;

namespace JazzyLucas.Core
{
    public class ViewController : MonoBehaviour
    {
        [field: SerializeField] public Transform ViewTransform { get; private set; }
        [field: SerializeField] public bool LockCursorOnAwake { get; private set; } = true;

        private InputPoller inputPoller;
        
        private Angle yaw;
        public Angle GetYaw() => ViewTransform.rotation.eulerAngles.y;
        private Angle pitch;
        public Angle GetPitch() => ViewTransform.rotation.eulerAngles.x;

        private void Awake()
        {
            inputPoller = new();
            if (LockCursorOnAwake)
                Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                return;
            
            var input = inputPoller.PollInput();
            Process(input);
        }

        private void Process(InputData input)
        {
            Vector2 delta = input.MouseDelta;
            
            yaw += delta.x;
            pitch += delta.y;
                
            pitch = AngleUtil.CustomClampAngle(pitch);
                
            var rotation = Quaternion.Euler((float)pitch, (float)yaw, 0);
            ViewTransform.rotation = rotation;
        }
    }
}
