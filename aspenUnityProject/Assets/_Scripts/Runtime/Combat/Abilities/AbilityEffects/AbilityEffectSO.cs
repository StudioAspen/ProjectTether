using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Abilities.AdditionalAbilityEffects
{
    public abstract class AbilityEffectSO : ScriptableObject
    {
        public abstract void Execute(AbilityContext ab);

    }
}