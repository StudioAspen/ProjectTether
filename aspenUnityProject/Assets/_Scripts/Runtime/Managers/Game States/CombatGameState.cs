using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class CombatGameState : GameState
    {
       public override void Enter()
       {
           InputManager.Instance.Actions.Player.Disable();
           InputManager.Instance.Actions.UI.Enable();
           InputManager.Instance.Actions.TileSelect.Enable();

           InputManager.Instance.LockCursor(false);
           
           GameManager.ChangeScene(GameManager.Instance.BattleScene.Name);
       }
       
       public override void Update()
       {
           
       }

       protected override void OnExit()
       {
           InputManager.Instance.Actions.TileSelect.Disable();
       }
    }
}