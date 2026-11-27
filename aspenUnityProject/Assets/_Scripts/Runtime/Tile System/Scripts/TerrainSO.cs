using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Tile_System.Scripts.TerrainEffects;
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
        public TerrainSO toCombine;
        
        /// <summary>
        /// resulting new terrain from combination
        /// </summary>
        public TerrainSO fromCombine; 
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

        [Tooltip("original+toCombine=fromCombine. Example: oil terrain + fire terrain = burning oil")] [SerializeField]
        private List<Combination> _combinations = new();
        public List<Combination> Combinations => _combinations;

        [SerializeField] private int _startingStacks;
        public int StartingStacks => _startingStacks;

        [SerializeField] private List<TerrainEffectSO> _enterEffects = new();
        public List<TerrainEffectSO> EnterEffects => _enterEffects;

        [SerializeField] private List<TerrainEffectSO> _standEffects = new();
        public List<TerrainEffectSO> StandEffects => _standEffects;

        [SerializeField] private List<TerrainEffectSO> _exitEffects = new();
        public List<TerrainEffectSO> ExitEffects => _exitEffects;

        [SerializeField] private List<TerrainEffectSO> _combineEffects = new();
        public List<TerrainEffectSO> CombineEffects => _combineEffects;

        [SerializeField] private List<TerrainEffectSO> _dissipateEffects = new();
        public List<TerrainEffectSO>  DissipateEFfects => _dissipateEffects;
        
        /// <summary>
        /// called whenever a terrain is set under the unit or when they enter.
        /// example: setting a portal terrain should teleport the unit instantly, so it warps with OnEnter. The ability
        /// that sets the portal terrain should not teleport the unit and instead leave it to the terrain
        /// if it creates a teleporting terrain
        /// </summary>
        /// <param name="context"></param>
        /// <param name="specificUnit"></param>
        public void OnEnter(TerrainContext context, params UnitController[] specificUnit)
        {
            if (_enterEffects == null)
                return;
            foreach (var effect in _enterEffects)
            {
                effect.Execute(context, specificUnit);
            }
        }

        /// <summary>
        /// called next turn when the unit ends their turn on the tile
        /// </summary>
        /// <param name="context"></param>
        public void OnStand(TerrainContext context)
        {
           if (_standEffects == null)
                return;
           foreach (var effect in _standEffects)
            {
                effect.Execute(context);
            }
        }

        /// <summary>
        /// Called on unit leaves the tile
        /// </summary>
        /// <param name="context"></param>
        /// <param name="specificUnit"></param>
        public void OnExit(TerrainContext context, params UnitController[] specificUnit)
        {
            if (_exitEffects == null)
                return;
            foreach (var effect in _exitEffects)
            {
                effect.Execute(context, specificUnit);
            }
        }
        

        /// <summary>
        /// called when A terrain is added to the tile while 
        /// </summary>
        /// <param name="context"></param>
        public void OnCombine(TerrainContext context)
        {
            if (_combineEffects == null)
                return;
            foreach (var effect in _combineEffects)
            {
                effect.Execute(context);
            }
        }

        /// <summary>
        /// called when the terrain effect goes away. 
        /// </summary>
        /// <param name="context"></param>
        public void OnDissipate(TerrainContext context)
        {
            if (_combineEffects == null)
                return;
            foreach (var effect in _dissipateEffects)
            {
                effect.Execute(context);
            }
        }

 
    }
}