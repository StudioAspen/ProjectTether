using _Scripts.Runtime.Combat.States.ContextData;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class CombatGameState : GameState
    {
       public CombatGameState (GameStateContext gameStateContext) : base(gameStateContext) { }
       
       protected override void OnEnter()
       {
           Debug.Log("combat game state entered");
           UnityEngine.SceneManagement.SceneManager.LoadScene("battleScene");
       }
       public override void Update()
       {
           
       }

       protected override void OnExit()
       {
           
       }
    }
}