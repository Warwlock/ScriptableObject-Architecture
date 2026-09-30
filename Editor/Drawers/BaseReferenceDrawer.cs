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

            /// Hierarchy (Simplified):
            /// Foldout
            ///     Toggle
            ///         VisualElement (Contains our Label and Checkmark the arrow)
            ///     VisualElement (Foldout Container) ("root" points to this so we use "hierachy")
            ///     VisualElement (Our Custom UI)

            // UI Toolkit doesn't have BeginProperty()/EndProperty()
            // This is workaround I came up with
            Foldout root = new Foldout
            {
                text = property.displayName,
                bindingPath = property.propertyPath
            };
            root.AddToClassList("unity-base-field");
            root.style.flexDirection = FlexDirection.Row;

            var checkmark = root.Q<VisualElement>(className: Foldout.checkmarkUssClassName);
            if (checkmark != null)
            {
                checkmark.style.display = DisplayStyle.None;
            }

            Toggle toggle = root.Q<Toggle>();
            toggle.style.flexShrink = 0;
            toggle.style.marginRight = 0;
            toggle.style.marginLeft = 0;
            toggle.style.paddingLeft = 0;

            TextElement label = toggle.Q<TextElement>();
            label.AddToClassList("unity-base-field__label");
            label.AddToClassList("unity-property-field__label");

            VisualElement defaultContent = root.Q<VisualElement>(className: "unity-foldout__content");
            defaultContent.style.fontSize = 0;
            defaultContent.style.marginLeft = 0;
            // End of workaround

            VisualElement inputContainer = new VisualElement();
            inputContainer.AddToClassList("unity-base-field__input");
            inputContainer.style.flexDirection = FlexDirection.Row;
            
            // Dropdown list
            List<string> choices = new List<string> { "Use Variable", "Use Constant" };
            PopupField<string> popup = new PopupField<string>(choices, useConstant.boolValue ? 1 : 0);
            popup.style.width = 110;
            popup.style.flexShrink = 0;
            popup.style.marginRight = 4;

            // Variable and Constant value fields
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

            // This is required, we don't want to put our custom UI into the container, instead we put it into Foldout.
            root.hierarchy.Add(inputContainer);

            return root;
        }
    }
}