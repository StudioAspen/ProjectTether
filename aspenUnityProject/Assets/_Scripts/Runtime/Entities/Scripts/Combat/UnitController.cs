using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts.Combat
{
    public abstract class UnitController : MonoBehaviour, IComparable<UnitController>
    {
       
        private Renderer _renderer;
        protected Unit stats { get; set; }

        void Start()
        {
           _renderer = GetComponent<Renderer>(); 
        }

        
        //makes more sense to me to have movement-related data in the UnitController 
        //because the controller has access to the actual in-game positions 
        public bool HasMoved { get; protected set; }
        public Vector3Int TileCoords {get; private set;} 
        
        public event Action<UnitController> OnDeath;
        
       
        public abstract void Initialize(UnitDataSO baseStats);

        //passing UnitController is more versatile
        /// <summary>
        /// invokes action upon death 
        /// </summary>
        /// <param name="damage"></param>
        public void TakeDamage(int damage)
        {
            if (stats.DecreaseHealthRemaining(damage))
            {
               if(stats.IsDead) 
                  OnDeath?.Invoke(this); 
            }
        }
        
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

        public void Hide()
        {
           _renderer.enabled = false; 
        }

        public void Show()
        {
           _renderer.enabled = true;
        }

        //should reset values that should be upon turn change  (e.g., hasMoved)
        /// <summary>
        /// Resets values that should be reset per turn like stunned bool (status effects are applied after) 
        /// </summary>
        public abstract void ResetValues();

        /// <summary>
        /// compares UnitController via Unit, which compares faction and level. Allies are placed before Enemies (higher prio in turns)
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(UnitController other)
        {
            if (other is null) return 1;
            return GetData().CompareTo(other.GetData()); 
        }
        
        
    }
}