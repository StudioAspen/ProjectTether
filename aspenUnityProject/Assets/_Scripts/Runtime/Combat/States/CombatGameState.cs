using _Scripts.Runtime.Managers;
using UnityEngine;

namespace _Scripts.Runtime.Combat.States
{
    public class CombatGameState : GameState
    {
       public CombatGameState (GameManager gameManager) : base(gameManager) { }
       public override void Enter()
       {
           Debug.Log("combat game state entered");
           UnityEngine.SceneManagement.SceneManager.LoadScene("battleScene");
       }
       public override void Update()
       {
           
       }

       public override void Exit()
       {
       }
    }
}