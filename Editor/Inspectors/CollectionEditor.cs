using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(BaseCollection), true)]
    public class CollectionEditor : UnityEditor.Editor
    {
        private BaseCollection Target => (BaseCollection)target;

        private const string TITLE_FORMAT = "List ({0})";
        private const string LIST_PROPERTY_NAME = "_list";

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            SerializedProperty listProperty = serializedObject.FindProperty(LIST_PROPERTY_NAME);

            ListView listView = new ListView
            {
                headerTitle = string.Format(TITLE_FORMAT, Target.Type),
                showBorder = true,
                reorderable = true,
                showAddRemoveFooter = true,
                showFoldoutHeader = false,
                reorderMode = ListViewReorderMode.Animated,
                showAlternatingRowBackgrounds = AlternatingRowBackground.All,
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                makeItem = () =>
                    {
                        return new PropertyField();
                    },

                bindItem = (element, index) =>
                    {
                        SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(index);
                        ((PropertyField)element).BindProperty(elementProperty);
                    }
            };

            listView.BindProperty(listProperty);
            root.Add(listView);

            return root;
        }
    }
}