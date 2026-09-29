using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Combat.States.ContextData
{
    //exists because I do not want to pass the entire CombatManager
    public interface ICombatFunctionProvider
    {
        public TileController GetCurrTileController();
        public TileController GetTileController(Vector3Int to);
        public void SelectTile(InputAction.CallbackContext context);
        public void MoveTileSelector(CubeCoordDirections directions);
        public Vector3Int GetSelectedTile(); 
        
        public void RedoSelection();
        public UnitController GetCurrentUnit();
        public void ChangeTurn();
        public void ExitExamine();
        public void ExamineUnit(UnitController unit);



    }
}