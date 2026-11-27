using _Scripts.Runtime.Entities.Scripts.Combat;

namespace _Scripts.Runtime.Combat.Statuses.StatusEffects.OnTick
{
    public class GenericTickEffectSO : StatusEffectSO
    {
        public override void Execute(StatusContext sc)
        {
           CombatFormulas.StatusDamage(sc.StatusSOData.DamageTakenPerTurn, sc.StatusSOData.DamageTypes, sc.Applied); 
           //apply weaknesses and stuff here maybe  
        }

    }
}