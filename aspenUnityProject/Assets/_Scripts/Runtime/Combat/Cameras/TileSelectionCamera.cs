using _Scripts.Runtime.Combat.States.CombatManagerStates;
using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Cameras
{
    public class TileSelectionCamera : MonoBehaviour
    {
        private CinemachineCamera virtualCamera;

        void Awake()
        {
            virtualCamera = GetComponent<CinemachineCamera>();
            virtualCamera.Priority = 1;
        }

        void OnEnable()
        {
            SelectTileState.TileSelectionStarted += ActivateCamera;
            SelectTileState.TileSelectionEnded += DeactivateCamera;
        }

        void OnDisable()
        {
            SelectTileState.TileSelectionStarted -= ActivateCamera;
            SelectTileState.TileSelectionEnded -= DeactivateCamera;
        }
  
        // Switch to the tile selection camera.
        void ActivateCamera()
        {
            virtualCamera.Priority = 3;
        }

        // Return control to the allied turn camera.
        void DeactivateCamera()
        {
            virtualCamera.Priority = 1;
        }
    }
}
