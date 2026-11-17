using System;
using UnityEngine;

namespace _Scripts.Runtime.Overworld.Tools.Grapple
{
    /// <summary>
    /// Attach to objects that can grappleable
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class Grappleable : MonoBehaviour
    {
        [field: SerializeField] public bool IsPullable { get; private set; } = false;

        private Rigidbody _rigidBody;
        public Rigidbody RigidBody => _rigidBody;
        public Collider Collider { get; private set; }

        private void OnValidate()
        {
            if (IsPullable && !TryGetComponent(out _rigidBody))
            {
                _rigidBody = gameObject.AddComponent<Rigidbody>();
                _rigidBody.freezeRotation = true;
                _rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
                _rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }

        private void Awake()
        {
            if(IsPullable && !TryGetComponent(out _rigidBody))
                Debug.LogError($"Pullable grapple object {name} is missing a Rigidbody component");
            
            Collider = GetComponent<Collider>();
        }
    }
}