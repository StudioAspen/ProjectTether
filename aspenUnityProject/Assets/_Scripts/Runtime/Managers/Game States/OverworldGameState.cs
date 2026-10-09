using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    //combat manager will get pools from level 
    //get inputActions that invoke using GameManager function for UImanager
    public class OverworldGameState : GameState
    {
       public OverworldGameState(GameStateContext gameStateContext) : base(gameStateContext) { }

       protected override void OnEnter()
       {
           Debug.Log("Entering Overworld Game State");
       }

       public override void Update()
       {
           
       }

       protected override void OnExit()
       {
           
       }
    }
}