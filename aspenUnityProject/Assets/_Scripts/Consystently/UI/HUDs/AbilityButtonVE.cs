using _Scripts.Runtime.Combat;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Consystently.UI
{

    public class AbilityButtonVE : ButtonVE
    {
        AbilitySO _currentAbility;

        private void Awake()
        {
            //GetComponent<Button>().onClick.AddListener(AA);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AA()
        {

        }

        public void OnInstantiate(AbilitySO currentAbility)
        {
            _currentAbility = currentAbility;
            TextChild.text = _currentAbility.Name;
        }
    }
}
