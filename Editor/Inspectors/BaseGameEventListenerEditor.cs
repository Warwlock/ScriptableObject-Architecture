using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    public abstract class BaseGameEventListenerEditor : UnityEditor.Editor
    {
        private IStackTraceObject Target { get { return (IStackTraceObject)target; } }
        private StackTraceElement _stackTraceElement;

        protected abstract void DrawRaiseButton(VisualElement container);

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            // Find properties
            SerializedProperty eventProp = serializedObject.FindProperty("_event");
            SerializedProperty responseProp = serializedObject.FindProperty("_response");
            SerializedProperty enableDebugProp = serializedObject.FindProperty("_enableGizmoDebugging");
            SerializedProperty debugColorProp = serializedObject.FindProperty("_debugColor");

            // Fields
            PropertyField eventField = new PropertyField(eventProp, "Event")
            {
                tooltip = "Event which will trigger the response"
            };
            root.Add(eventField);

            root.Add(new PropertyField(responseProp, "Response"));

            // Debugging
            Foldout debugFoldout = new Foldout
            {
                text = "Show Debug Fields",
                value = false
            };
            root.Add(debugFoldout);

            // Callback Debug Section
            Label callbackLabel = new Label("Callback Debugging");
            callbackLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            callbackLabel.style.marginTop = 10;
            callbackLabel.style.marginBottom = 5;
            debugFoldout.Add(callbackLabel);

            VisualElement callbackIndent = new VisualElement();
            callbackIndent.style.marginLeft = 15;
            debugFoldout.Add(callbackIndent);

            DrawRaiseButton(callbackIndent);

            _stackTraceElement = new StackTraceElement(Target, startCollapsed: true);
            callbackIndent.Add(_stackTraceElement);

            // Gizmo Debugging
            Label gizmoLabel = new Label("Gizmo Debugging");
            gizmoLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            gizmoLabel.style.marginTop = 20;
            gizmoLabel.style.marginBottom = 5;
            debugFoldout.Add(gizmoLabel);

            VisualElement gizmoIndent = new VisualElement();
            gizmoIndent.style.marginLeft = 15;
            debugFoldout.Add(gizmoIndent);

            PropertyField enableDebugField = new PropertyField(enableDebugProp, "Enable Gizmo Debugging");
            gizmoIndent.Add(enableDebugField);

            PropertyField debugColorField = new PropertyField(debugColorProp, "Debug Color")
            {
                tooltip = "Color used to draw debug gizmos in the scene"
            };
            gizmoIndent.Add(debugColorField);
            
            debugColorField.SetEnabled(enableDebugProp.boolValue);
            debugColorField.TrackPropertyValue(enableDebugProp, prop => 
            {
                debugColorField.SetEnabled(prop.boolValue); 
            });

            return root;
        }
    } 
}