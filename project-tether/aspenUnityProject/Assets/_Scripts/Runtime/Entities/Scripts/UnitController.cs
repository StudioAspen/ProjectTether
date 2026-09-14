using System;
using TileSystem;
using UnityEngine;

namespace Tether.CharacterSystems
{
    public abstract class UnitController : MonoBehaviour
    {
        //makes more sense to me to have movement-related data in the UnitController 
        //because the controller has access to the actual in-game positions 
        public bool HasMoved { get; protected set; }
        public Vector3Int TileCoords {get; private set;} 
        
        public event Action<UnitController> OnUnitMove; 
        
        public abstract void Initialize(UnitDataSO baseStats);
        public abstract void TakeDamage(int damage);

        //TryMove will change variables to determine other game logic
        public abstract void TryMove(Vector3 position, Vector3Int coords);
        
        //pure move function that will not trigger anything. 
        //Used mainly for initialization
        public abstract void Move(Vector3 position);
        
        //use if you want unit movement animation but don't want vars to be changed
        public abstract void MoveInvoke(Vector3 position);
        
        //possibly refactor and make the data public 
        public abstract Unit GetData();

        public void SetTile(Vector3Int tileCubeCoord)
        { 
            TileCoords = tileCubeCoord;
        }
        
        public void OnMove(UnitController unitController) { OnUnitMove?.Invoke(unitController); }

        //should reset values that should be upon turn change  (e.g., hasMoved)
        public abstract void ResetValues();
    }
}