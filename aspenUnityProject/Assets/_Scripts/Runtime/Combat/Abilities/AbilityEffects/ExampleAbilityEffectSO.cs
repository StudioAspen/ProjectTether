using UnityEngine;

namespace _Scripts.Runtime.Combat.Abilities.AbilityEffects
{
    [CreateAssetMenu(fileName = "ExampleAbilityEffect" , menuName = "Scriptable Objects/Combat/Abilities/AbilityEffects/ExampleAbilityEffect")]
    public class ExampleAbilityEffectSO : AbilityEffectSO
    {
        
         
        /// <summary>
        /// example additional move effect that moves the unit to target 
        /// </summary>
        /// <param name="ab"></param>
        public override void Execute(AbilityContext ab)
        {
            ab.Caster.TryMove(ab.Target.Position(), ab.Target.tileCoordinate);
        }
    }
}