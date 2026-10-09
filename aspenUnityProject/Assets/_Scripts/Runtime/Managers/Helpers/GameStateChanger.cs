using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Helpers
{
    public class GameStateChanger : MonoBehaviour
    {
        [SerializeField] private bool _specifyState = true;
        [SerializeField, ShowIf("_specifyState")] private GameManager.State _targetState;

        public void ChangeState()
        {
            GameManager.Instance.ChangeGameState(_targetState);
        }

        public void ReturnToPreviousState()
        {
            GameManager.Instance.ReturnGameState();
        }
    }
}