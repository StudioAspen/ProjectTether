using _Scripts.Runtime.Managers;
using UnityEngine;

namespace _Scripts.Runtime.Combat.States
{
    public class MainMenuGameState : GameState
    {
        public MainMenuGameState(GameManager gameManager) : base(gameManager) { }

        //TODO:
        //implement main menu state transitions 
        public override void Enter()
        {
            Debug.Log("Main menu transitions are not ready, sorry!");
        }

        public override void Update()
        {
        }

        public override void Exit()
        {
            
        }
        
    }
}