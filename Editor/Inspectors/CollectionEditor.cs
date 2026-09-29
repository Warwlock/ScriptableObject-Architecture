using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(BaseCollection), true)]
    public class CollectionEditor : UnityEditor.Editor
    {
        private BaseCollection Target { get { return (BaseCollection)target; } }
        private SerializedProperty CollectionItemsProperty
        {
            get { return serializedObject.FindProperty(LIST_PROPERTY_NAME);}
        }

        private ReorderableList _reorderableList;
        private GUIContent _titleGUIContent;

        private const string TITLE_FORMAT = "List ({0})";
        private const string LIST_PROPERTY_NAME = "_list";

        private void OnEnable()
        {
            _titleGUIContent = new GUIContent(string.Format(TITLE_FORMAT, Target.Type));

            _reorderableList = new ReorderableList(
                serializedObject,
                CollectionItemsProperty)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement,
                //elementHeightCallback = GetHeight,
            };
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            serializedObject.Update();

            _reorderableList.DoLayoutList();

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, _titleGUIContent);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), 
                element
            );
        }

        /*private float GetHeight(int index)
        {
            SerializedProperty property = CollectionItemsProperty.GetArrayElementAtIndex(index);

            return GenericPropertyDrawer.GetHeight(property, Target.Type) + EditorGUIUtility.standardVerticalSpacing;
        }*/
    }
}