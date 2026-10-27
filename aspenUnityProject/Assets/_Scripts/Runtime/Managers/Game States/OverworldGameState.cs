using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    //combat manager will get pools from level 
    //get inputActions that invoke using GameManager function for UImanager
    public class OverworldGameState : GameState
    {
        public override void Enter()
        {
            InputManager.Instance.Actions.Player.Enable();
            InputManager.Instance.Actions.UI.Disable();

            InputManager.Instance.LockCursor(true);

            GameManager.ChangeScene(GameManager.Instance.OverworldScene.Name);
        }

        public override void Update()
        {

        }

        protected override void OnExit()
        {

        }
    }
}