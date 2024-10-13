using UnityEngine;
using UnityEditor;
using JazzyLucas.Core;
using Resources = JazzyLucas.Core.Resources;

namespace JazzyLucas.Core.Editor
{
    [CustomEditor(typeof(Resources), true)]
    public class ResourceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Properties"))
            {
                EditorUtility.OpenPropertyEditor(target);
            }

            base.OnInspectorGUI();
        }
    }
}
