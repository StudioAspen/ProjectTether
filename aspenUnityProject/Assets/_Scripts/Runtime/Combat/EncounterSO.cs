using System;
using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

//misc directory is temp name? 
namespace _Scripts.Runtime.Combat
{
    [CreateAssetMenu(fileName = "NewEncounter", menuName = "Scriptable Objects/Misc/Encounter")]
    public class EncounterSO : ScriptableObject
    {
        public const int TILE_COUNT = 19;
        public const int TILE_MAX_UNITS = 4;

        [System.Serializable]
        public class UnitListWrapper
        {
            public List<UnitDataSO> Units = new();
        }
        
        [field: SerializeField] public List<UnitListWrapper> Tiles { get; private set; } = new List<UnitListWrapper>();
        
        private void OnValidate()
        {
            Validate();
        }

        public void Validate()
        {
            while (Tiles.Count < TILE_COUNT)
                Tiles.Add(new UnitListWrapper());
            
            if (Tiles.Count > TILE_COUNT)
                Tiles = Tiles.GetRange(0, TILE_COUNT);

            foreach (UnitListWrapper listWrapper in Tiles)
            {
                // Truncate the list if more than TILE_MAX_UNITS
                if (listWrapper.Units.Count > TILE_MAX_UNITS)
                    listWrapper.Units = listWrapper.Units.GetRange(0, TILE_MAX_UNITS);
            }
        }
        
        [Button("Clean Null Units")]
        public void CleanNullUnits()
        {
            foreach (UnitListWrapper listWrapper in Tiles)
            {
                // Remove null units starting from the end
                for (int i = listWrapper.Units.Count - 1; i >= 0; i--)
                {
                    if(listWrapper.Units[i] ==null)
                        listWrapper.Units.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Creates a runtime instance of this encounter with the same units, but can be modified
        /// </summary>
        public EncounterSO CreateRuntimeCopy()
        {
            EncounterSO copy = ScriptableObject.CreateInstance<EncounterSO>();
            copy.Tiles = new List<UnitListWrapper>(Tiles);
            return copy;
        }
        
        /// <summary>
        /// Creates a runtime instance of an encounter with empty data
        /// </summary>
        public static EncounterSO CreateEmptyRuntimeInstance()
        {
            EncounterSO instance = ScriptableObject.CreateInstance<EncounterSO>();
            instance.Validate();
            return instance;
        }
        
        /// <summary>
        /// Creates a Encounter data runtime instance from battle sim configuration
        /// </summary>
        public static EncounterSO CreateRuntimeInstanceFromBattleSim(List<BattlefieldTile> tiles)
        {
            EncounterSO encounterData = CreateEmptyRuntimeInstance();
           
            for (int tileIndex = 0; tileIndex < tiles.Count; tileIndex++)
            {
                if (tiles[tileIndex].FilledSlots.Count < 1)
                    continue;

                foreach (UnitPieceSlot unitSlot in tiles[tileIndex].FilledSlots)
                    encounterData.TryAddUnit(tileIndex, unitSlot.Piece.Sim.Data);
            }

            return encounterData;
        }

        public bool TryAddUnit(int tileIndex, UnitDataSO unitData)
        {
            if (tileIndex < 0 || tileIndex >= TILE_COUNT)
            {
                Debug.LogWarning($"Can't add unit in invalid tileIndex {tileIndex}");
                return false;
            }

            if (unitData == null)
            {
                Debug.LogWarning($"Can't add null unit at tileIndex {tileIndex}");
                return false;
            }

            if (Tiles[tileIndex].Units.Count >= TILE_MAX_UNITS)
            {
                Debug.LogWarning($"Can't add unit to full tile at tileIndex {tileIndex}");
                return false;
            }
            
            Tiles[tileIndex].Units.Add(unitData);
            return true;
        }

        public bool TryRemoveUnit(int tileIndex, UnitDataSO unitData)
        {
            if (tileIndex < 0 || tileIndex >= TILE_COUNT)
            {
                Debug.LogWarning($"Can't remove unit in invalid tileIndex {tileIndex}");
                return false;
            }

            if (unitData == null)
            {
                Debug.LogWarning($"Can't remove null unit at tileIndex {tileIndex}");
                return false;
            }

            if (!Tiles[tileIndex].Units.Contains(unitData))
            {
                Debug.LogWarning($"Can't remove nonexistent unit {unitData.name} at tileIndex {tileIndex}");
                return false;
            }
            
            Tiles[tileIndex].Units.Remove(unitData);
            return true;
        }
    }
}