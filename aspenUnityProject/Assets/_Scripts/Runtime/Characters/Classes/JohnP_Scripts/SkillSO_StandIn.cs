using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO_StandIn", menuName = "Scriptable Objects/Misc/Skill_StandIn")]
public class Skill : ScriptableObject
{
    [SerializeField]
    private string name;

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
    private BattleParametersList.BattleParameters[] _effectsDueToAbility;
    public BattleParametersList.BattleParameters[] EffectsDueToAbility => _effectsDueToAbility;

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
