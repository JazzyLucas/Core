using System;
using UnityEngine;
using JazzyLucas.Core.Input;
using L = JazzyLucas.Core.Utils.Logger;

namespace JazzyLucas.Core
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerWrapper : MonoBehaviour
    {
        public event Action<ControllerColliderHit> OnColliderHit;
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            OnColliderHit?.Invoke(hit);
        }
    }
}
