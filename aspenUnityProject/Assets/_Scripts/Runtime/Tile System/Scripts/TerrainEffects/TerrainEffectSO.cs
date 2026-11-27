using UnityEngine;
using _Scripts.Runtime.Entities.Scripts.Combat;

namespace _Scripts.Runtime.Tile_System.Scripts.TerrainEffects
{
    public abstract class TerrainEffectSO : ScriptableObject
    {
        public abstract void Execute(TerrainContext context, params UnitController[] specificUnit);
    }
}