using UnityEngine;

namespace _Scripts.Runtime.Managers.Helpers
{
    public class GameManagerHelper : MonoBehaviour
    {
        public void QuitGame() => GameManager.QuitApplication();
        
        public void StartGame() => GameManager.Instance.StartGame();
        
        public void GotoMainMenu() => GameManager.Instance.GotoMainMenu();
    }
}