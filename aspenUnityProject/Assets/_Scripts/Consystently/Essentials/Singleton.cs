namespace Consystently.Essentials
{
  using UnityEngine;

  /// <summary>
  /// Class to derive from when a class should only contain a single instance, that is globally obtainable.
  /// </summary>
  /// <typeparam name="T">The derived class itself, to create the global instance reference.</typeparam>
  [DisallowMultipleComponent]
  public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
  {
    /// <summary>
    /// Set the instance, only if no instance existed previously.
    /// </summary>
    public static T Instance { get; protected set; }

    protected virtual void Awake() => HandleSingleton();

    private void HandleSingleton()
    {
      Instance ??= this as T;
      if (Instance == this)
        DontDestroyOnLoad(gameObject);
      else
        Destroy(gameObject);
    }
  }
}

