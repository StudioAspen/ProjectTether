using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;


namespace Slinky.NodeWeb
{
    [CreateAssetMenu(fileName = "New Node Web", menuName = "Node Web/Node Web Graph")]
    public class NodeWebGraphSO : ScriptableObject
    {
        [field: SerializeField] public List<BaseNodeSO> AllNodes { get; private set; } = new List<BaseNodeSO>();
        [field: SerializeField] public SerializedDictionary<string, BaseNodeSO> AllNodesDict { get; private set; } = new SerializedDictionary<string, BaseNodeSO>();

        public void AddNode(BaseNodeSO node) => AllNodes.Add(node);
        public void RemoveNode(BaseNodeSO node) => AllNodes.Remove(node);
    }
}
