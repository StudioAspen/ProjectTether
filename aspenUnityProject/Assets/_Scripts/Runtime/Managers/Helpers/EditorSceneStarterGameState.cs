using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Runtime.Managers.Helpers
{
    /// <summary>
    /// So that we can start in any scene and our game manager knows what state to use
    /// </summary>
    public class EditorSceneStarterGameState : MonoBehaviour
    {
        [SerializeField] private GameManager.State _startingState;

#if UNITY_EDITOR
        private static string _editorStartingScenePath;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            _editorStartingScenePath = SceneManager.GetActiveScene().path;
        }
#endif

        private void Start()
        {
#if UNITY_EDITOR
            if (gameObject.scene.path != _editorStartingScenePath)
                return;

            Debug.LogWarning($"Editor scene started, automatically changing game state state to {_startingState}");
            GameManager.Instance.ChangeGameState(_startingState);
#endif
        }
    }
}