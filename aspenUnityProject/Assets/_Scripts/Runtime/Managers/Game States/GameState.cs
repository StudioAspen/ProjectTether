using _Scripts.Runtime.Combat.States.ContextData;

namespace _Scripts.Runtime.Managers.Game_States
{
    public abstract class GameState
    {
        protected GameStateContext gameStateContext;

        protected GameState(GameStateContext gameStateContext)
        {
            this.gameStateContext = gameStateContext;
        }
        
        //GameManager performs once upon entering state 
        public abstract void Enter();
        
        //actions that will be looped by GameManager while in state 
        //honestly probably not going to be used 
        public abstract void Update();
        
        //GameManager performs once before exiting state 
        public abstract void Exit(); 
    }
}