using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JazzyLucas.Core.Utils
{
    public class TransformAnchor : MonoBehaviour
    {
        [field: SerializeField] public Transform childTransform { get; set; }
        [field: SerializeField] public Transform parentTransform { get; set; }
        [field: SerializeField] public Vector3 offset { get; set; }
        [field: SerializeField] public bool matchTransform { get; set; }
        [field: SerializeField] public bool matchRotation { get; set; }
        [field: SerializeField] public bool matchYAxisOnly { get; set; }
        
        private void Awake()
        {
        }

        private void LateUpdate()
        {
            Process();
        }
        
        private void Process()
        {
            if (matchTransform)
                childTransform.position = parentTransform.position;
            if (matchRotation)
                childTransform.rotation = parentTransform.rotation;
            if (matchYAxisOnly)
                childTransform.rotation = Quaternion.Euler(0f, parentTransform.rotation.eulerAngles.y, 0f);
            if (offset != Vector3.zero)
                childTransform.Translate(offset, Space.Self);
        }
    }
}
