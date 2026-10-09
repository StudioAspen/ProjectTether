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

        public void Enter()
        {
            gameStateContext.PreviousStates.Push(this);
            OnEnter();
        }
        
        //GameManager performs once upon entering state 
        protected abstract void OnEnter();
        
        //actions that will be looped by GameManager while in state 
        //honestly probably not going to be used 
        public abstract void Update();
        
        public void Exit()
        {
            gameStateContext.PreviousStates.Pop();
            OnExit();
        }
        
        //GameManager performs once before exiting state 
        protected abstract void OnExit(); 
    }
}