using UnityEngine;

[CreateAssetMenu(fileName = "QuirkSo_StandIn", menuName = "Scriptable Objects/Misc/QuirkSO_StandIn")]
public class QuirkSO_StandIn : ScriptableObject
{
    [SerializeField] private string nameOfTool;

    [SerializeField] private string nameOfBattlePassive;

    [SerializeField]
    [Tooltip("Effects due to Quirk. 0 = Attack , 1 = Dodge , 2 = Speed , 3 = Critical Chance , 4 = Critical Damage")]
    private int[] _battleEffectsDueToQuirk;
    public int[] BattleEffectsDueToQuirk => _battleEffectsDueToQuirk;

    [Tooltip("Percent change of the effect by the quirk.")]
    [SerializeField] private float[] _percentOfEffect;
    public float[] PercentofEffect => _percentOfEffect;

    [SerializeField]
    [TextArea(3 , 10)]
    private string effectExplained;
}
