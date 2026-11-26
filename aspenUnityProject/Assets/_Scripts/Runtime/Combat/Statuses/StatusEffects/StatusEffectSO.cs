using UnityEngine;

namespace _Scripts.Runtime.Combat.Statuses.StatusEffects
{
    public abstract class StatusEffectSO : ScriptableObject
    {
        public abstract void Execute(StatusContext sc);
    }
}