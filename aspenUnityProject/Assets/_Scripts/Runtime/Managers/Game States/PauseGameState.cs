using _Scripts.Runtime.UI;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class PauseGameState : GameState
    {
        public override void Enter()
        {
            Time.timeScale = 0;
            
            Menu.Focus(PersistentMenuManager.Instance.PauseMenu);
        }

        public override void Update()
        {
            
        }

        protected override void OnExit()
        {
            //unpause
            Time.timeScale = 1;
        }

   }
}