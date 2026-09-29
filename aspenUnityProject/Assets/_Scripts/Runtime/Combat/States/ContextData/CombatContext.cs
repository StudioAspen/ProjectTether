using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using UnityEngine;

namespace _Scripts.Runtime.Combat.States.ContextData
{
    public class CombatContext
    {
        public Dictionary<Vector3Int, int> TileCubeCoords { get; private set; }
        public TileController[]  TileControllers { get; private set; }
        public InputSystem_Actions Input { get; private set; }
        public ICombatFunctionProvider CombatFunctionProvider { get; private set; }
        
        public List<AllyUnitController> AllyUnits { get; private set; }

        public CombatContext(TileController[] tileControllers,  Dictionary<Vector3Int, int> tileCubeCoords, InputSystem_Actions input, List<AllyUnitController> allyUnits, ICombatFunctionProvider combatFunctionProvider)
        {
           TileControllers = tileControllers;
           TileCubeCoords = tileCubeCoords;
           Input = input;
           AllyUnits = allyUnits;
           CombatFunctionProvider = combatFunctionProvider;
        }

       
    }
}