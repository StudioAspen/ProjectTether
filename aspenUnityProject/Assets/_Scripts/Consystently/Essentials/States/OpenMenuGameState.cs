namespace Consystently.Essentials
{
    public class OpenMenuGameState : GameState
    {
        public OpenMenuGameState(GameManager gameManager) : base(gameManager) {}

        public override void Enter()
        {
           gameManager.PauseGame();
           gameManager.PushOldState();
        }
        public override void Update() {}

        public override void Exit()
        {
            //unpause
            gameManager.PauseGame();
        }
    }
}