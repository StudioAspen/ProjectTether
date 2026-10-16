using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class MainMenuGameState : GameState
    {
        //honestly not sure what's going to be here for the starting menu 
        public override void Enter()
        {
            InputManager.Instance.Actions.Player.Disable();
            InputManager.Instance.Actions.UI.Enable();
            
            InputManager.Instance.LockCursor(false);
            
            GameManager.ChangeScene(GameManager.Instance.MainMenuScene.Name);
        }

        public override void Update()
        {
            
        }

        protected override void OnExit()
        {
            
        }
    }
}