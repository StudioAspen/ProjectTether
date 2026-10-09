using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Slinky.NodeWeb;

public class NodeWebEditorWindow : EditorWindow
{
    CustomGraphView _treeCanvas;
    [field: SerializeField] public static bool IsNodeWebGraphActive { get; private set; }
    [field: SerializeField] public static NodeWebGraphSO CurrentNodeWebGraph { get; private set; }

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
            Debug.Log($"Loaded a new node web: {evt.newValue}");
            
            _treeCanvas.PopulateView(evt.newValue as NodeWebGraphSO);
        });

        _treeCanvas = new CustomGraphView();
        root.Add(nodeWebGraphSelector);

        _treeCanvas.style.flexGrow = 1;
        root.Add(_treeCanvas);
    }
}
