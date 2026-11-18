
using _Scripts.Consystently.UI;
using _Scripts.Runtime.Combat.States.CombatManagerStates;
using Unity.Cinemachine;
using UnityEngine;

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

        AbilitySelectionPanel.OnAbilityMenuClose += DeactivateCamera;
        AbilityButtonVE.OnAbilitySelectView += ActivateCamera;
    }

    void OnDisable()
    {
        SelectTileState.TileSelectionStarted -= ActivateCamera;
        SelectTileState.TileSelectionEnded -= DeactivateCamera;

        AbilitySelectionPanel.OnAbilityMenuClose -= DeactivateCamera;
        AbilityButtonVE.OnAbilitySelectView -= ActivateCamera;
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
