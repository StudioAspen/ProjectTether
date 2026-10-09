using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class OpenMenuGameState : GameState
    {
        public OpenMenuGameState(GameStateContext gameStateContext) : base(gameStateContext) {}

        protected override void OnEnter()
        {
            Time.timeScale = 0;
        }
        public override void Update() {}

        protected override void OnExit()
        {
            //unpause
            Time.timeScale = 1;
        }

   }
}