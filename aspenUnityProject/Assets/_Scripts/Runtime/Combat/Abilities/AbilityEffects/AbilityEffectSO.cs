using UnityEngine;

namespace _Scripts.Runtime.Combat.Abilities.AbilityEffects
{
    public abstract class AbilityEffectSO : ScriptableObject
    {
        public abstract void Execute(AbilityContext ab);

    }
}