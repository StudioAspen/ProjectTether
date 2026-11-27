using _Scripts.Runtime.Entities.Scripts.Combat;

namespace _Scripts.Runtime.Combat.Statuses.StatusEffects.OnRemove
{
    //generic buff removal effect
    public class RemoveBuffStatusEfffectSO : StatusEffectSO
    {
        public override void Execute(StatusContext sc)
        {
            Unit unit = sc.Applied.GetData();
            if (unit.Faction == Faction.Ally)
            {
                foreach (var sm in sc.StatusSOData.GetStatMultiplier(((AllyUnit)unit).CurrentSuit.GetClassType()))
                {
                    unit.RevertEStat(sm.stat, sm.multiplier);
                }
            }
            else
            {
                foreach (var sm in sc.StatusSOData.GenericStatsModified)
                    unit.RevertEStat(sm.stat, sm.multiplier);
            } 

        }
    }
}