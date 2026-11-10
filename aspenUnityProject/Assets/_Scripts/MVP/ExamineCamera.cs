
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Tile_System.Scripts;
using Unity.Cinemachine;
using UnityEngine;

public class ExamineCamera : MonoBehaviour
{
    //for getting the cinemachine camera
    private CinemachineCamera virtualCamera;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
        virtualCamera.Priority = 0;
    }

    void OnEnable()
    {
        //checks when a unit or tile is examined
        CombatManager.examinedUnit += ExamineUnit;
        CombatManager.examinedTile += ExamineTile;
        CombatManager.exitedExamine += ExitExamine;
    }

    void OnDisable()
    {
        //stops checking when the script is disabled
        CombatManager.examinedUnit -= ExamineUnit;
        CombatManager.examinedTile -= ExamineTile;
        CombatManager.exitedExamine -= ExitExamine;

        if (virtualCamera != null)
            virtualCamera.Priority = 0;
    }

    //camera follows the unit being examined
    void ExamineUnit(UnitController unit)
    {
        if (unit == null)
            return;

        virtualCamera.Target.TrackingTarget = unit.transform;
        virtualCamera.Priority = 5;
    }

    //camera follows the tile being examined
    void ExamineTile(TileController tile)
    {
        if (tile == null)
            return;

        virtualCamera.Target.TrackingTarget = tile.transform;
        virtualCamera.Priority = 5;
    }

    //switches back to the normal camera
    void ExitExamine()
    {
        virtualCamera.Priority = 0;
    }
  
}
