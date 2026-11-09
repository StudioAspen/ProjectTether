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
        [SerializeField, Required] private GameObject _targetIndicatorObject;
        
        [Header("Grapple Config")]
        [SerializeField] private float _maxDetectionRadius = 10f;
        [SerializeField] private float _rayDetectionRadius = 5f;
        [SerializeField] private float _projectileSpeed = 10f;
        [SerializeField] private float _pullToObjectSpeed = 20f;
        [SerializeField] private float _pullToPlayerSpeed = 20f;
        [SerializeField] private float _pullStopDistance = 1f;
        [SerializeField] private float _pullTimeoutDuration = 10f;
        [SerializeField] private float _shootCooldown = 1f;
        
        public enum State
        {
            Ready,
            Shooting,
            PullingUserToObject,
            PullingObjectToUser,
            Cooldown
        }
        [SerializeField, ReadOnly] private State _currentState = State.Ready;
        
        private (Grappleable targetGrappleable, Vector3 targetProjectileDestination)? _targetGrappleObject;
        private GrappleProjectile _currentProjectile;
        [SerializeField, ReadOnly] private float _pullDuration = 0f;
        [SerializeField, ReadOnly] private float _cooldownTimer = 0f;

        private void Start()
        {
            _targetIndicatorObject.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            InputManager.Instance.Actions.Player.PrimaryToolAction.performed += Input_PrimaryToolActionPerformed;
        }

        private void OnDisable()
        {
            if(InputManager.Instance != null)
                InputManager.Instance.Actions.Player.PrimaryToolAction.performed -= Input_PrimaryToolActionPerformed;
        }
        
        private void Update()
        {
            UpdateState(_currentState);
        }

        private void FixedUpdate()
        {
            FixedUpdateState(_currentState);
        }
        
        private void OnDrawGizmos()
        {
            if (_projectileSpawnTransform == null)
                return;

            Vector3 direction = transform.forward.normalized;
            float radius = Mathf.Max(0f, _rayDetectionRadius);
            Vector3 spawnPosition = _projectileSpawnTransform.position;
            Vector3 origin = spawnPosition + direction * radius;
            float distance = Mathf.Max(0f, _maxDetectionRadius);
            Vector3 end = origin + direction * distance;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(spawnPosition, origin);
            Gizmos.DrawWireSphere(origin, radius);
            Gizmos.DrawWireSphere(end, radius);
            Gizmos.DrawLine(origin, end);

            // Draw rails around the sphere's path.
            Vector3 right = Vector3.Cross(direction, Vector3.up);
            if (right.sqrMagnitude < 0.001f)
                right = Vector3.Cross(direction, Vector3.right);

            right.Normalize();
            Vector3 up = Vector3.Cross(right, direction).normalized;

            const int railCount = 8;
            for (int i = 0; i < railCount; i++)
            {
                float angle = i * Mathf.PI * 2f / railCount;
                Vector3 offset = (right * Mathf.Cos(angle) + up * Mathf.Sin(angle)) * radius;

                Gizmos.DrawLine(origin + offset, end + offset);
            }
        }

        private void Input_PrimaryToolActionPerformed(InputAction.CallbackContext context)
        {
            if (_currentState != State.Ready)
                return;

            if (_targetGrappleObject == null)
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
                    _currentProjectile.Shoot(this, _targetGrappleObject.Value.targetGrappleable, _targetGrappleObject.Value.targetProjectileDestination, _projectileSpeed);
                    break;
                case State.PullingUserToObject:
                    _playerController.OverrideWithCustomMovement(true);
                    _pullDuration = 0f;
                    break;
                case State.PullingObjectToUser:
                    _targetGrappleObject.Value.targetGrappleable.RigidBody.isKinematic = true;
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
                case State.PullingUserToObject:
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

                    Vector3 directionToObject = _currentProjectile.transform.position - _playerController.transform.position;
                    directionToObject.Normalize();
            
                    _playerController.MoveWithVelocity(directionToObject * _pullToObjectSpeed);

                    if (Vector3.SqrMagnitude(_playerController.transform.position - _currentProjectile.transform.position) <
                        _pullStopDistance * _pullStopDistance)
                    {
                        ChangeState(State.Cooldown);
                        return;
                    }
                    break;
                case State.PullingObjectToUser:
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
        
        private void FixedUpdateState(State state)
        {
            switch (state) 
            {
                case State.Ready:
                    break;
                case State.Shooting:
                    break;
                case State.PullingUserToObject:
                    break;
                case State.PullingObjectToUser:
                    if (_targetGrappleObject == null || _targetGrappleObject.Value.targetGrappleable == null)
                    {
                        ChangeState(State.Cooldown);
                        return;
                    }
                    
                    Vector3 directionToPlayer = _playerController.transform.position - _currentProjectile.transform.position;
                    directionToPlayer.Normalize();
                    
                    Rigidbody grappleableRigidbody = _targetGrappleObject.Value.targetGrappleable.RigidBody;
                    grappleableRigidbody.MovePosition(grappleableRigidbody.position + directionToPlayer * (_pullToPlayerSpeed * Time.fixedDeltaTime));
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
                    _targetIndicatorObject.gameObject.SetActive(false);
                    break;
                case State.Shooting:
                    break;
                case State.PullingUserToObject:
                    _playerController.OverrideWithCustomMovement(false);
                    break;
                case State.PullingObjectToUser:
                    _targetGrappleObject.Value.targetGrappleable.RigidBody.isKinematic = false;
                    break;
                case State.Cooldown:
                    break;
            }
        }

        public void OnProjectileReachedDestination()
        {
            if (_currentState != State.Shooting)
                return;

            if (_targetGrappleObject == null)
            {
                ChangeState(State.Cooldown);
                return;
            }
            
            if(_targetGrappleObject.Value.targetGrappleable.IsPullable)
                ChangeState(State.PullingObjectToUser);
            else
                ChangeState(State.PullingUserToObject);
        }
        
        private void ScanForAimSurfaces()
        {
            List<(Grappleable grappleable, Vector3 hitPoint)> validHits = new();
            
            RaycastHit[] hits = Physics.SphereCastAll(
                _projectileSpawnTransform.position + transform.forward * _rayDetectionRadius, 
                _rayDetectionRadius, 
                transform.forward, 
                _maxDetectionRadius
            );
            foreach (RaycastHit hit in hits)
            {
                Grappleable grappleable = hit.collider.GetComponentInParent<Grappleable>();
                if (grappleable == null)
                    continue;

                validHits.Add((grappleable, hit.collider.ClosestPoint(_projectileSpawnTransform.position)));
            }

            if (validHits.Count == 0)
            {
                _targetGrappleObject = null;
                return;
            }
            
            // Get closest hitpoint
            float closestSqrMagnitude = float.MaxValue;
            (Grappleable grappleable, Vector3 hitPoint)? closestHit = null;
            foreach (var hit in validHits)
            {
                float sqrMagnitude = (hit.hitPoint - _playerController.transform.position).sqrMagnitude;
                if (sqrMagnitude < closestSqrMagnitude)
                {
                    closestSqrMagnitude = sqrMagnitude;
                    closestHit = hit;
                }
            }
            
            _targetGrappleObject = closestHit;
        }

        private void UpdateLightIndicator()
        {
            if (_targetGrappleObject == null)
            {
                _targetIndicatorObject.gameObject.SetActive(false);
                return;
            }
            
            _targetIndicatorObject.gameObject.SetActive(true);
            _targetIndicatorObject.GetComponent<Renderer>().material.color =
                _targetGrappleObject.Value.targetGrappleable.IsPullable ? Color.yellow : Color.red;
                
            _targetIndicatorObject.transform.position =  _targetGrappleObject.Value.targetProjectileDestination;
        }
    }
}
