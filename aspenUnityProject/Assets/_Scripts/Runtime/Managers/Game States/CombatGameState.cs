using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    public class CombatGameState : GameState
    {
       public override void Enter()
       {
           GameManager.ChangeScene(GameManager.Instance.BattleScene.Name);
       }
       
       public override void Update()
       {
           
       }

       protected override void OnExit()
       {
           
       }
    }
}