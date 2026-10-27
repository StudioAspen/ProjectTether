using System;
using _Scripts.Runtime.Managers;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Overworld.Tools.Grapple
{
    public class GrappleTool : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Required] private OverworldPlayerController _playerController;
        [SerializeField, Required] private GrappleProjectile _projectilePrefab;
        [SerializeField, Required] private Transform _projectileSpawnTransform;
        [SerializeField, Required] private Transform _cameraTargetTransform;
        
        [Header("Aim Lookahead")]
        [SerializeField] private float _aimLookaheadOffset = 3f;
        [SerializeField] private float _aimLookaheadAnimDuration = 0.3f;
        [SerializeField] private Ease _aimLookaheadAnimEase = Ease.OutCubic;
        
        [Header("Grapple Config")]
        [SerializeField] private float _shootRange = 10f;
        [SerializeField] private float _projectileSpeed = 10f;
        [SerializeField] private float _pullSpeed = 10f;
        [SerializeField] private float _pullStopDistance = 1f;
        [SerializeField] private float _pullTimeoutDuration = 10f;
        [SerializeField] private float _shootCooldown = 1f;
        
        public enum State
        {
            Ready,
            Aiming,
            Shooting,
            Pulling,
            Cooldown
        }
        [SerializeField, ReadOnly] private State _currentState = State.Ready;
        
        private GrappleProjectile _currentProjectile;
        [SerializeField, ReadOnly] private float _pullDuration = 0f;
        [SerializeField, ReadOnly] private float _cooldownTimer = 0f;

        private void OnEnable()
        {
            InputManager.Instance.Actions.Player.PrimaryToolAction.performed += Input_PrimaryToolActionPerformed;
            
            InputManager.Instance.Actions.Player.SecondaryToolAction.started += Input_SecondaryToolActionStarted;
            InputManager.Instance.Actions.Player.SecondaryToolAction.canceled += Input_SecondaryToolActionCanceled;
        }

        private void OnDisable()
        {
            InputManager.Instance.Actions.Player.PrimaryToolAction.performed -= Input_PrimaryToolActionPerformed;
            
            InputManager.Instance.Actions.Player.SecondaryToolAction.started -= Input_SecondaryToolActionStarted;
            InputManager.Instance.Actions.Player.SecondaryToolAction.canceled -= Input_SecondaryToolActionCanceled;
        }

        private void Input_PrimaryToolActionPerformed(InputAction.CallbackContext context)
        {
            if (_currentState != State.Aiming)
                return;

            ChangeState(State.Shooting);
        }
        
        private void Input_SecondaryToolActionStarted(InputAction.CallbackContext context)
        {
            if (_currentState != State.Ready)
                return;
            
            ChangeState(State.Aiming);
        }
        
        private void Input_SecondaryToolActionCanceled(InputAction.CallbackContext context)
        {
            if (_currentState != State.Aiming)
                return;

            ChangeState(State.Ready);
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
                    break;
                case State.Aiming:
                    _cameraTargetTransform.DOKill();
                    _cameraTargetTransform.DOLocalMoveZ(_aimLookaheadOffset, _aimLookaheadAnimDuration)
                        .SetEase(_aimLookaheadAnimEase);
                    break;
                case State.Shooting:
                    _currentProjectile = Instantiate(_projectilePrefab, _projectileSpawnTransform.position, Quaternion.identity);
                    _currentProjectile.Shoot(this, transform.forward, _shootRange, _projectileSpeed);
                    break;
                case State.Pulling:
                    _playerController.OverrideWithCustomMovement(true);
                    _pullDuration = 0f;
                    break;
                case State.Cooldown:
                    if(_currentProjectile != null)
                        Destroy(_currentProjectile.gameObject);
                    _currentProjectile = null;
            
                    _cooldownTimer = 0f;
                    break;
            }
        }

        private void UpdateState(State state)
        {
            switch (state) 
            {
                case State.Ready:
                    break;
                case State.Aiming:
                    break;
                case State.Shooting:
                    break;
                case State.Pulling:
                    if (_currentProjectile == null)
                    {
                        ChangeState(State.Cooldown);
                        return;
                    }
                    
                    _pullDuration += Time.deltaTime;
                    if (_pullDuration >= _pullTimeoutDuration)
                    {
                        ChangeState(State.Cooldown);
                        return;
                    }

                    Vector3 direction = _currentProjectile.transform.position - _playerController.transform.position;
                    direction.y = 0f;
                    direction.Normalize();
            
                    _playerController.MoveWithVelocity(direction * _pullSpeed);

                    if (Vector3.SqrMagnitude(_playerController.transform.position - _currentProjectile.transform.position) <
                        _pullStopDistance * _pullStopDistance)
                    {
                        ChangeState(State.Cooldown);
                        return;
                    }
                    break;
                case State.Cooldown:
                    _cooldownTimer += Time.deltaTime;
                    if (_cooldownTimer >= _shootCooldown)
                    {
                        _cooldownTimer = 0f;
                        ChangeState(State.Ready);
                        return;
                    }
                    break;
            }
        }

        private void OnStateExit(State state)
        {
            switch (state) 
            {
                case State.Ready:
                    break;
                case State.Aiming:
                    _cameraTargetTransform.DOKill();
                    _cameraTargetTransform.DOLocalMove(Vector3.zero, _aimLookaheadAnimDuration).SetEase(_aimLookaheadAnimEase);
                    break;
                case State.Shooting:
                    break;
                case State.Pulling:
                    _playerController.OverrideWithCustomMovement(false);
                    break;
                case State.Cooldown:
                    break;
            }
        }

        public void OnProjectileCollideSuccess()
        {
            if (_currentState != State.Shooting)
                return;
            
            ChangeState(State.Pulling);
        }
        
        public void OnProjectileCollideFail()
        {
            if (_currentState != State.Shooting)
                return;

            ChangeState(State.Cooldown);
        }
        
        private void Update()
        {
            UpdateState(_currentState);
        }
    }
}