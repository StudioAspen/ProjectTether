using UnityEngine;

namespace _Scripts.Runtime.Managers.Game_States
{
    //combat manager will get pools from level 
    //get inputActions that invoke using GameManager function for UImanager
    public class OverworldGameState : GameState
    {
       public override void Enter()
       {
           GameManager.ChangeScene(GameManager.Instance.OverworldScene.Name);
       }

       public override void Update()
       {
           
       }

       protected override void OnExit()
       {
           
       }
    }
}