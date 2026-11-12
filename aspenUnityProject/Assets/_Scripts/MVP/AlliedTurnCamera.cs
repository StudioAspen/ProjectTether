using UnityEngine;
using Unity.Cinemachine;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Combat.States.CombatManagerStates;

public class AlliedTurnCamera : MonoBehaviour
{
    private CinemachineCamera virtualCamera;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineCamera>();
    }

    void OnEnable()
    {
        CombatManager.battlePhaseChanged += OnBattlePhaseChanged;
    }

    void OnDisable()
    {
        CombatManager.battlePhaseChanged -= OnBattlePhaseChanged;
    }

    void OnBattlePhaseChanged(
        BattlePhase phase,
        UnitController currentUnit)
    {
        if (currentUnit == null)
            return;

        if (currentUnit is AllyUnitController)
        {
            virtualCamera.Target.TrackingTarget =
                currentUnit.transform;
        }
    }
}