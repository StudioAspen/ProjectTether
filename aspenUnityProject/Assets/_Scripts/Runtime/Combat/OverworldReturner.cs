using _Scripts.Runtime.Managers;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Runtime.Combat
{
    public class OverworldReturner : MonoBehaviour
    {
        [Button("Return to Overworld", EButtonEnableMode.Playmode)]
        public void ReturnToOverworld()
        {
            GameManager.Instance.ChangeGameState(GameManager.State.Overworld);
        }
    }
}