using _Scripts.Runtime.Combat;
using UnityEngine;

namespace Slinky.NodeWeb
{
    public class StatNodeSO : BaseNodeSO
    {
        [field: SerializeField] public StatSO Stat { get; private set; }

        public void SetStatSO(StatSO stat) => Stat = stat;
    }
}
