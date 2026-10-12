using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Misc/Skill")]
public class Skill : ScriptableObject
{
    public string name;
    public string function;
    public string triggerCondition;
    public string explanation;
}
