using System;
using Unity.VisualScripting;
using UnityEngine;

//this script must be attached to the object before converting it to a prefab and dragging it to a unit SO. 
namespace Tether.CharacterSystems
{
    public class EnemyUnitController : UnitController
    {
        private EnemyUnit stats;
        
        //encounters to be passed upon player collision 
        [SerializeField] private EncounterSO[] encounters; 
        
        //to be called by the combat manager. Will pass in encounter data and initialize the units. 
        public override void Initialize(UnitDataSO baseStats)
        {
            stats = new EnemyUnit(baseStats);
            Debug.Log($"{gameObject.name} initialized with stats instance ID: {stats.GetHashCode()}"); 
        }

        public override void TakeDamage(int damage)
        {
            
        }

        public override void TryMove(Vector3 position, Vector3Int coords)
        {
            SetTile(coords);
            MoveInvoke(position);
            HasMoved = true;
        }

        public override void MoveInvoke(Vector3 position)
        {
            Move(position);
            OnMove(this);
        }

        public override void Move(Vector3 position)
        {
           transform.position = position;
        }

        public override Unit GetData()
        {
            return stats; 
        }

        public override void ResetValues()
        {
            HasMoved = false;
            stats.EndDefend();
        }

 
        /*TODO:
            Add collision function wherein the enemy unit 
            calls EncounterManager.Instance.StartEncounter, passing a random encounter from encounters 
            I am not sure if events should be used for passing important data, so use a direct function
        */
    }
}