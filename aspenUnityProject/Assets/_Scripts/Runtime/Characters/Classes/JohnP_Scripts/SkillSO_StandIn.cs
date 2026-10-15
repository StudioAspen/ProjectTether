using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO_StandIn", menuName = "Scriptable Objects/Misc/Skill_StandIn")]
public class Skill : ScriptableObject
{
    [SerializeField]
    private string name;

    [SerializeField]
    private string effect;

    [SerializeField]
    private string triggerCondition;
    
    [TextArea(3, 10)]
    [SerializeField]
    private string explanation;
}
