using UnityEngine;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using _Scripts.Runtime.Combat;
using System.Linq;
using System.Net;


namespace Slinky.NodeWeb
{
    [CreateAssetMenu(fileName = "New Node Web", menuName = "Scriptable Objects/Node Web Graph")]
    public class NodeWebGraphSO : ScriptableObject
    {
        [field: SerializeField] public List<BaseNodeSO> AllNodes { get; private set; } = new List<BaseNodeSO>();
        [field: SerializeField] public SerializedDictionary<string, BaseNodeSO> AllNodesDict { get; private set; } = new SerializedDictionary<string, BaseNodeSO>();

        public void AddNode(BaseNodeSO node) => AllNodes.Add(node);
        public void RemoveNode(BaseNodeSO node) => AllNodes.Remove(node);

        /*
         * TODO:
         * Create public helper functions that return a filter of different Node Types
         * I.E: List<AbilitySO> GetAllAbilitys(), List<StatSO> GetAllAStats(), List<SkillSO> GetAllSkills()
         */

        public List<AbilitySO> GetAbilities() => AllNodes.OfType<AbilityNodeSO>().Select(abilityNode => abilityNode.Ability).ToList();
        public List<AbilitySO> GetActiveAbilities()
        {
            List<AbilitySO> output = new();

            foreach (BaseNodeSO node in AllNodes)
            {
                if (node is AbilityNodeSO abilityNode && abilityNode.IsUnlocked) output.Add(abilityNode.Ability);
            }

            return output;
        }
    }
}
