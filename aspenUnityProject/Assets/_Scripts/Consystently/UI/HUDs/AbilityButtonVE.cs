using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Entities.Scripts.Combat;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Consystently.UI
{

    public class AbilityButtonVE : ButtonVE
    {
        AbilitySelectionPanel _abilitySelectionPanel;
        [field: SerializeField] public AbilitySO CurrentAbility { get; private set; }
        [field: SerializeField] public UnitController CurrentUnit { get; private set; }

        #region Events
        public static event Action<AbilitySO> OnAbilitySelect = delegate { };
        public static event Action OnAbilitySelectView = delegate { };
        #endregion

        protected override void Awake()
        {
            //GetComponent<Button>().onClick.AddListener(AA);
            base.Awake();

        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SendAbility()
        {
            Debug.Log("Ive been clicked");

            OnAbilitySelect?.Invoke(CurrentAbility);
            OnAbilitySelectView?.Invoke();
        }

        public void OnSetup(AbilitySO currentAbility, AbilitySelectionPanel abilitySelectionPanel, UnitController unitController)
        {
            CurrentAbility = currentAbility;
            _abilitySelectionPanel = abilitySelectionPanel;
            CurrentUnit = unitController;

            Component.onClick.AddListener(SendAbility);

            TextChild.text = CurrentAbility.Name;
        }
    }
}
