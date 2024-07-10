using UnityEngine;
using UnityEditor;
using JazzyLucas.Core;

namespace JazzyLucas.Editor
{
    [CustomEditor(typeof(Container), true)]
    public class ContainerEditor : UnityEditor.Editor
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