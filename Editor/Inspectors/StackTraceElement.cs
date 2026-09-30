using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjectArchitecture.Editor
{
    public class StackTraceElement : VisualElement
    {
        private IStackTraceObject _target;
        private ListView _listView;
        private Label _detailLabel;
        private VisualElement _contentContainer;

        public StackTraceElement(IStackTraceObject target, bool startCollapsed = false)
        {
            _target = target;

            // Box Styling
            style.marginTop = 15;
            style.borderTopWidth = 1;
            style.borderBottomWidth = 1;
            style.borderLeftWidth = 1;
            style.borderRightWidth = 1;
            style.borderTopColor = style.borderBottomColor = 
            style.borderLeftColor = style.borderRightColor = new Color(0.15f, 0.15f, 0.15f);

            // Header and Toolbar
            Toolbar toolbar = new Toolbar();

            ToolbarButton clearButton = new ToolbarButton(ClearStack) { text = "Clear" };
            clearButton.style.width = 45;

            ToolbarToggle collapseToggle = new ToolbarToggle() { value = startCollapsed, text = "Collapse" };
            collapseToggle.style.width = 65;
            collapseToggle.RegisterValueChangedCallback(evt => {
                _contentContainer.style.display = evt.newValue ? DisplayStyle.None : DisplayStyle.Flex;
            });

            Label title = new Label("Stack Trace");
            title.style.flexGrow = 1;
            title.style.unityTextAlign = TextAnchor.MiddleCenter;
            title.style.unityFontStyleAndWeight = FontStyle.Bold;

            toolbar.Add(clearButton);
            toolbar.Add(collapseToggle);
            toolbar.Add(title);
            Add(toolbar);

            // Content - Split View
            _contentContainer = new VisualElement();
            _contentContainer.style.height = 400; // Original default height
            _contentContainer.style.display = startCollapsed ? DisplayStyle.None : DisplayStyle.Flex;

            TwoPaneSplitView splitView = new TwoPaneSplitView(0, 200, TwoPaneSplitViewOrientation.Vertical);

            // Top Pane: Logs
            _listView = new ListView
            {
                itemsSource = _target.StackTraces,
                makeItem = () => new Label() { style = { paddingLeft = 4, paddingTop = 2, paddingBottom = 2 } },
                bindItem = (element, i) =>
                {
                    Label label = (Label)element;
                    label.text = GetFirstLine(_target.StackTraces[i].ToString());
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                }
            };

            _listView.selectionChanged += (selection) => {
                var selected = selection.FirstOrDefault();
                _detailLabel.text = selected != null ? selected.ToString() : string.Empty;
            };
            
            // Native alternating background colors
            _listView.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            _listView.selectionType = SelectionType.Single;

            // Bottom Pane: Log Details
            ScrollView detailScrollView = new ScrollView();
            detailScrollView.style.paddingTop = 5;
            detailScrollView.style.paddingLeft = 5;
            
            _detailLabel = new Label();
            _detailLabel.style.whiteSpace = WhiteSpace.Normal; // Allow text wrapping
            detailScrollView.Add(_detailLabel);

            splitView.Add(_listView);
            splitView.Add(detailScrollView);

            _contentContainer.Add(splitView);
            Add(_contentContainer);

            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent evt)
        {
            Debug.Log("Sub");
            if (_target?.StackTraces is StackTraceList stackList)
            {
                stackList.OnListChanged += OnListChanged;
            }
        }

        private void OnDetachFromPanel(DetachFromPanelEvent evt)
        {
            Debug.Log("UnSub");
            if (_target?.StackTraces is StackTraceList stackList)
            {
                stackList.OnListChanged -= OnListChanged;
            }
        }

        private void OnListChanged()
        {
            Debug.Log("Refresh");
            Refresh();
        }

        public void Refresh()
        {
            _listView.RefreshItems();
        }

        private void ClearStack()
        {
            _target.StackTraces.Clear();
            _detailLabel.text = string.Empty;
            _listView.SetSelection(-1);
            Refresh();
        }

        private string GetFirstLine(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return value.Split(new[] { '\r', '\n' }).FirstOrDefault();
        }
    }
}