using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(BaseGameEventListener<,>), true)]
    public class GameEventListenerEditor : BaseGameEventListenerEditor
    {
        private MethodInfo _raiseMethod;

        protected void OnEnable()
        {
            _raiseMethod = target.GetType().BaseType.GetMethod("OnEventRaised");
        }
        protected override void DrawRaiseButton(VisualElement container)
        {
            Button raiseButton = new Button(() => 
            {
                _raiseMethod?.Invoke(target, null);
            })
            {
                text = "Raise",
                style = { marginBottom = 10, height = 25 }
            };

            container.Add(raiseButton);
        }
    } 
}