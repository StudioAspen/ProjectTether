using System;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Overworld.Tools.Defibulator;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Overworld.Tools.Yoyo
{
    public class YoyoTool : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Required] private OverworldPlayerController _playerController;
        [SerializeField, Required] private YoyoProjectile _yoyoProjectilePrefab;
        [SerializeField, Required] private GameObject _targetIndicatorObject;
        [SerializeField, Required] private GameObject _lightChargeObject;

        [Header("Config")]
        [SerializeField] private float _maxDetectionRadius = 10f;
        [SerializeField] private float _rayDetectionRadius = 5f;
        [SerializeField] private float _extendingProjectileSpeed = 10f;
        [SerializeField] private Ease _extendingEase = Ease.OutCubic;
        [SerializeField] private float _retractingProjectileSpeed = 10f;
        [SerializeField] private Ease _retractingEase = Ease.OutCubic;
    
        [Header("Charge")]
        [SerializeField] private bool _hasCharge;
    
        public enum State
        {
            Ready,
            Extending,
            Retracting
        }
        [SerializeField, ReadOnly] private State _currentState = State.Ready;

        private YoyoProjectile _currentProjectile;
        private Renderer _renderer;
        private (PowerSource powerSource, Vector3 hitPoint)? _targetPowerSource;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            _targetIndicatorObject.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            InputManager.Instance.Actions.Player.PrimaryToolAction.started += Input_PrimaryToolActionStarted;
            InputManager.Instance.Actions.Player.PrimaryToolAction.canceled += Input_PrimaryToolActionCanceled;
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.Actions.Player.PrimaryToolAction.started -= Input_PrimaryToolActionStarted;
                InputManager.Instance.Actions.Player.PrimaryToolAction.canceled -= Input_PrimaryToolActionCanceled;
            }
        }
    
        private void Update()
        {
            UpdateState(_currentState);
        }

        private void FixedUpdate()
        {
            FixedUpdateState(_currentState);
        }

        private void LateUpdate()
        {
            UpdateTargetIndicator();
            UpdateLightCharge();
        }

        private void OnDrawGizmos()
        {
            if (_playerController == null)
            {
                Debug.LogWarning($"{GetType()} is missing reference to PlayerController");
                return;
            }

            if (_currentState != State.Ready)
                return;
        
            CapsuleDetection.DrawGizmos(
                transform.position,
                _playerController.transform.forward,
                _maxDetectionRadius,
                _rayDetectionRadius,
                Color.cyan
            );
        }

        private void ChangeState(State newState, bool forceChange = false)
        {
            if (!forceChange && _currentState == newState)
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
                    _renderer.enabled = true;
                    _playerController.OverrideWithCustomMovement(false);
                    _targetPowerSource = null;
                    break;
                case State.Extending:
                    _currentProjectile = Instantiate(_yoyoProjectilePrefab, transform.position, Quaternion.identity);
                    _currentProjectile.Init(this, _targetPowerSource.Value.hitPoint);
                    _currentProjectile.Extend(_extendingProjectileSpeed, _extendingEase);
                    break;
                case State.Retracting:
                    _currentProjectile.Retract(_retractingProjectileSpeed, _retractingEase);
                    break;
            }
        }

        private void UpdateState(State state)
        {
            switch (state) 
            {
                case State.Ready:
                    ScanForAimSurfaces();
                    break;
                case State.Extending:
                    break;
                case State.Retracting:
                    break;
            }
        }
    
        private void FixedUpdateState(State state)
        {
            switch (state) 
            {
                case State.Ready:
                    break;
                case State.Extending:
                    break;
                case State.Retracting:
                    break;
            }
        }

        private void OnStateExit(State state)
        {
            switch (state) 
            {
                case State.Ready:
                    _renderer.enabled = false;
                    _playerController.OverrideWithCustomMovement(true);
                    break;
                case State.Extending:
                    break;
                case State.Retracting:
                    break;
            }
        }

        private void Input_PrimaryToolActionStarted(InputAction.CallbackContext context)
        {
            if (_currentState != State.Ready)
                return;

            if (_targetPowerSource == null)
                return;
        
            ChangeState(State.Extending);
        }
    
        private void Input_PrimaryToolActionCanceled(InputAction.CallbackContext context)
        {
            if (_currentState != State.Extending)
                return;
        
            ChangeState(State.Retracting);
        }
    
        private void ScanForAimSurfaces()
        {
            CapsuleDetection.TryGetObjectInCapsule<PowerSource>(
                transform.position,
                _playerController.transform.forward,
                _maxDetectionRadius,
                _rayDetectionRadius,
                out _targetPowerSource
            );
        }

        public void OnProjectileDestinationReached()
        {
            if (_currentState != State.Extending)
                return;
        
            SwapCharge();
            ChangeState(State.Retracting);
        }

        public void OnRetractComplete()
        {
            if (_currentState != State.Retracting)
                return;
        
            ChangeState(State.Ready);
        }

        private void SwapCharge()
        {
            if (_targetPowerSource == null)
                return;
        
            PowerSource powerSource = _targetPowerSource.Value.powerSource;
            if (_hasCharge && !powerSource.IsActive)
            {
                _hasCharge = false;
                powerSource.ForceActivate();
            }
            else if (!_hasCharge && powerSource.IsActive)
            {
                _hasCharge = true;
                powerSource.Deactivate();
            }
        }
    
        private void UpdateTargetIndicator()
        {
            if (_currentState != State.Ready)
                return;
            
            if (_targetPowerSource == null)
            {
                _targetIndicatorObject.gameObject.SetActive(false);
                return;
            }
            
            _targetIndicatorObject.gameObject.SetActive(true);
            _targetIndicatorObject.transform.position = _targetPowerSource.Value.hitPoint;
        }

        private void UpdateLightCharge()
        {
            _lightChargeObject.SetActive(_hasCharge);
            if(_currentProjectile != null)
                _lightChargeObject.transform.position = _currentProjectile.transform.position;
        }
    }
}