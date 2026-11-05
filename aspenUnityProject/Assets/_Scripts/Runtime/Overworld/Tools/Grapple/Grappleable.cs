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

        private void OnValidate()
        {
            if(IsPullable && !TryGetComponent(out _rigidBody))
                _rigidBody = gameObject.AddComponent<Rigidbody>();
        }

        private void Awake()
        {
            if(IsPullable && !TryGetComponent(out _rigidBody))
                Debug.LogError($"Pullable grapple object {name} is missing a Rigidbody component");
        }
    }
}