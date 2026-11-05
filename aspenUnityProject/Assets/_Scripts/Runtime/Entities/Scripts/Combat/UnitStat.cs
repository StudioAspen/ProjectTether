using System;
using _Scripts.Runtime.Combat;
using Unity.VisualScripting;

namespace _Scripts.Runtime.Entities.Scripts.Combat
{
    /// <summary>
    /// Stats should not be modified by statuses directly, as that
    /// would introduce stat drifting due to rounding. These stats will
    /// be used for damage calculation.
    /// </summary>
    public class UnitStat
    {
        public int BaseStat { get; private set; }
        public int EffectiveStat {get; private set;}
        
        public UnitStat(int stat)
        {
            BaseStat = stat;
            EffectiveStat = stat;
        }

        public void SetEffectiveStat(int value)
        {
            BaseStat = value;
        }

    }
}