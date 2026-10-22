using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Slinky.NodeWeb;

public class NodeWebEditorWindow : EditorWindow
{
    CustomGraphView _treeCanvas;
    NodeWebGraphSO _currentNodeWebGraph = null;

    [MenuItem("Window/Node Web Editor")]
    static void OpenWindow()
    {
        NodeWebEditorWindow window = GetWindow<NodeWebEditorWindow>();
        window.titleContent = new GUIContent("Node Web Editor");
    }

    private void CreateGUI()
    {
        var root = this.rootVisualElement;

        ObjectField nodeWebGraphSelector = new ObjectField("Active Node Web");
        nodeWebGraphSelector.objectType = typeof(NodeWebGraphSO);


        nodeWebGraphSelector.RegisterValueChangedCallback(evt =>
        {
            _currentNodeWebGraph = evt.newValue as NodeWebGraphSO ?? null;
            _treeCanvas.PopulateView(_currentNodeWebGraph);

        });

        _treeCanvas = new CustomGraphView();
        root.Add(nodeWebGraphSelector);

        _treeCanvas.style.flexGrow = 1;
        root.Add(_treeCanvas);
    }
}
