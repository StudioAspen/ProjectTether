using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "Scriptable Objects/Misc/Skill")]
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
