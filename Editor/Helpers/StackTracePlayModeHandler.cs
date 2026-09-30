using UnityEditor;

namespace ScriptableObjectArchitecture.Editor
{
    [InitializeOnLoad]
    public static class StackTracePlayModeHandler
    {
        static StackTracePlayModeHandler()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (SOArchitecturePreferences.IsClearStackTracesOnPlayMode
                    && state == PlayModeStateChange.ExitingEditMode)
                {
                    StackTraceList.ClearAllInstances();
                }
            };
        }
    }
}