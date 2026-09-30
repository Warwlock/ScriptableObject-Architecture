using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomPropertyDrawer(typeof(Vector4))]
    public class Vector4Drawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Vector4Field vector4Field = new Vector4Field(property.displayName);
            vector4Field.AddToClassList("unity-base-field__aligned");
            vector4Field.BindProperty(property);

            return vector4Field;
        }
    }
}
