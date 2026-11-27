using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Math;
using UnityEngine;

namespace _Scripts.Runtime.Tile_System.Scripts
{
    public class TileController : MonoBehaviour
    {
        [SerializeField] private TerrainSO baseData;
        
        private Terrain terrainData;
        private const int MAX_UNITS = 4;
        private const float ArbitraryOffset = 5;

        [SerializeField, Range(0,18)]
        private int tileNum;

        public Vector3Int tileCoordinate;

        public List<Terrain> Terrains { get; private set; } = new List<Terrain>();
        
        //change to array if unit positions should not change on unit removal. Everything else so far has been an array because
        //I assumed early on that specific positions within the tile mattered (they don't currently) 
        public List<UnitController> UnitControllers { get; private set; }= new List<UnitController>();

        public void Initialize(TerrainSO baseData)
        {
            terrainData = new Terrain(baseData);
        }
            

        public bool AddUnit(UnitController unitController)
        {
            if (UnitControllers.Contains(unitController) || UnitControllers.Count >= MAX_UNITS)
                return false;
            UnitControllers.Add(unitController);
            foreach (var t in Terrains)
            {
                t.OnEnter(unitController);
            }
//            RepositionUnits(ArbitraryOffset);
            return true;
        }

        public UnitController GetUnitAt(int position)
        {
            if (UnitControllers.Count == 0 ||  position > UnitControllers.Count - 1)
                return null;
            
            return  UnitControllers[position];
        }

        public UnitController PeekUnit()
        {
            if (UnitControllers.Count < 1)
                return null;
            return UnitControllers[^1];
        }
        
        //returns deleted controller so the controller can be moved to a different TileController
        //by the CombatManager
        public UnitController RemoveUnit(UnitController unitController)
        {
            UnitController removed = unitController;
            UnitControllers.Remove(unitController);
            RepositionUnits();
            if (Terrains.Count < 1)
            {
                foreach (var t in Terrains)
                {
                    t.OnExit(unitController);
                }
            }

            return removed;
        }

        public Vector3 Position()
        {
            return transform.position; 
        }

        //. . . triangle box 
        public void RepositionUnits()
        {
            int count = UnitControllers.Count;
            if (count <= 1)
                return;
            for (int unit = 0; unit < count; unit++)
            {
                float angle = (2f * Mathf.PI * unit) / count;
                UnitControllers[unit].Move(transform.position + new Vector3(ArbitraryOffset * -Mathf.Cos(angle), 0, ArbitraryOffset* Mathf.Sin(angle)));
            }
        }

        /// <summary>
        /// 0-18
        /// </summary>
        /// <returns></returns>
        public int Num()
        {
            return tileNum;
        }

        public int UnitCount()
        {
            return UnitControllers.Count;
        }

        //currently, units can only move to adjacent tiles with the movement button
        public bool IsMoveable(Vector3Int from, int range, Faction faction)
        {
            if (tileCoordinate == from || UnitControllers.Count >= MAX_UNITS || from.HexGridDistance(tileCoordinate) > range)
                return false;
            foreach (UnitController unit in UnitControllers)
            {
                if (unit.GetData().Faction != faction)
                    return false;
            }
            return true; 
        }

        public void AddTerrain(TerrainContext TerrainData)
        {
            Terrain terrain = new Terrain(TerrainData);
            Terrains.Add(terrain);
        }
        

    }
}