using UnityEngine;
using System.Collections.Generic;


namespace Slinky.NodeWeb
{
    [CreateAssetMenu(fileName = "New Node Web", menuName = "Node Web/Node Web Graph")]
    public class NodeWebGraphSO : ScriptableObject
    {
        [field: SerializeField] public List<BaseNodeSO> AllNodes { get; private set; } = new List<BaseNodeSO>();

        public void AddNode(BaseNodeSO node) => AllNodes.Add(node);
        public void RemoveNode(BaseNodeSO node) => AllNodes.Remove(node);
    }
}
