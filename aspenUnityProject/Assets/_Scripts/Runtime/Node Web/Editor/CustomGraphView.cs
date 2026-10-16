using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using Slinky.NodeWeb;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.Animations;
using _Scripts.Runtime.Combat;
using Codice.CM.WorkspaceServer.DataStore;

namespace Slinky.NodeWeb
{
    public enum NodeTypes
    {
        AbilityNodeSO,
        SkillNodeSO,
        StatNodeSO
    }
}

public class CustomGraphView : GraphView
{
    NodeWebGraphSO _activeNodeWebGraph;
    bool _isPopulating = false;

    public CustomGraphView()
    {
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        GridBackground grid = new GridBackground();

        this.Insert(0, grid);
        grid.StretchToParentSize();


        this.RegisterCallback<ContextualMenuPopulateEvent>(BuildContextMenu);

        this.graphViewChanged += OnGraphViewChanged;
    }

    public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();

        foreach (Port targetPort in ports.ToList())
        {
            if (startAnchor == targetPort) continue;
            if (startAnchor.node == targetPort.node) continue;
            if (startAnchor.direction == targetPort.direction) continue;

            compatiblePorts.Add(targetPort);
        }

        return compatiblePorts;
    }

    GraphViewChange OnGraphViewChanged(GraphViewChange viewChange)
    {
        if (_isPopulating) return viewChange;

        // Connecting Nodes to each other LOGIC
        if (viewChange.edgesToCreate != null)
        {
            foreach (Edge edge in viewChange.edgesToCreate)
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;

                parentView.DataReference.UnlockIDs.Add(childView.DataReference.GUID);
                childView.DataReference.PrerequisitesIDs.Add(parentView.DataReference.GUID);

                Debug.Log($"Parent Node Children: {childView.DataReference.UnlockIDs.Count}");
            }
        }

        // Deleteing Nodes connection to each other
        if (viewChange.elementsToRemove != null)
        {
            foreach (GraphElement ele in viewChange.elementsToRemove)
            {
                if (ele is Edge deletedEdge)
                {
                    NodeView parentView = deletedEdge.output.node as NodeView;
                    NodeView childView = deletedEdge.input.node as NodeView;

                    parentView.DataReference.UnlockIDs.Remove(childView.DataReference.GUID);
                    childView.DataReference.PrerequisitesIDs.Remove(parentView.DataReference.GUID);

                    Debug.Log($"Parent Node Children: {childView.DataReference.UnlockIDs.Count}");
                }

                if (ele is NodeView deletedNode)
                {
                    _activeNodeWebGraph.AllNodes.Remove(deletedNode.DataReference);

                    foreach (BaseNodeSO remainingNode in _activeNodeWebGraph.AllNodes)
                    {
                        if (remainingNode.UnlockIDs.Contains(deletedNode.DataReference.GUID))
                        {
                            remainingNode.UnlockIDs.Remove(deletedNode.DataReference.GUID);
                            EditorUtility.SetDirty(remainingNode);
                        }
                    }

                    switch (deletedNode.DataReference)
                    {
                        case AbilityNodeSO abilityNode:
                            AbilitySO ability = abilityNode.Ability;
                            Undo.DestroyObjectImmediate(ability);
                            break;

                        case StatNodeSO statNode:
                            StatSO stat = statNode.Stat;
                            Undo.DestroyObjectImmediate(stat);
                            break;

                        case SkillNodeSO skillNode:
                            SkillSO skill = skillNode.Skill;
                            Undo.DestroyObjectImmediate(skill);
                            break;
                    }

                    Undo.DestroyObjectImmediate(deletedNode.DataReference);
                    EditorUtility.SetDirty(_activeNodeWebGraph);
                }
            }
        }

        if (_activeNodeWebGraph != null)
        {
            EditorUtility.SetDirty(_activeNodeWebGraph);
        }

        // If it is not returned, canvas will freeze
        return viewChange;
    }

    public void PopulateView(NodeWebGraphSO nodeWebGraphAssest)
    {
        this._activeNodeWebGraph = nodeWebGraphAssest;
        this._isPopulating = true;
        //this.DeleteElements(graphElements.ToList());
        // Deleting Elements
        var elements = graphElements.ToList();
        foreach (var element in elements)
        {
            if (element is NodeView || element is Edge)
            {
                this.RemoveElement(element);
            }
        }

        Dictionary<string, NodeView> visualNodeDict = new Dictionary<string, NodeView>();

        foreach (BaseNodeSO nodeData in nodeWebGraphAssest.AllNodes)
        {
            NodeView nodeView = new NodeView(nodeData);
            this.AddElement(nodeView);

            visualNodeDict.Add(nodeData.GUID, nodeView);
        }

        foreach (BaseNodeSO parentData in nodeWebGraphAssest.AllNodes)
        {
            foreach (string unlockGUID in parentData.UnlockIDs)
            {
                NodeView parentView = visualNodeDict[parentData.GUID];
                NodeView childView = visualNodeDict[unlockGUID];

                Edge visualEdge = new Edge();
                visualEdge.output = parentView.outputContainer[0] as Port;
                visualEdge.input = childView.inputContainer[0] as Port;

                visualEdge.output.Connect(visualEdge);
                visualEdge.input.Connect(visualEdge);
                this.AddElement(visualEdge);

            }
        }

        this._isPopulating = false;
    }

    void BuildContextMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Create Ability Node", action => CreateNode<AbilityNodeSO>(action.eventInfo.localMousePosition));
        evt.menu.AppendAction("Create Stat Node", action => CreateNode<StatNodeSO>(action.eventInfo.localMousePosition));
        evt.menu.AppendAction("Create Skill Node", action => CreateNode<SkillNodeSO>(action.eventInfo.localMousePosition));
    }

    void CreateNode<NodeType>(Vector2 mousePos) where NodeType : BaseNodeSO, new()
    {
        // Creating the Node Type as a scriptable object
        // For furture: modified the file name to include the name of the ability, stat or skill
        NodeType newNode = ScriptableObject.CreateInstance<NodeType>();
        string newGUID = System.Guid.NewGuid().ToString();

        // Setting info
        newNode.SetGUID(newGUID);
        newNode.SetEditorPosition(ConvertMousePositionToGraphSpace(mousePos));

        SaveNodeAsAsset(newNode, newGUID);
    }

    void SaveNodeAsAsset<NodeType>(NodeType node, string guid) where NodeType : BaseNodeSO
    {
        string fileName = "";
        string shortenGUID = guid.Substring(0, 5);

        switch (node)
        {
            case AbilityNodeSO abilityNode:
                fileName = $"AbilityNode_{shortenGUID}.asset";
                break;
            case StatNodeSO statNode:
                fileName = $"StatNode_{shortenGUID}.asset";
                break;
            case SkillNodeSO skillNode:
                fileName = $"skillNode_{shortenGUID}.asset";
                break;
        }

        string filePath = $"Assets/_Scripts/Runtime/Node Web/Scriptable Objects/Nodes/{fileName}";

        // Saving to folder physically
        AssetDatabase.CreateAsset(node, filePath);
        _activeNodeWebGraph.AddNode(node);
        CreateSOType(node, guid);

        NodeView nodeView = new NodeView(node);
        this.AddElement(nodeView);

        // Forces Unity to save files immediately
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void CreateSOType<NodeType>(NodeType node, string guid) where NodeType : BaseNodeSO
    {
        switch (node)
        {
            case AbilityNodeSO abilityNodeSO:
                AbilitySO abilitySO = ScriptableObject.CreateInstance<AbilitySO>();
                abilityNodeSO.SetAbilitySO(abilitySO);

                string fileName = $"Ability_{guid}.asset";
                string filePath = $"Assets/_Scripts/Runtime/Node Web/Scriptable Objects/Abilitys/{fileName}";

                AssetDatabase.CreateAsset(abilitySO, filePath);
                break;
            case SkillNodeSO skillNodeSO:
                SkillSO skillSO = ScriptableObject.CreateInstance<SkillSO>();
                skillNodeSO.SetSkillSO(skillSO);

                string fileName2 = $"Skill_{guid}.asset";
                string filePath2 = $"Assets/_Scripts/Runtime/Node Web/Scriptable Objects/Skills/{fileName2}";
                AssetDatabase.CreateAsset(skillSO, filePath2);
                break;
            case StatNodeSO statNodeSO:
                StatSO statSO = ScriptableObject.CreateInstance<StatSO>();
                statNodeSO.SetStatSO(statSO);

                string fileName3 = $"Stat_{guid}.asset";
                string filePath3 = $"Assets/_Scripts/Runtime/Node Web/Scriptable Objects/Stats/{fileName3}";
                AssetDatabase.CreateAsset(statSO, filePath3);
                break;
        }


    }

    Vector2 ConvertMousePositionToGraphSpace(Vector2 mousePos) => this.contentViewContainer.WorldToLocal(mousePos);
}

