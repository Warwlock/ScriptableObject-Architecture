using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Type = System.Type;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(GameEventBase<>), true)]
    public class TypedGameEventEditor : BaseGameEventEditor
    {
        private MethodInfo _raiseMethod;

        protected virtual void OnEnable()
        {
            _raiseMethod = target.GetType().BaseType.GetMethod("Raise", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
        }
        protected override void DrawRaiseButton(VisualElement root)
        {
            SerializedProperty property = serializedObject.FindProperty("_debugValue");

            PropertyField debugValueField = new PropertyField(property);
            root.Add(debugValueField);

            Button raiseButton = new Button(() =>
            {
                CallMethod(GetDebugValue(property));
            })
            {
                text = "Raise",
                style = { marginBottom = 10, height = 25 }
            };

            root.Add(raiseButton);

        }
        private object GetDebugValue(SerializedProperty property)
        {
            Type targetType = property.serializedObject.targetObject.GetType();
            FieldInfo targetField = targetType.GetField("_debugValue", BindingFlags.Instance | BindingFlags.NonPublic);

            return targetField.GetValue(property.serializedObject.targetObject);
        }

        private void CallMethod(object value)
        {
            _raiseMethod.Invoke(target, new object[] { value });
        }
    }
}