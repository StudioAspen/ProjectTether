using System;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts.Combat
{
    public abstract class UnitController : MonoBehaviour
    {
        //makes more sense to me to have movement-related data in the UnitController 
        //because the controller has access to the actual in-game positions 
        public bool HasMoved { get; protected set; }
        public Vector3Int TileCoords {get; private set;} 
        
       
        public abstract void Initialize(UnitDataSO baseStats);
        public abstract void TakeDamage(int damage);

        /// <summary>
        ///TryMove will change variables to determine other game logic
        /// </summary>
        /// <param name="position"></param>
        /// <param name="coords"></param>
        public abstract void TryMove(Vector3 position, Vector3Int coords);
        
        /// <summary>
        ///pure move function that will not trigger anything. 
        /// </summary>
        /// <param name="position"></param>
        //Used mainly for initialization
        public abstract void Move(Vector3 position);
        
       
        //I made this getter method really early on and don't feel like refactoring
        public abstract Unit GetData();

        public void SetTile(Vector3Int tileCubeCoord)
        { 
            TileCoords = tileCubeCoord;
        }

        //should reset values that should be upon turn change  (e.g., hasMoved)
        public abstract void ResetValues();
    }
}