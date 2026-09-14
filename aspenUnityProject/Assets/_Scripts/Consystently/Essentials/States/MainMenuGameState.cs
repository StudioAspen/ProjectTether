using System;
using UnityEngine;

namespace Consystently.Essentials
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