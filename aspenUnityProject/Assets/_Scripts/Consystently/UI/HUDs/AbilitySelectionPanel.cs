using _Scripts.Runtime.Characters.Scripts;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using _Scripts.Runtime.UI;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace _Scripts.Consystently.UI
{
    public class AbilitySelectionPanel : Panel
    {
        [Header("UI References")]
        #region UI_Referemces
        [SerializeField] HUDElement _actionRing;
        [SerializeField] AbilityButtonVE[] _abilityButtonVEs = new AbilityButtonVE[7];
        #endregion

        #region References
        RangeDisplay _rangeDisplay;
        TileController[] _tileController;
        UnitController _currentUnit;
        #endregion

        #region Actions
        public static event Action OnAbilityMenuClose = delegate { };
        #endregion

        private void OnEnable()
        {
            AbilityButtonVE.OnAbilitySelect += RunAbilityRangeSelection;
        }

        private void OnDisable()
        {
            AbilityButtonVE.OnAbilitySelect -= RunAbilityRangeSelection;
        }


        private void OnValidate()
        {
            if (_actionRing == null) Debug.LogError("Action Ring Field is empty");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Close();

            for (int i = 0; i < _abilityButtonVEs.Length; i++)
            {
                AbilityButtonVE btn = _abilityButtonVEs[i];

                btn.Hide();
            }
        }
        public void Intialize(UnitController currentUnit, RangeDisplay rangeDisplay, TileController[] tileController)
        {
            _currentUnit = currentUnit;
            _rangeDisplay = rangeDisplay;
            _tileController = tileController;

            Unit unit = currentUnit.GetData();

            SetAbilityButtons(abilities: unit.Moves);
            // Activates the submenu
            _actionRing.Close();
            this.Open();
        }

        public void CloseAbilitiesMenu()
        {
            Close();
            _actionRing.Open();

            OnAbilityMenuClose?.Invoke();
        }

        public void RunAbilityRangeSelection(AbilitySO ability)
        {
            _rangeDisplay.DisplayAbilityRange(tileControllers: _tileController, cc: _currentUnit, ability);
        }

        void SetAbilityButtons(List<AbilitySO> abilities)
        {
            if (abilities.Count == 0) return;

            for (int i = 0; i < abilities.Count; i++)
            {
                AbilityButtonVE btn = _abilityButtonVEs[i];

                btn.Show();
                btn.OnSetup(currentAbility: abilities[i], abilitySelectionPanel: this, unitController: _currentUnit);
            }
        }
    }
}
