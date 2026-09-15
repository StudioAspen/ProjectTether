using UnityEngine;
using UnityEngine.SceneManagement;

namespace Consystently.Essentials
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