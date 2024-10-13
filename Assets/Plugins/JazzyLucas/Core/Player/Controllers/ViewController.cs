using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JazzyLucas.Core.Input;
using JazzyLucas.Core.Utils;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    public class ViewController : Controller
    {
        [field: SerializeField] public Transform ViewTransform { get; private set; }
        [field: SerializeField] public bool LockCursorOnInit { get; private set; } = true;
        
        [field: HideInInspector] private Angle yaw;
        [field: HideInInspector] private Angle pitch;

        public override void Init()
        {
            base.Init();
            if (LockCursorOnInit)
                Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void Process()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                return;
            
            var input = inputPoller.PollInput();
            
            Vector2 delta = input.MouseDelta;
            
            yaw += delta.x;
            pitch += delta.y;
                
            pitch = AngleUtil.CustomClampAngle(pitch);
                
            var rotation = Quaternion.Euler((float)pitch, (float)yaw, 0);
            ViewTransform.rotation = rotation;
        }
        
        public Angle GetYaw() => ViewTransform.rotation.eulerAngles.y;
        
        public Angle GetPitch() => ViewTransform.rotation.eulerAngles.x;
    }
}
