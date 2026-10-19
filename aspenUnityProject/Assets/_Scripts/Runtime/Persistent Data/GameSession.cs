using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Save;
using UnityEngine;

namespace _Scripts.Runtime.Persistent_Data
{
    public static class GameSession
    {
        public static PlayerData PlayerData { get; private set; }
        private const string PlayerDataSaveKey = "player_data";
        
        public static EncounterSO CurrentEncounterData { get; private set; }
        
        public static bool IsDesignerMode { get; private set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Load()
        {
            PlayerData = SaveManager.GameStore.GetObject(
                PlayerDataSaveKey, new PlayerData());
            
            CurrentEncounterData = null;
            IsDesignerMode = false;
        }

        public static void Save()
        {
            if (PlayerData != null)
                SaveManager.GameStore.SetObject(PlayerDataSaveKey, PlayerData);
        }
        
        public static void SetCurrentEncounterData(EncounterSO encounterData) => CurrentEncounterData = encounterData;
        public static void SetDesignerMode(bool isDesignerMode) => IsDesignerMode = isDesignerMode;
    }
}
