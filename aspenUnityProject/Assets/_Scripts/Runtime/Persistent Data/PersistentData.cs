using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Persistent_Data
{
    public static class PersistentData
    {
        public static Encounter CurrentEncounterData { get; private set; } = new();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            CurrentEncounterData = new();
        }
        
        public static void SetCurrentEncounterData(Encounter encounterData) => CurrentEncounterData = encounterData;
    }
}