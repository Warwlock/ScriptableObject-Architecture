using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(BaseVariable<>), true)]
    public class BaseVariableEditor : UnityEditor.Editor
    {
        private dynamic Target { get { return target; } }
        protected bool IsClampable
        {
            get
            {
                // Safety Check
                try { return Target.Clampable; }
                catch { return false; }
            }
        }

        private const string READONLY_TOOLTIP = "Should this value be changable during runtime? Will still be editable in the inspector regardless";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty valueProp = serializedObject.FindProperty("_value");
            SerializedProperty useDefaultProp = serializedObject.FindProperty("_useDefaultValue");
            SerializedProperty defaultValProp = serializedObject.FindProperty("_defaultValue");

            SerializedProperty isClampedProp = serializedObject.FindProperty("_isClamped");
            SerializedProperty minValProp = serializedObject.FindProperty("_minClampedValue");
            SerializedProperty maxValProp = serializedObject.FindProperty("_maxClampedValue");

            SerializedProperty readOnlyProp = serializedObject.FindProperty("_readOnly");
            SerializedProperty raiseWarningProp = serializedObject.FindProperty("_raiseWarning");

            root.Add(new PropertyField(valueProp));

            // Default Field and Container
            PropertyField useDefaultField = new PropertyField(useDefaultProp);
            root.Add(useDefaultField);

            VisualElement defaultContainer = new VisualElement();
            defaultContainer.style.marginLeft = 15;
            defaultContainer.style.display = useDefaultProp.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            defaultContainer.Add(new PropertyField(defaultValProp));
            root.Add(defaultContainer);

            // Toggle Visibility
            useDefaultField.TrackPropertyValue(useDefaultProp, prop => 
            {
                defaultContainer.style.display = prop.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            });

            root.Add(new VisualElement { style = { height = 15 } });

            // Read-Only Container
            VisualElement readOnlySection = new VisualElement();

            PropertyField readOnlyField = new PropertyField(readOnlyProp, "Read Only")
            {
                tooltip = READONLY_TOOLTIP
            };
            readOnlySection.Add(readOnlyField);

            VisualElement raiseWarningContainer = new VisualElement();
            raiseWarningContainer.style.marginLeft = 15;
            raiseWarningContainer.style.display = readOnlyProp.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            raiseWarningContainer.Add(new PropertyField(raiseWarningProp));
            readOnlySection.Add(raiseWarningContainer);

            // Show warning when read-only true
            readOnlyField.TrackPropertyValue(readOnlyProp, prop => 
            {
                raiseWarningContainer.style.display = prop.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            });

            if (IsClampable)
            {
                VisualElement clampSection = new VisualElement();

                PropertyField isClampedField = new PropertyField(isClampedProp);
                clampSection.Add(isClampedField);

                VisualElement minMaxContainer = new VisualElement();
                minMaxContainer.style.marginLeft = 15;
                minMaxContainer.style.display = isClampedProp.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
                
                minMaxContainer.Add(new PropertyField(minValProp));
                minMaxContainer.Add(new PropertyField(maxValProp));
                clampSection.Add(minMaxContainer);

                readOnlySection.style.display = isClampedProp.boolValue ? DisplayStyle.None : DisplayStyle.Flex;

                // Disable read-only, enable min-max
                isClampedField.TrackPropertyValue(isClampedProp, prop => 
                {
                    minMaxContainer.style.display = prop.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
                    readOnlySection.style.display = prop.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
                });

                root.Add(clampSection);
            }

            root.Add(readOnlySection);

            return root;
        }
    }
    [CustomEditor(typeof(BaseVariable<,>), true)]
    public class BaseVariableWithEventEditor : BaseVariableEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = base.CreateInspectorGUI();

            root.Add(new VisualElement { style = { height = 15 } });

            SerializedProperty eventProp = serializedObject.FindProperty("_event");
            root.Add(new PropertyField(eventProp));

            return root;
        }
    }
}