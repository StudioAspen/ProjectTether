namespace _Scripts.Runtime.Managers.Game_States
{
    public abstract class GameState
    {
        //GameManager performs once upon entering state 
        public abstract void Enter();
        
        //actions that will be looped by GameManager while in state 
        //honestly probably not going to be used 
        public abstract void Update();
        
        public void Exit()
        {
            OnExit();
        }
        
        //GameManager performs once before exiting state 
        protected abstract void OnExit(); 
    }
}