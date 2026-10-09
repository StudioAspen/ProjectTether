using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class MainMenuGameState : GameState
    {
        public MainMenuGameState(GameStateContext gameStateContext) : base(gameStateContext) { }

        //honestly not sure what's going to be here for the starting menu 
        protected override void OnEnter()
        {
            Debug.Log("Main menu transitions are not ready, sorry!");
        }

        public override void Update()
        {
        }

        protected override void OnExit()
        {
            
        }
        
    }
}