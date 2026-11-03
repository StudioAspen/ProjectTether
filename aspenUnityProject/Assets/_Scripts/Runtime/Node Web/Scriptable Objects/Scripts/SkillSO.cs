using UnityEngine;

public class SkillSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } 

    public void SetName(string name) => Name = name;

    [SerializeField]
    [Tooltip("Is this a class innate skill?")]
    private bool _isClassInnate;
    public bool IsClassInnate => _isClassInnate;

    [SerializeField]
    [Tooltip("Does this consume an MSC slot?")]
    private bool _consumesMscSlot;
    public bool ConsumesMSCSlot => _consumesMscSlot;

    [SerializeField]
    [Tooltip("Is this a transferable skill.")]
    private bool _isTransferable;
    public bool IsTransferable => _isTransferable;

    [SerializeField]
    [Tooltip("Stats raised by skill. 0 = Attack , 1 = Dodge , 2 = Speed , 3 = Critical Chance , 4 = Critical Damage")]
    private int[] _effectsDueToSkill;
    public int[] EffectsDueToSkill => _effectsDueToSkill;

    [Tooltip("Percent change of the effect by the skill.")]
    [SerializeField] private float[] _percentOfEffect;
    public float[] PercentofEffect => _percentOfEffect;

    [SerializeField] private bool[] isStackable;
    public bool[] IsStackable => isStackable;

    [SerializeField]
    private string triggerCondition;

    [TextArea(3, 10)]
    [SerializeField]
    private string explanation;
}
