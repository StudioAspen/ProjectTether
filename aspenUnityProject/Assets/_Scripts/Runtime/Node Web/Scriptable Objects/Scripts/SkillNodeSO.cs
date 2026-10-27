using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Slinky.NodeWeb
{
    public class SkillNodeSO : BaseNodeSO
    {
        [field: SerializeField] public SkillSO Skill { get; private set; }

        public void SetSkillSO(SkillSO skill) => Skill = skill;

        public void SetSkillNodeActive()
        {
            Debug.Log("Function SetSkillNode is being called");

            if (PrerequisitesIDs.Count <= 0) return;

            bool isAllPrerequsiteNodesUnlocked = GenerateListOfPreresquiteNodes().All(node => node.IsUnlocked);

            if (isAllPrerequsiteNodesUnlocked)
            {
                this.SetIsUnlocked(true);
            }
            else
            {
                this.SetIsUnlocked(false);
            }
        }

        List<BaseNodeSO> GenerateListOfPreresquiteNodes()
        {
            List<BaseNodeSO> output = new();

            foreach (string id in PrerequisitesIDs)
            {
                output.Add(CurrentNodeWebGraphLocation.AllNodesDict[id]);
            }

            return output;
        }


    }
}
