using TMPro.EditorUtilities;
using UnityEngine;

public class Breacher : CombatClass
{
    public Breacher()
    {
        SeClassType(CombatClassType.Breacher);
    }

    public override void AddClassStatBuff(UnitDataSO stats)
    {
        base.AddClassStatBuff(stats);
        // int def = stats.Defense;
        // int res = stats.Resistance;

        // def += 2;
        // res += 2;

        // stats.SetDefense(def);
        // stats.SetResistance(res);
    }

    public override void RemoveClassStatBuff(UnitDataSO stats)
    {
        base.RemoveClassStatBuff(stats);
        // int def = stats.Defense;
        // int resist = stats.Resistance;

        // def -= 2;
        // resist -= 2;

        // stats.SetDefense(def);
        // stats.SetResistance(resist);
    }
}