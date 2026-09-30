using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomPropertyDrawer(typeof(SceneInfo))]
    internal sealed class SceneInfoPropertyDrawer : PropertyDrawer
    {
        private const string SCENE_PREVIEW_TITLE = "Preview (Read-Only)";
        private const string SCENE_NAME_PROPERTY = "_sceneName";
        private const string SCENE_INDEX_PROPERTY = "_sceneIndex";
        private const string SCENE_ENABLED_PROPERTY = "_isSceneEnabled";

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();

            SerializedProperty sceneNameProperty = property.FindPropertyRelative(SCENE_NAME_PROPERTY);
            SerializedProperty sceneIndexProperty = property.FindPropertyRelative(SCENE_INDEX_PROPERTY);
            SerializedProperty enabledProperty = property.FindPropertyRelative(SCENE_ENABLED_PROPERTY);

            // Scene Object Field
            ObjectField sceneAssetField = new ObjectField(property.displayName)
            {
                objectType = typeof(SceneAsset),
                allowSceneObjects = false
            };

            if (!string.IsNullOrEmpty(sceneNameProperty.stringValue))
            {
                sceneAssetField.SetValueWithoutNotify(AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneNameProperty.stringValue));
            }

            sceneAssetField.RegisterValueChangedCallback(evt =>
            {
                SceneAsset newAsset = evt.newValue as SceneAsset;
                string newPath = AssetDatabase.GetAssetPath(newAsset);

                sceneNameProperty.stringValue = newPath;

                if (string.IsNullOrEmpty(newPath))
                {
                    sceneIndexProperty.intValue = -1;
                    enabledProperty.boolValue = false;
                }

                property.serializedObject.ApplyModifiedProperties();
            });

            sceneAssetField.TrackPropertyValue(sceneNameProperty, prop =>
            {
                string currentPath = AssetDatabase.GetAssetPath(sceneAssetField.value);
                if (currentPath != prop.stringValue)
                {
                    sceneAssetField.SetValueWithoutNotify(AssetDatabase.LoadAssetAtPath<SceneAsset>(prop.stringValue));
                }
            });

            root.Add(sceneAssetField);

            // Read-Only Preview Section
            Label previewLabel = new Label(SCENE_PREVIEW_TITLE)
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            root.Add(previewLabel);

            VisualElement previewContainer = new VisualElement();
            previewContainer.SetEnabled(false);
            previewContainer.Add(new PropertyField(sceneNameProperty));
            previewContainer.Add(new PropertyField(sceneIndexProperty));
            previewContainer.Add(new PropertyField(enabledProperty));

            root.Add(previewContainer);

            return root;
        }
    }
}