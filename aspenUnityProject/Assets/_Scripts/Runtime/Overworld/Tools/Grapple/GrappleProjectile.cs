using UnityEngine;

namespace _Scripts.Runtime.Overworld.Tools.Grapple
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrappleProjectile : MonoBehaviour
    {
        private GrappleTool _tool;
        private Rigidbody _rigidbody;

        private Vector3 _direction;
        private float _range;
        private float _speed;
        private Vector3 _startPosition;
        private bool _isMoving;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Shoot(GrappleTool tool, Vector3 direction, float range, float speed)
        {
            _tool = tool;
            _direction = direction;
            _range = range;
            _speed = speed;
            _isMoving = true;
            
            _startPosition = transform.position;
        }

        private void Update()
        {
            CheckDistanceTravelled();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void ApplyMovement()
        {
            if (!_isMoving)
                return;
            
            _rigidbody.MovePosition(transform.position + _direction * _speed * Time.fixedDeltaTime);
        }

        private void CheckDistanceTravelled()
        {
            float distanceTravelledSquared = Vector3.SqrMagnitude(transform.position - _startPosition);
            if (distanceTravelledSquared >= _range * _range)
                OnMaxRangeReached();
        }

        private void OnMaxRangeReached()
        {
            _tool.OnProjectileCollideFail();
            _isMoving = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            Grappleable grappleable = other.GetComponentInParent<Grappleable>();
            if (grappleable == null)
                return;
            
            _tool.OnProjectileCollideSuccess();
            _isMoving = false;
        }
    }
}