using System.Collections.Generic;
using _Scripts.Consystently.Essentials;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Save;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Runtime.Persistent_Data
{
    public class GameSession : Singleton<GameSession>
    {
        private const string PlayerDataSaveKey = "player_data";

        [field: SerializeField, ReadOnly] public PlayerData PlayerData { get; private set; }

        [field: SerializeField, ReadOnly] public EncounterSO CurrentEncounterData { get; private set; }

        [field: SerializeField, ReadOnly] public string CurrentOverworldEnemyId { get; private set; }

        [field: SerializeField, ReadOnly] public bool IsDesignerMode { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            Load();
        }

        private void Load()
        {
            if (SaveManager.GameStore == null)
            {
                Debug.LogWarning($"{nameof(GameSession)}: GameStore is unavailable; starting with fresh player data.", this);
                PlayerData = new PlayerData();
            }
            else
            {
                PlayerData = SaveManager.GameStore.GetObject(PlayerDataSaveKey, new PlayerData());
            }

            if (PlayerData == null)
            {
                Debug.LogWarning($"{nameof(GameSession)}: Loaded player data was null; starting with fresh player data.", this);
                PlayerData = new PlayerData();
            }

            if (PlayerData.DefeatedOverworldEnemyIds == null)
                PlayerData.DefeatedOverworldEnemyIds = new List<string>();

            CurrentEncounterData = null;
            CurrentOverworldEnemyId = null;
            IsDesignerMode = false;
        }

        public void Save()
        {
            if (PlayerData == null)
            {
                Debug.LogWarning($"{nameof(GameSession)}: Cannot save because PlayerData is null.", this);
                return;
            }

            if (SaveManager.GameStore == null)
            {
                Debug.LogWarning($"{nameof(GameSession)}: Cannot save because GameStore is unavailable.", this);
                return;
            }

            SaveManager.GameStore.SetObject(PlayerDataSaveKey, PlayerData);
        }

        public void SetCurrentEncounterData(EncounterSO encounterData)
        {
            if (encounterData == null)
                Debug.LogWarning($"{nameof(GameSession)}: Setting CurrentEncounterData to null.", this);

            CurrentEncounterData = encounterData;
        }

        public bool IsOverworldEnemyDefeated(string enemyId)
        {
            if (string.IsNullOrWhiteSpace(enemyId))
            {
                Debug.LogWarning($"{nameof(GameSession)}: Cannot check defeated state because the enemy ID is null or empty.", this);
                return false;
            }

            if (PlayerData?.DefeatedOverworldEnemyIds == null)
            {
                Debug.LogWarning($"{nameof(GameSession)}: Cannot check defeated state for '{enemyId}' because the defeated-ID list is unavailable.", this);
                return false;
            }

            return PlayerData.DefeatedOverworldEnemyIds.Contains(enemyId);
        }

        public void SetCurrentOverworldEnemyId(string enemyId)
        {
            if (string.IsNullOrWhiteSpace(enemyId))
            {
                Debug.LogWarning(
                    $"{nameof(GameSession)}: Cannot set the current overworld enemy ID because the supplied ID is null, empty, or whitespace. Existing value '{CurrentOverworldEnemyId ?? "<none>"}' was retained.",
                    this);
                return;
            }
            
            CurrentOverworldEnemyId = enemyId;
        }

        public void MarkCurrentOverworldEnemyDefeated()
        {
            if (string.IsNullOrEmpty(CurrentOverworldEnemyId))
                return;

            if (PlayerData == null)
            {
                Debug.LogWarning($"{nameof(GameSession)}: Cannot record defeated enemy '{CurrentOverworldEnemyId}' because PlayerData is null.", this);
                return;
            }

            if (PlayerData.DefeatedOverworldEnemyIds == null)
            {
                Debug.LogWarning($"{nameof(GameSession)}: Defeated enemy-ID list was null; recreating it before recording '{CurrentOverworldEnemyId}'.", this);
                PlayerData.DefeatedOverworldEnemyIds = new List<string>();
            }

            if (!PlayerData.DefeatedOverworldEnemyIds.Contains(CurrentOverworldEnemyId))
            {
                PlayerData.DefeatedOverworldEnemyIds.Add(CurrentOverworldEnemyId);
                Save();
            }

            CurrentOverworldEnemyId = null;
        }

        public void ClearCurrentOverworldEnemyId() => CurrentOverworldEnemyId = null;
        public void SetDesignerMode(bool isDesignerMode) => IsDesignerMode = isDesignerMode;
    }
}
