using System;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [Serializable]
    [MultiLine]
    public sealed class SceneInfo : ISerializationCallbackReceiver
    {
        /// <summary>
        /// Returns the fully-qualified name of the scene.
        /// </summary>
        public string SceneName => _sceneName;

        /// <summary>
        /// Returns the index of the scene in the build settings; if not present, -1 will be returned instead.
        /// </summary>
        public int SceneIndex
        {
            get =>_sceneIndex;
            internal set => _sceneIndex = value;
        }

        /// <summary>
        /// Returns true if the scene is present in the build settings, otherwise false.
        /// </summary>
        public bool IsSceneInBuildSettings =>_sceneIndex != -1;

        /// <summary>
        /// Returns true if the scene is enabled in the build settings, otherwise false.
        /// </summary>
        public bool IsSceneEnabled
        {
            get => _isSceneEnabled;
            internal set => _isSceneEnabled = value;
        }

#if UNITY_EDITOR
        public UnityEditor.SceneAsset Scene => UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(_sceneName);
#endif

        [SerializeField] private string _sceneName;

        [SerializeField] private int _sceneIndex;

        [SerializeField] private bool _isSceneEnabled;

        public SceneInfo() => _sceneIndex = -1;

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (Scene != null)
            {
                var sceneAssetPath = UnityEditor.AssetDatabase.GetAssetPath(Scene);
                var sceneAssetGUID = UnityEditor.AssetDatabase.AssetPathToGUID(sceneAssetPath);
                var scenes = UnityEditor.EditorBuildSettings.scenes;

                SceneIndex = -1;
                for (var i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].guid.ToString() == sceneAssetGUID)
                    {
                        SceneIndex = i;
                        IsSceneEnabled = scenes[i].enabled;
                        break;
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize() { }

        #endregion
    }
}