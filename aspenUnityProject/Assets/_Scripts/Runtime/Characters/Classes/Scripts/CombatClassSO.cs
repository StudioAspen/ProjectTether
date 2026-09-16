using UnityEngine;

[CreateAssetMenu(fileName = "CombatClass", menuName = "Scriptable Objects/Misc/Combat Class")]
public class CombatClassSO : ScriptableObject
{
    
    //TODO: add stat modifier later 
    [SerializeField] private Element[] defaultAttackElements;
    public Element[] DefaultAttackElements => defaultAttackElements;

    [SerializeField] private string className;
    public string ClassName => className;

    //0 = cannot attack anything, 1 = 1 ring around unit, 3 = effectively global range for 19 tiles
    [SerializeField, Range(0,3)]
    private int defaultAttackRange;

    public int DefaultAttackRange => defaultAttackRange; 
    
    [SerializeField] private string classDescription;
    public string ClassDescription => classDescription;
    
}
