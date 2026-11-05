using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace _Scripts.Runtime.Overworld
{
    /// <summary>
    /// Relays Unity collision and trigger callbacks through inspector-configurable events.
    /// Add this to the same GameObject as the collider that should detect contacts.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    [ExecuteAlways]
    public class CollisionDetector : MonoBehaviour
    {
        private Collider _detectorCollider;
        private bool _lastTriggerMode;
        private bool _hasTriggerModeSnapshot;

        [BoxGroup("Filtering"), SerializeField]
        private bool _filterByLayer;

        [BoxGroup("Filtering"), SerializeField, ShowIf(nameof(_filterByLayer))]
        private LayerMask _layers = ~0;

        [BoxGroup("Filtering"), SerializeField]
        private bool _filterByTag;

        [BoxGroup("Filtering"), SerializeField, ShowIf(nameof(_filterByTag)), Tag]
        private string _tagToMatch = "Untagged";

        [field: BoxGroup("Collision Events"), ShowIf(nameof(IsCollisionMode)), SerializeField]
        public UnityEvent<Collision> CollisionEnter { get; private set; } = new UnityEvent<Collision>();

        [field: BoxGroup("Collision Events"), ShowIf(nameof(IsCollisionMode)), SerializeField]
        public UnityEvent<Collision> CollisionStay { get; private set; } = new UnityEvent<Collision>();

        [field: BoxGroup("Collision Events"), ShowIf(nameof(IsCollisionMode)), SerializeField]
        public UnityEvent<Collision> CollisionExit { get; private set; } = new UnityEvent<Collision>();

        [field: BoxGroup("Trigger Events"), ShowIf(nameof(IsTriggerMode)), SerializeField]
        public UnityEvent<Collider> TriggerEnter { get; private set; } = new UnityEvent<Collider>();

        [field: BoxGroup("Trigger Events"), ShowIf(nameof(IsTriggerMode)), SerializeField]
        public UnityEvent<Collider> TriggerStay { get; private set; } = new UnityEvent<Collider>();

        [field: BoxGroup("Trigger Events"), ShowIf(nameof(IsTriggerMode)), SerializeField]
        public UnityEvent<Collider> TriggerExit { get; private set; } = new UnityEvent<Collider>();

        private bool IsTriggerMode => DetectorCollider != null && DetectorCollider.isTrigger;
        private bool IsCollisionMode => DetectorCollider != null && !DetectorCollider.isTrigger;
        private Collider DetectorCollider => _detectorCollider != null
            ? _detectorCollider
            : _detectorCollider = GetComponent<Collider>();

        private void OnEnable()
        {
            _detectorCollider = GetComponent<Collider>();
            _lastTriggerMode = _detectorCollider.isTrigger;
            _hasTriggerModeSnapshot = true;
        }

        private void OnValidate()
        {
            _detectorCollider = GetComponent<Collider>();
            UpdateTriggerMode();
        }

        private void Update()
        {
            // ExecuteAlways lets changes made on the Collider inspector clear stale listeners
            // even when the CollisionDetector itself was not the component being edited.
            UpdateTriggerMode();
        }

        private void UpdateTriggerMode()
        {
            if (DetectorCollider == null)
                return;

            bool isTrigger = DetectorCollider.isTrigger;
            if (!_hasTriggerModeSnapshot)
            {
                _lastTriggerMode = isTrigger;
                _hasTriggerModeSnapshot = true;
                return;
            }

            if (_lastTriggerMode == isTrigger)
                return;

            _lastTriggerMode = isTrigger;
            if (isTrigger)
                ClearCollisionEvents();
            else
                ClearTriggerEvents();
        }

        private void ClearCollisionEvents()
        {
            ClearEvent(CollisionEnter);
            ClearEvent(CollisionStay);
            ClearEvent(CollisionExit);
        }

        private void ClearTriggerEvents()
        {
            ClearEvent(TriggerEnter);
            ClearEvent(TriggerStay);
            ClearEvent(TriggerExit);
        }

        private static void ClearEvent(UnityEventBase unityEvent)
        {
            if (unityEvent == null)
                return;

#if UNITY_EDITOR
            for (int i = unityEvent.GetPersistentEventCount() - 1; i >= 0; i--)
                UnityEventTools.RemovePersistentListener(unityEvent, i);
#endif
            unityEvent.RemoveAllListeners();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (MatchesFilters(collision.collider))
                CollisionEnter.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (MatchesFilters(collision.collider))
                CollisionStay.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            if (MatchesFilters(collision.collider))
                CollisionExit.Invoke(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (MatchesFilters(other))
                TriggerEnter.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (MatchesFilters(other))
                TriggerStay.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (MatchesFilters(other))
                TriggerExit.Invoke(other);
        }

        private bool MatchesFilters(Collider other)
        {
            if (other == null)
                return false;

            if (_filterByLayer && (_layers.value & (1 << other.gameObject.layer)) == 0)
                return false;

            if (_filterByTag && !other.CompareTag(_tagToMatch))
                return false;

            return true;
        }
    }
}
