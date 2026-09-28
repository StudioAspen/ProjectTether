using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class OpenMenuGameState : GameState
    {
        public OpenMenuGameState(GameStateContext gameStateContext) : base(gameStateContext) {}

        public override void Enter()
        {
            Time.timeScale = 0;
        }
        public override void Update() {}

        public override void Exit()
        {
            //unpause
            Time.timeScale = 1;
        }

   }
}