using _Scripts.Runtime.Entities.Scripts.Combat;

namespace _Scripts.Runtime.Combat.Statuses.StatusEffects.OnAdd
{
    //generic buff add effect
    public class AddBuffStatusEffectSO : StatusEffectSO
    {
        public override void Execute(StatusContext sc)
        {
            Unit unit = sc.Applied.GetData();
            if (unit.Faction == Faction.Ally)
            {
                foreach (var sm in sc.StatusSOData.GetStatMultiplier(((AllyUnit)unit).CurrentSuit.GetClassType()))
                {
                    unit.MultiplyEStat(sm.stat, sm.multiplier);
                }
            }
            else
            {
                foreach (var sm in sc.StatusSOData.GenericStatsModified)
                    unit.MultiplyEStat(sm.stat, sm.multiplier);
            }

        }
    }
}