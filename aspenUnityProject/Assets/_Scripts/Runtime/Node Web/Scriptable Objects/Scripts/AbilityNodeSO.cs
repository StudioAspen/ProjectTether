using _Scripts.Runtime.Combat;
using UnityEngine;

namespace Slinky.NodeWeb
{
    public class AbilityNodeSO : BaseNodeSO
    {
        [field: SerializeField] public AbilitySO Ability { get; private set; }

        public void SetAbilitySO(AbilitySO ability) => Ability = ability;

    }
}