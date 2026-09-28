using _Scripts.Runtime.Combat.States.ContextData;

namespace _Scripts.Runtime.Combat.States
{
    //combat manager will get pools from level 
    //get inputActions that invoke using GameManager function for UImanager
    public class OverworldGameState : GameState
    {
       public OverworldGameState(GameStateContext gameStateContext) : base(gameStateContext) { }

       public override void Enter()
       {
           gameStateContext.PreviousStates.Push(this);
       }
       public override void Update()
       {}

       public override void Exit()
       {
           gameStateContext.PreviousStates.Pop();
       }
    }
}