using _Scripts.Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Overworld.Tools.Drone
{
    public class DroneTool : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Required] private Drone _dronePrefab;
        [SerializeField, Required] private Transform _droneSpawnTransform;
        [SerializeField, Required] private GameObject _targetIndicatorObject;

        [Header("Drone Config")]
        [SerializeField] private float _maxDetectionRadius = 10f;
        [SerializeField] private float _rayDetectionRadius = 5f;
        [SerializeField] private float _droneSpeed = 25f;
        [SerializeField] private float _droneLifetime = 3f;
        [SerializeField] private float _shootCooldown = 1f;

        public enum State
        {
            Ready,
            Shooting,
            Cooldown
        }

        [SerializeField, ReadOnly] private State _currentState = State.Ready;

        private DroneTargetable _target;
        private Vector3 _targetPoint;
        private Drone _currentDrone;
        private Renderer _renderer;
        private float _cooldownTimer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            if (_targetIndicatorObject != null)
                _targetIndicatorObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.Actions.Player.PrimaryToolAction.performed += Input_PrimaryToolActionPerformed;
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
                InputManager.Instance.Actions.Player.PrimaryToolAction.performed -= Input_PrimaryToolActionPerformed;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case State.Ready:
                    ScanForTargets();
                    break;
                case State.Cooldown:
                    _cooldownTimer += Time.deltaTime;
                    if (_cooldownTimer >= _shootCooldown)
                        ChangeState(State.Ready);
                    break;
            }
        }

        private void LateUpdate()
        {
            UpdateTargetIndicator();
        }

        private void OnDrawGizmos()
        {
            if (_droneSpawnTransform == null)
                return;

            CapsuleDetection.DrawGizmos(
                _droneSpawnTransform.position,
                transform.forward,
                _maxDetectionRadius,
                _rayDetectionRadius,
                Color.cyan
            );
        }

        private void Input_PrimaryToolActionPerformed(InputAction.CallbackContext context)
        {
            if (_currentState != State.Ready || _target == null)
                return;

            ChangeState(State.Shooting);
        }

        private void ChangeState(State newState)
        {
            if (_currentState == newState)
                return;

            OnStateExit(_currentState);
            _currentState = newState;
            OnStateEnter(_currentState);
        }

        private void OnStateEnter(State state)
        {
            switch (state)
            {
                case State.Ready:
                    _target = null;
                    if (_renderer != null)
                        _renderer.enabled = true;
                    break;
                case State.Shooting:
                    if (_renderer != null)
                        _renderer.enabled = false;
                    if (_target == null)
                    {
                        ChangeState(State.Cooldown);
                        return;
                    }

                    Vector3 direction = (_targetPoint - _droneSpawnTransform.position).normalized;
                    if (direction.sqrMagnitude < 0.0001f)
                        direction = transform.forward;

                    _currentDrone = Instantiate(_dronePrefab, _droneSpawnTransform.position,
                        Quaternion.LookRotation(direction, Vector3.up));
                    _currentDrone.Launch(this, direction, _droneSpeed, _droneLifetime);
                    break;
                case State.Cooldown:
                    _cooldownTimer = 0f;
                    break;
            }
        }

        private void OnStateExit(State state)
        {
            if (state == State.Ready && _targetIndicatorObject != null)
                _targetIndicatorObject.SetActive(false);
        }

        private void ScanForTargets()
        {
            if (_droneSpawnTransform == null)
                return;

            if (!CapsuleDetection.TryGetObjectInCapsule<DroneTargetable>(
                    _droneSpawnTransform.position,
                    transform.forward,
                    _maxDetectionRadius,
                    _rayDetectionRadius,
                    out var targetHit) || targetHit == null || targetHit.Value.obj == null)
            {
                _target = null;
                return;
            }

            _target = targetHit.Value.obj;
            _targetPoint = targetHit.Value.hitPoint;
        }

        private void UpdateTargetIndicator()
        {
            if (_targetIndicatorObject == null || _currentState != State.Ready)
                return;

            bool hasTarget = _target != null;
            _targetIndicatorObject.SetActive(hasTarget);
            if (hasTarget)
                _targetIndicatorObject.transform.position = _targetPoint;
        }

        public void OnDroneFinished(Drone drone)
        {
            if (_currentDrone != drone)
                return;

            _currentDrone = null;
            if (_currentState == State.Shooting)
                ChangeState(State.Cooldown);
        }
    }
}
