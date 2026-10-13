using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class CombatGameState : GameState
    {
       public override void Enter()
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