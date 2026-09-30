using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomPropertyDrawer(typeof(BaseReference<,>), true)]
    public sealed class BaseReferenceDrawer : PropertyDrawer
    {
        private SerializedProperty useConstant;
        private SerializedProperty constantValue;
        private SerializedProperty variable;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            useConstant = property.FindPropertyRelative("_useConstant");
            constantValue = property.FindPropertyRelative("_constantValue");
            variable = property.FindPropertyRelative("_variable");

            VisualElement root = new VisualElement();
            root.AddToClassList("unity-base-field");
            root.AddToClassList("unity-property-field");

            Label label = new Label(property.displayName);
            label.AddToClassList("unity-base-field__label");
            label.AddToClassList("unity-property-field__label");
            root.Add(label);

            VisualElement inputContainer = new VisualElement();
            inputContainer.AddToClassList("unity-base-field__input");
            inputContainer.style.flexDirection = FlexDirection.Row; // Align children horizontally
            root.Add(inputContainer);

            List<string> choices = new List<string> { "Use Variable", "Use Constant" };
            PopupField<string> popup = new PopupField<string>(choices, useConstant.boolValue ? 1 : 0);
            popup.style.width = 110;
            popup.style.flexShrink = 0;
            popup.style.marginRight = 4;

            PropertyField constantField = new PropertyField(constantValue, string.Empty);
            PropertyField variableField = new PropertyField(variable, string.Empty);
            
            constantField.style.flexGrow = 1;
            variableField.style.flexGrow = 1;

            void UpdateVisibility(bool isConstant)
            {
                constantField.style.display = isConstant ? DisplayStyle.Flex : DisplayStyle.None;
                variableField.style.display = isConstant ? DisplayStyle.None : DisplayStyle.Flex;
            }

            popup.RegisterValueChangedCallback(evt =>
            {
                bool isConstant = evt.newValue == choices[1];
                useConstant.boolValue = isConstant;
                property.serializedObject.ApplyModifiedProperties();
            });

            root.TrackPropertyValue(useConstant, prop =>
            {
                popup.value = prop.boolValue ? choices[1] : choices[0];
                UpdateVisibility(prop.boolValue);
            });

            UpdateVisibility(useConstant.boolValue);

            inputContainer.Add(popup);
            inputContainer.Add(constantField);
            inputContainer.Add(variableField);

            return root;
        }
    }
}