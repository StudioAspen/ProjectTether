using _Scripts.Runtime.Characters.Scripts;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Entities.Scripts.Combat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.Consystently.UI
{
    public class AbilitySelectionPanel : Panel
    {
        [Header("Ability Selection")]
        #region Ability_Selection
        [SerializeField] GameObject _abilityButtonPrefab;
        [SerializeField] Transform _buttonsSpawnPoint;
        [SerializeField] Transform _abilityButtonHolder;
        #endregion

        [Header("UI Section")]
        #region UI_Section
        [SerializeField] HUDElement _actionRing;
        #endregion

        // For Testing Purposes
        [SerializeField] PlayableCharacterUnitSO _character;

        private void OnValidate()
        {
            if (_abilityButtonPrefab == null) Debug.LogError("Ability Button Prefab Field is empty");
            if (_buttonsSpawnPoint == null) Debug.LogError("Button Spawn Point Field is empty");
            if (_abilityButtonHolder == null) Debug.LogError("Ability Button Holder Field is empty");

            if (_actionRing == null) Debug.LogError("Action Ring Field is empty");
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Close();
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Rewrite this function to take in the all the current unit's abilites
        public void OnSetUp(UnitController currentUnit)
        {
            List<AbilitySO> listOfAllCharacterAbilites = new List<AbilitySO>();

            // Adding From Playable Character
            listOfAllCharacterAbilites.AddRange(_character.Moves);
            // Adding From Combat Class
            listOfAllCharacterAbilites.AddRange(_character.CombatClass.ClassAbilityList);
            // Adding From All Suits (Including Starting Suit)
            List<SuitBaseDataSO> suits = _character.Suits.ToList();
            suits.Add(_character.StartingSuit);

            foreach (SuitBaseDataSO suit in suits)
            {
                listOfAllCharacterAbilites.AddRange(suit.SkillTree.GetActiveAbilities());
            }


            // Printing
            foreach (AbilitySO ability in listOfAllCharacterAbilites)
            {
                Debug.Log(ability.Name);
            }

            SpawnAbilityButtons(listOfAllCharacterAbilites);

        }

        public void BackToActionRing()
        {
            Close();
            _actionRing.Open();
        }
        
        /*
         * Note: Once ryan give a maxiumum of how many abilites a character can have, we'll create an object pool of AbilityButtonVE to
         * replace instantie/destroying as it will be memory extensive, espcially if a player keeps backing in and out of the 
         * ability menu
         */

        void SpawnAbilityButtons(List<AbilitySO> abilites)
        {
            const float BUTTON_BOTTOM_SPACING = 80f;

            for (int i = 0; i < abilites.Count; i++)
            {
                if (i == 0)
                {
                    GameObject abilityButton = Instantiate(_abilityButtonPrefab, _abilityButtonHolder);
                    abilityButton.transform.position = _buttonsSpawnPoint.position;
                    abilityButton.GetComponent<AbilityButtonVE>().OnInstantiate(currentAbility: abilites[i]);
                } else
                {
                    Vector3 newPosition = new Vector3(_buttonsSpawnPoint.position.x, (_buttonsSpawnPoint.position.y - (BUTTON_BOTTOM_SPACING * i)), _buttonsSpawnPoint.position.z);
                    GameObject abilityButton = Instantiate(_abilityButtonPrefab, _abilityButtonHolder);
                    abilityButton.transform.position = newPosition;
                    abilityButton.GetComponent<AbilityButtonVE>().OnInstantiate(currentAbility: abilites[i]);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_buttonsSpawnPoint.position, radius: 10f);
        }
    }
}
