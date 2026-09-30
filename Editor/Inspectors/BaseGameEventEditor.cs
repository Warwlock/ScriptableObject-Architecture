using UnityEditor;
using UnityEngine.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    public abstract class BaseGameEventEditor : UnityEditor.Editor
    {
        private IStackTraceObject Target { get { return (IStackTraceObject)target; } }
        private StackTraceElement _stackTraceElement;

        protected abstract void DrawRaiseButton(VisualElement root);

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            DrawRaiseButton(root);

            if (!SOArchitecturePreferences.IsDebugEnabled)
            {
                HelpBox helpBox = new HelpBox("Debug mode disabled\nStack traces will not be filed on raise!", HelpBoxMessageType.Warning);
                root.Add(helpBox);
            }

            _stackTraceElement = new StackTraceElement(Target);
            root.Add(_stackTraceElement);

            return root;
        }
    }
}