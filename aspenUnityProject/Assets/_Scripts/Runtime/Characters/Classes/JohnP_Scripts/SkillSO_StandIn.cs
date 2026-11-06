using _Scripts.Runtime.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO_StandIn", menuName = "Scriptable Objects/Misc/Skill_StandIn")]
public class Skill : ScriptableObject
{
    [SerializeField]
    protected string name;

    [SerializeField]
    [Tooltip("Is this a class innate skill?")]
    protected bool _isClassInnate;
    public bool IsClassInnate => _isClassInnate;

    [SerializeField]
    [Tooltip("Does this consume an MSC slot?")]
    protected bool _consumesMscSlot;
    public bool ConsumesMSCSlot => _consumesMscSlot;

    [SerializeField]
    [Tooltip("Is this a transferable skill.")]
    protected bool _isTransferable;
    public bool IsTransferable => _isTransferable;

    [SerializeField]
    [Tooltip("Is this an enemy skill.")]
    protected bool _isEnemySkill;
    public bool IsEnemySkill => _isEnemySkill;

    [SerializeField]
    protected StatusSO[] _statusEffectsDueToAbility;
    public StatusSO[] StatusEffectsDueToAbility => _statusEffectsDueToAbility;

    [Tooltip("Percent change of the effect by the skill.")]
    [SerializeField] protected float[] _percentOfEffect;
    public float[] PercentofEffect => _percentOfEffect;

    [SerializeField] protected bool[] isStackable;
    public bool[] IsStackable => isStackable;

    [SerializeField]
    private string triggerCondition;
    
    [TextArea(3, 10)]
    [SerializeField]
    private string explanation;
}
