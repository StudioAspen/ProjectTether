
using TMPro;
using UnityEngine;
using System.Collections;

namespace _Scripts.Consystently.UI
{
    public class BattleLogHE : HUDElement
    {
        [Header("Battle Log")]
        [SerializeField] private RectTransform _container;
        [SerializeField] private TextMeshProUGUI _logText;

        [Header("Settings")]
        [SerializeField, Range(0f, 10f)]
        private float _displayDuration = 2.10f;

        [SerializeField] private bool _hideOnStart = true;

        private Coroutine _hideCoroutine;

        private void Awake()
        {
            if (_logText != null)
                _logText.text = string.Empty;

            if (_hideOnStart && _container != null)
                _container.gameObject.SetActive(false);
        }

        /// <summary>
        /// Displays a combat message.
        /// </summary>
        public void ShowMessage(string message)
        {
            if (_container == null || _logText == null)
                return;

            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }

            _logText.text = message;
            _container.gameObject.SetActive(true);

            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }

        /// <summary>
        /// Announces an actor performing an action against a target.
        /// Example: Knight, Attacks: Goblin
        /// </summary>
        public void LogAction(string actor, string action, string target)
        {
            ShowMessage($"{actor}, {action}: {target}");
        }

        /// <summary>
        /// Announces terrain-related events.
        /// </summary>
        public void LogTerrainEvent(string eventDescription)
        {
            ShowMessage(eventDescription);
        }

        /// <summary>
        /// Announces damage-over-time or other ongoing effects.
        /// </summary>
        public void LogEffect(string actor, string effect, string target)
        {
            ShowMessage($"{actor}, {effect}: {target}");
        }

        /// <summary>
        /// Announces status effects such as stun, poison, or burn.
        /// </summary>
        public void LogStatusEffect(string target, string status)
        {
            ShowMessage($"{target} is affected by {status}!");
        }

        /// <summary>
        /// Announces damage events.
        /// </summary>
        public void LogDamage(string actor, string target, int damage)
        {
            ShowMessage($"{actor} deals {damage} damage to {target}!");
        }


        /// <summary>
        /// Announces healing events.
        /// </summary>
        public void LogHealing(string actor, string target, int amount)
        {
            ShowMessage($"{actor} heals {target} for {amount} HP!");
        }

        /// <summary>
        /// Hides the combat log after the configured duration.
        /// </summary>
        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(_displayDuration);

            if (_container != null)
                _container.gameObject.SetActive(false);

            _hideCoroutine = null;
        }

        /// <summary>
        /// Immediately clears and hides the combat log.
        /// </summary>
        public void ClearLog()
        {
            if (_hideCoroutine != null)
            {
                StopCoroutine(_hideCoroutine);
                _hideCoroutine = null;
            }

            if (_logText != null)
                _logText.text = string.Empty;

            if (_container != null)
                _container.gameObject.SetActive(false);
        }
    }
}
