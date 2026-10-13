using UnityEngine;

public class SkillSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } 

    public void SetName(string name) => Name = name;
}
