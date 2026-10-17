using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Persistent_Data
{
    public static class PersistentData
    {
        public static EncounterSO CurrentEncounterData { get; private set; }
        public static bool IsDesignerMode { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            CurrentEncounterData = null;
            IsDesignerMode = false;
        }
        
        public static void SetCurrentEncounterData(EncounterSO encounterData) => CurrentEncounterData = encounterData;
        public static void SetDesignerMode(bool isDesignerMode) => IsDesignerMode = isDesignerMode;
    }
}