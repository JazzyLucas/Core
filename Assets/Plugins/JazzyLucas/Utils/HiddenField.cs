using UnityEngine;
using UnityEditor;

namespace JazzyLucas.Utils
{
    [System.Serializable]
    public class HiddenField
    {
        public bool dummy;
    }

    [CustomPropertyDrawer(typeof(HiddenField))]
    public class HiddenFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
        }
    }
}
