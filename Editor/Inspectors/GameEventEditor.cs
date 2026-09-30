using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(GameEventBase), true)]
    public sealed class GameEventEditor : BaseGameEventEditor
    {
        private GameEvent Target { get { return (GameEvent)target; } }

        protected override void DrawRaiseButton(VisualElement root)
        {
            Button raiseButton = new Button(() => Target.Raise())
            {
                text = "Raise",
                style = { marginBottom = 10, height = 25 }
            };

            root.Add(raiseButton);
        }
    } 
}