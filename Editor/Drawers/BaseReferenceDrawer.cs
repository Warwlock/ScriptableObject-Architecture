using UnityEditor;
using UnityEngine;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomPropertyDrawer(typeof(BaseReference<,>), true)]
    public sealed class BaseReferenceDrawer : PropertyDrawer
    {
        private static readonly string[] popupOptions =
        {
            "Use Constant",
            "Use Variable"
        };

        private SerializedProperty useConstant;
        private SerializedProperty constantValue;
        private SerializedProperty variable;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            useConstant = property.FindPropertyRelative("_useConstant");
            constantValue = property.FindPropertyRelative("_constantValue");
            variable = property.FindPropertyRelative("_variable");
                        
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += Styles.PopupStyle.margin.top;
            buttonRect.width = Styles.PopupStyle.fixedWidth + Styles.PopupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, popupOptions, Styles.PopupStyle);

            useConstant.boolValue = result == 0;

            EditorGUI.PropertyField(position, 
                useConstant.boolValue ? constantValue : variable, 
                GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
            
            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        
        static class Styles
        {
            static Styles()
            {
                PopupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
                {
                    imagePosition = ImagePosition.ImageOnly,
                };
            }

            public static GUIStyle PopupStyle { get; set; }
        }
    }
}