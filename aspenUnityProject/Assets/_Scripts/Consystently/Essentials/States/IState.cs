namespace Consystently.Essentials
{
    //TODO: get rid of update later?
    public interface IState
    {
        public void Enter();
        public void Update();
        public void Exit();
    }
}