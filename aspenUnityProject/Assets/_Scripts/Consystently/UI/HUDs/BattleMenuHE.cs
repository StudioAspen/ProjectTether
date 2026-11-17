
using System;
using System.Collections;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.States.CombatManagerStates;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Tile_System.Scripts;
using _Scripts.Runtime.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace _Scripts.Consystently.UI
{
    public class BattleMenuHE : HUDElement
    {
        [Header("Battle Menu")]
        [SerializeField] private MeshFilter focusedEntity;
        protected GameMenu _menu;

        [SerializeField] private GameObject cursor;
        private bool _designerMode;

        [Header("Action Ring Buttons")]
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _defendButton;
        [SerializeField] private Button _moveButton;
        [SerializeField] private Button _abilityButton;


        //todo: needs to be moved to a separate script attached to the battle log, not this script for the action ring 
        [Header("Battle Announcements")]

        [SerializeField]
        private GameObject announcementPanel;

        [SerializeField]
        private TMP_Text announcementText;

        [SerializeField]
        [Min(0f)]
        private float announcementDuration = 2.10f;

        private Coroutine announcementCoroutine;

       
        public static event Action<string> AnnouncementRequested;

        public static void Announce(
            string actor,
            string action,
            string target)
        {
            string message = $"[{actor}, {action}: {target}]";
            AnnouncementRequested?.Invoke(message);
        }

       
        public static void AnnounceEvent(string message)
        {
            AnnouncementRequested?.Invoke(message);
        }

        private void HandleAnnouncement(string message)
        {
            if (announcementPanel == null || announcementText == null)
                return;

            // Stop the previous announcement timer.
            if (announcementCoroutine != null)
                StopCoroutine(announcementCoroutine);

            announcementText.text = message;
            announcementPanel.SetActive(true);

            announcementCoroutine =
                StartCoroutine(HideAnnouncementAfterDelay());
        }

        private IEnumerator HideAnnouncementAfterDelay()
        {
            yield return new WaitForSeconds(announcementDuration);

            announcementPanel.SetActive(false);
            announcementCoroutine = null;
        }

        

        public static event Action<CombatActions> PlayerAction;
        public static event Action<CombatActions, int> PlayerSelectiveAction;
        public static event Action<bool> ToggledManualEnemy;

        protected void Awake()
        {
            _menu ??= GetComponent<GameMenu>();

            if (announcementPanel != null)
                announcementPanel.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();

            CombatManager.battlePhaseChanged += HandleUserActions;
            CombatManager.examinedTile += ExamineTile;
            CombatManager.exitedExamine += ExitExamine;
            CombatManager.examinedUnit += ExamineUnit;

            AnnouncementRequested += HandleAnnouncement;

            if (focusedEntity != null && focusedEntity.mesh != null)
                Debug.Log(focusedEntity.mesh.bounds.size);

            if (_attackButton != null)
                _attackButton.onClick.AddListener(
                    () => OnActionButtonClicked(CombatActions.Attack));

            if (_defendButton != null)
                _defendButton.onClick.AddListener(
                    () => OnActionButtonClicked(CombatActions.Defend));

            if (_moveButton != null)
                _moveButton.onClick.AddListener(
                    () => OnActionButtonClicked(CombatActions.Move));

            if (_abilityButton != null)
                _abilityButton.onClick.AddListener(
                    () => OnActionButtonClicked(CombatActions.Ability));
        }

        protected virtual void OnDestroy()
        {
           
            CombatManager.battlePhaseChanged -= HandleUserActions;
            CombatManager.examinedTile -= ExamineTile;
            CombatManager.exitedExamine -= ExitExamine;
            CombatManager.examinedUnit -= ExamineUnit;

            AnnouncementRequested -= HandleAnnouncement;

            if (_attackButton != null)
                _attackButton.onClick.RemoveAllListeners();

            if (_defendButton != null)
                _defendButton.onClick.RemoveAllListeners();

            if (_moveButton != null)
                _moveButton.onClick.RemoveAllListeners();

            if (_abilityButton != null)
                _abilityButton.onClick.RemoveAllListeners();

            if (announcementCoroutine != null)
                StopCoroutine(announcementCoroutine);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                MenuManager.Instance.OpenMenu(_menu);
        }

        private void OnActionButtonClicked(CombatActions action)
        {
            Debug.Log($"[BattleMenuHE] Player confirmed action: {action}");

            if (action != CombatActions.Defend)
            {
                Hide();
                InputManager.Instance.Actions.UI.Disable();
                cursor.SetActive(true);
            }

            PlayerAction?.Invoke(action);
        }

        private void HandleUserActions(
            BattlePhase bs,
            UnitController unitController)
        {
            cursor.SetActive(false);

            if (bs is PlayerPhase)
                Show();
            else
                Hide();

            if (_canvasGroup.interactable)
                InputManager.Instance.Actions.UI.Enable();
            else
                InputManager.Instance.Actions.UI.Disable();
        }

        private void ExamineTile(TileController tc)
        {
            cursor.SetActive(false);
        }

        private void ExitExamine()
        {
            cursor.SetActive(true);
        }

        private void ExamineUnit(UnitController uc)
        {
            Debug.Log(uc.GetData().Name);
        }
    }
}
