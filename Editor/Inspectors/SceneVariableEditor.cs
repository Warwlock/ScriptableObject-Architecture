using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(SceneVariable))]
    internal class SceneVariableEditor : UnityEditor.Editor
    {
        private const string SCENE_NOT_ASSIGNED_WARNING = "Please assign a scene as the current serialized values for " +
                                             "the scene do not resolve to an asset in the project.";
        private const string SCENE_NOT_IN_BUILD_SETTINGS_WARNING =
            "Scene assigned is not currently in the Build Settings";
        private const string SCENE_NOT_ENABLED_IN_BUILD_SETTINGS_WARNING =
            "Scene assigned is present in build settings, but not enabled.";
        private const string SCENE_INFO_PROPERTY = "_value";

        private SceneVariable Target => (SceneVariable)target;
        private HelpBox _warningBox;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            _warningBox = new HelpBox("", HelpBoxMessageType.Warning);
            _warningBox.style.display = DisplayStyle.None;
            root.Add(_warningBox);

            SerializedProperty sceneInfoProperty = serializedObject.FindProperty(SCENE_INFO_PROPERTY);
            PropertyField sceneInfoField = new PropertyField(sceneInfoProperty);
            root.Add(sceneInfoField);

            UpdateWarningState();
            sceneInfoField.TrackPropertyValue(sceneInfoProperty, (sp) => UpdateWarningState());

            return root;
        }

        private void UpdateWarningState()
        {
            if (Target == null) return;

            if (Target.Value.Scene == null)
            {
                _warningBox.text = SCENE_NOT_ASSIGNED_WARNING;
                _warningBox.style.display = DisplayStyle.Flex;
            }
            else if (!Target.Value.IsSceneInBuildSettings)
            {
                _warningBox.text = SCENE_NOT_IN_BUILD_SETTINGS_WARNING;
                _warningBox.style.display = DisplayStyle.Flex;
            }
            else if (!Target.Value.IsSceneEnabled)
            {
                _warningBox.text = SCENE_NOT_ENABLED_IN_BUILD_SETTINGS_WARNING;
                _warningBox.style.display = DisplayStyle.Flex;
            }
            else
            {
                _warningBox.style.display = DisplayStyle.None;
            }
        }
    }
}