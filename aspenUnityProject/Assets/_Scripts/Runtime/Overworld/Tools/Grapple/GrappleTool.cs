using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField, Required] private GameObject _lightIndicatorObject;
        
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
            Shooting,
            Pulling,
            Cooldown
        }
        [SerializeField, ReadOnly] private State _currentState = State.Ready;
        
        private Vector3? _targetProjectileDestination;
        private GrappleProjectile _currentProjectile;
        [SerializeField, ReadOnly] private float _pullDuration = 0f;
        [SerializeField, ReadOnly] private float _cooldownTimer = 0f;

        private void Start()
        {
            _lightIndicatorObject.SetActive(false);
        }

        private void OnEnable()
        {
            InputManager.Instance.Actions.Player.PrimaryToolAction.performed += Input_PrimaryToolActionPerformed;
        }

        private void OnDisable()
        {
            InputManager.Instance.Actions.Player.PrimaryToolAction.performed -= Input_PrimaryToolActionPerformed;
        }

        private void Input_PrimaryToolActionPerformed(InputAction.CallbackContext context)
        {
            if (_currentState != State.Ready)
                return;

            if (_targetProjectileDestination == null)
                return;

            ChangeState(State.Shooting);
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
                case State.Shooting:
                    _currentProjectile = Instantiate(_projectilePrefab, _projectileSpawnTransform.position, Quaternion.identity);
                    _currentProjectile.Shoot(this, _targetProjectileDestination.Value, _projectileSpeed);
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
                    ScanForAimSurfaces();
                    UpdateLightIndicator();
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
                    _lightIndicatorObject.SetActive(false);
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

        public void OnProjectileReachedDestination()
        {
            if (_currentState != State.Shooting)
                return;
            
            ChangeState(State.Pulling);
        }
        
        private void Update()
        {
            UpdateState(_currentState);
        }

        private void ScanForAimSurfaces()
        {
            List<Vector3> validHitPoints = new();
            
            RaycastHit[] hits = Physics.RaycastAll(_projectileSpawnTransform.position, transform.forward, _shootRange);
            foreach (RaycastHit hit in hits)
            {
                Grappleable grappleable = hit.collider.GetComponentInParent<Grappleable>();
                if (grappleable == null)
                    continue;
                
                validHitPoints.Add(hit.point);
            }

            if (validHitPoints.Count == 0)
            {
                _targetProjectileDestination = null;
                return;
            }
            
            // Get closest hitpoint
            float closestSqrMagnitude = float.MaxValue;
            Vector3? closestPoint = null;
            foreach (Vector3 point in validHitPoints)
            {
                float sqrMagnitude = (point - _playerController.transform.position).sqrMagnitude;
                if (sqrMagnitude < closestSqrMagnitude)
                {
                    closestSqrMagnitude = sqrMagnitude;
                    closestPoint = point;
                }
            }
            
            _targetProjectileDestination = closestPoint;
        }

        private void UpdateLightIndicator()
        {
            _lightIndicatorObject.SetActive(_targetProjectileDestination != null);
        }
    }
}