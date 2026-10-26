using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Runtime.Overworld.Tools.Grapple
{
    public class GrappleTool : Tool
    {
        [SerializeField, Required] private GrappleProjectile _projectilePrefab;
        [SerializeField, Required] private Transform _projectileSpawnTransform;
        [SerializeField] private float _shootRange = 10f;
        [SerializeField] private float _projectileSpeed = 10f;
        [SerializeField] private float _pullSpeed = 10f;
        [SerializeField] private float _pullStopDistance = 1f;
        [SerializeField] private float _pullTimeoutDuration = 10f;
        [SerializeField] private float _shootCooldown = 5f;
        [SerializeField, ReadOnly] private Collider _colliderShot;
        
        public enum State
        {
            Ready,
            Shooting,
            Pulling,
            Cooldown
        }
        [SerializeField, ReadOnly] private State _currentState = State.Ready;

        private ToolUser _user;
        private OverworldPlayerController _playerController;
        private GrappleProjectile _currentProjectile;
        [SerializeField, ReadOnly] private float _pullDuration = 0f;
        [SerializeField, ReadOnly] private float _cooldownTimer = 0f;
        
        private protected override void OnActivate(ToolUser user)
        {
            if (_user == null)
            {
                _user = user;
                _playerController = _user.GetComponent<OverworldPlayerController>();
            }
            
            if (_currentState != State.Ready)
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
                    _currentProjectile.Shoot(this, _user.transform.forward, _shootRange, _projectileSpeed);
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