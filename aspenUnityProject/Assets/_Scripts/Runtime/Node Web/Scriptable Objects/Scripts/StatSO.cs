using UnityEngine;

public class StatSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "Stat Name";
    public void SetName(string name) => Name = name;

}
