namespace _Scripts.Runtime.Managers
{
    //probably remove this interface
    //TODO: get rid of update later?
    public interface IState
    {
        public void Enter();
        public void Update();
        public void Exit();
    }
}