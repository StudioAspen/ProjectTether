using System.Collections.Generic;
using Consystently.Essentials.Math;
using Tether.CharacterSystems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace TileSystem
{
    public class TileController : MonoBehaviour
    {
        private Tile tileData;
        private const int MaxUnits = 4;
        private const float ArbitraryOffset = 10;

        [SerializeField, Range(0,18)]
        private int tileNum;

        public Vector3Int tileCoordinate;
        
        //change to array if positions ever matter. Everything else so far has been an array because
        //I assumed early on that specific positions within the tile mattered (they don't currently) 
        public List<UnitController> UnitControllers { get; private set; }= new List<UnitController>();


        public void Initialize(TileSO baseData)
        {
            tileData = new Tile(baseData);
        }
            

        public bool AddUnit(UnitController unitController)
        {
            if (UnitControllers.Contains(unitController) || UnitControllers.Count >= MaxUnits)
                return false;
            UnitControllers.Add(unitController);
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
            RepositionUnits(ArbitraryOffset);
            return removed;
        }

        public Vector3 Position()
        {
            return transform.position; 
        }

        //. . . triangle box 
        public void RepositionUnits(float offset)
        {
            int count = UnitControllers.Count;
            if (count <= 1)
                return;
            for (int unit = 0; unit < count; unit++)
            {
                float angle = (2f * Mathf.PI * unit) / count;
                UnitControllers[unit].Move(transform.position + new Vector3(offset * -Mathf.Cos(angle), 0, offset * Mathf.Sin(angle)));
            }
        }
        

        public int Num()
        {
            return tileNum;
        }

        public int UnitCount()
        {
            return UnitControllers.Count;
        }

        //currently, units can only move to adjacent tiles 
        public bool IsMoveable(Vector3Int from, int range, Faction faction)
        {
            if (tileCoordinate == from || UnitControllers.Count >= MaxUnits || from.HexGridDistance(tileCoordinate) > range)
                return false;
            foreach (UnitController unit in UnitControllers)
            {
                if (unit.GetData().Faction != faction)
                    return false;
            }
            return true; 
        }

    }
}