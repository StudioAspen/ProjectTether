using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Tile_System.Scripts
{
    [System.Serializable]
    public struct Combination
    {
        /// <summary>
        /// terrain to be combined with 
        /// </summary>
        public AbstractTerrainSO toCombine;
        
        /// <summary>
        /// resulting new terrain from combination
        /// </summary>
        public AbstractTerrainSO fromCombine; 
    } 

    
    [CreateAssetMenu(fileName = "TerrainSO", menuName = "Scriptable Objects/Terrain/Terrain")]
    public class TerrainSO : ScriptableObject
    {
                [SerializeField] private string _id;
        public  string Id => _id;

        [Tooltip("for in-game menus")]
        [SerializeField] private string _name;
        public string Name => _name;

        [SerializeField] private string _description;
        public string Description => _description;
        
        [Tooltip("set to negative for healing over time instead of dmg")]
        [SerializeField] private int _damageOvertime;
        public int DamageOvertime => _damageOvertime;

        [SerializeField] private int _damageOnEnter;
        public int DamageOnEnter => _damageOnEnter;

        [SerializeField] private int _damageOnExit;
        public int DamageOnExit => _damageOnExit;

        [SerializeField] private bool _hasStatusOnEnter;
        public bool HasStatusOnEnter => _hasStatusOnEnter;
        
        [SerializeField] private bool _hasStatusOnExit;
        public bool HasStatusOnExit => _hasStatusOnExit;
        
        [Tooltip("statuses that the tile gives units when they step on it")]
        [SerializeField] private List<StatusSO> _statuses;
        public List<StatusSO> Statuses => _statuses;

        [Tooltip("terrain duration. Set to negative for infinite duration")]
        [SerializeField] private int _duration; 
        public int Duration => _duration;
        
        //TODO: replace this with what can actually change the tile texture
        [SerializeField] private Mesh _mesh;
        public Mesh Mesh => _mesh;

        [Tooltip("lava terrain = fire element, etc.")] 
        [SerializeField] private Element[] _damageTypes;
        public Element[] DamageTypes => _damageTypes;
        
        [Tooltip("original+toCombine=fromCombine. Example: oil terrain + fire terrain = burning oil")] 
        [SerializeField] private List<Combination> _combinations;
        public List<Combination> Combinations => _combinations;

        public virtual void OnEnter(UnitController unitController)
        {
            if (_hasStatusOnEnter)
            {
                foreach (StatusSO status in _statuses)
                {
                    unitController.GetData().AddStatus(status);
                }
            }
            CombatFormulas.TerrainDamage(DamageOnEnter, DamageTypes, unitController);
        }

        public virtual void Execute(UnitController unitController)
        {
            foreach (StatusSO status in _statuses)
            {
                unitController.GetData().AddStatus(status);
            }
            CombatFormulas.TerrainDamage(DamageOvertime, DamageTypes, unitController);
        }

        public virtual void OnExit(UnitController unitController)
        {
            if (_hasStatusOnExit)
            {
                foreach (StatusSO status in _statuses)
                {
                    unitController.GetData().AddStatus(status);
                }
            }
            CombatFormulas.TerrainDamage(DamageOnExit, DamageTypes, unitController); 
        }

 
    }
}