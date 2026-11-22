using UnityEngine;

namespace _Scripts.Runtime.Overworld.Tools.Drone
{
    [RequireComponent(typeof(Rigidbody))]
    public class Drone : MonoBehaviour
    {
        private DroneTool _tool;

        private Rigidbody _rigidbody;
        private float _remainingLifetime;
        private bool _launched;
        private bool _finished;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public void Launch(DroneTool tool, Vector3 direction, float speed, float lifetime)
        {
            _tool = tool;
            _remainingLifetime = Mathf.Max(0f, lifetime);
            _launched = true;
            _rigidbody.isKinematic = false;
            _rigidbody.linearVelocity = direction.normalized * Mathf.Max(0f, speed);
        }

        private void Update()
        {
            if (!_launched || _finished)
                return;

            _remainingLifetime -= Time.deltaTime;
            if (_remainingLifetime <= 0f)
                Finish();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_finished || collision == null || collision.collider == null)
                return;

            TryDestroyTarget(collision.collider);
            Finish();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_finished || other == null)
                return;

            TryDestroyTarget(other);
            Finish();
        }

        private void TryDestroyTarget(Collider hitCollider)
        {
            DroneTargetable targetable = hitCollider.GetComponentInParent<DroneTargetable>();
            if (targetable != null)
                Destroy(targetable.gameObject);
        }

        private void Finish()
        {
            if (_finished)
                return;

            _finished = true;
            if (_tool != null)
                _tool.OnDroneFinished(this);
            Destroy(gameObject);
        }
    }
}
