using _Scripts.Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Runtime.Combat
{
    public class OverworldReturner : MonoBehaviour
    {
        [Button("Return to Overworld"), EnableIf("@UnityEngine.Application.isPlaying")]
        public void ReturnToOverworld()
        {
            GameManager.Instance.ChangeGameState(GameManager.State.Overworld);
            GameManager.ChangeScene(GameManager.Instance.OverworldScene.Name);
        }
    }
}