namespace Consystently.Essentials
{
    public abstract class GameState : IState
    {
        protected GameManager gameManager;

        protected GameState(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }
        
        //GameManager performs once upon entering state 
        public abstract void Enter();
        
        //actions that will be looped by GameManager while in state 
        public abstract void Update();
        
        //GameManager performs once before exiting state 
        public abstract void Exit(); 
    }
}