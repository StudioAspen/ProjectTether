using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Persistent_Data
{
    public static class PersistentData
    {
        public static EncounterSO CurrentEncounterData { get; private set; } = new();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            CurrentEncounterData = null;
        }
        
        public static void SetCurrentEncounterData(EncounterSO encounterData) => CurrentEncounterData = encounterData;
    }
}