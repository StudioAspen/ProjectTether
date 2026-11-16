using _Scripts.Runtime.Combat.States.CombatManagerStates;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Managers;
using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Cameras
{
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
}