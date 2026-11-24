using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Abilities.AdditionalAbilityEffects
{
    [CreateAssetMenu(fileName = "DamageWStatus" , menuName = "Scriptable Objects/Combat/Abilities/AbilityEffects/DamageWStatusEffect")]
    public class DamageWStatusEffectSO :  AbilityEffectSO {
    
        public override void Execute(AbilityContext ab)
        {
            foreach (UnitController enemy in ab.Target.UnitControllers)
            {
                CombatFormulas.AbilityDamage(ab.Caster, enemy, ab.AbilityData);
                foreach (AbstractStatusSO i in ab.AbilityData.Status)
                {
                    enemy.GetData().AddStatus(i);
                }
            }
            if(ab.AbilityData.TileEffect != null)
                ab.Target.SetTerrain(ab.AbilityData.TileEffect); 
        }    
    }
}