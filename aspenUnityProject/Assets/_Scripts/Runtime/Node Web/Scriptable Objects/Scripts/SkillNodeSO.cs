using UnityEngine;

namespace Slinky.NodeWeb
{
    public class SkillNodeSO : BaseNodeSO
    {
        [field: SerializeField] public SkillSO Skill { get; private set; }

        public void SetSkillSO(SkillSO skill) => Skill = skill;
    }
}
