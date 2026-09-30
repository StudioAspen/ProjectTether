using UnityEngine;

namespace _Scripts.Runtime.Bootstrap
{
    /// <summary>
    /// Controls the Bootstrap scene, serving as the Single Entry Point manager.
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        private static Bootstrapper _instance;

        private const string BootstrapPrefabFileName = "Bootstrap";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoSpawn()
        {
            if (_instance != null) return;

            var prefab = Resources.Load<GameObject>(BootstrapPrefabFileName);
            Instantiate(prefab);
        }
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}