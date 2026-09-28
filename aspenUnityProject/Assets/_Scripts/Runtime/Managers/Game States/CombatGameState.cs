using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class CombatGameState : GameState
    {
       public CombatGameState (GameStateContext gameStateContext) : base(gameStateContext) { }
       
       public override void Enter()
       {
           Debug.Log("combat game state entered");
           gameStateContext.PreviousStates.Push(this);
           UnityEngine.SceneManagement.SceneManager.LoadScene("battleScene");
       }
       public override void Update()
       {
           
       }

       public override void Exit()
       {
           gameStateContext.PreviousStates.Pop();
       }
    }
}