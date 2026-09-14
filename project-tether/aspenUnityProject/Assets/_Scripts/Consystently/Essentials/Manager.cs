namespace Consystently.Essentials
{
  public abstract class Manager<T> : Singleton<T> where T : Singleton<T>
  {
    protected override void Awake() => base.Awake();
  }
}
