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
        
        [field: SerializeField] public List<UnitListWrapper> Data { get; private set; } = new List<UnitListWrapper>();
        
        private void OnValidate()
        {
            while (Data.Count < TILE_COUNT)
                Data.Add(new UnitListWrapper());
            
            if (Data.Count > TILE_COUNT)
                Data = Data.GetRange(0, TILE_COUNT);

            foreach (UnitListWrapper listWrapper in Data)
            {
                // Truncate the list if more than TILE_MAX_UNITS
                if (listWrapper.Units.Count > TILE_MAX_UNITS)
                    listWrapper.Units = listWrapper.Units.GetRange(0, TILE_MAX_UNITS);
            }
        }
        
        [Button("Clean Null Units")]
        public void CleanNullUnits()
        {
            foreach (UnitListWrapper listWrapper in Data)
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
        /// <returns></returns>
        public EncounterSO CreateRuntimeCopy()
        {
            EncounterSO copy = ScriptableObject.CreateInstance<EncounterSO>();
            copy.Data = new List<UnitListWrapper>(Data);
            return copy;
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

            if (Data[tileIndex].Units.Count >= TILE_MAX_UNITS)
            {
                Debug.LogWarning($"Can't add unit to full tile at tileIndex {tileIndex}");
                return false;
            }
            
            Data[tileIndex].Units.Add(unitData);
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

            if (!Data[tileIndex].Units.Contains(unitData))
            {
                Debug.LogWarning($"Can't remove nonexistent unit {unitData.name} at tileIndex {tileIndex}");
                return false;
            }
            
            Data[tileIndex].Units.Remove(unitData);
            return true;
        }
    }
}