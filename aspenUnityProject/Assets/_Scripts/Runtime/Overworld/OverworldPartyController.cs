using System;
using System.Collections.Generic;
using _Scripts.Runtime.Managers;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Overworld
{
    public class OverworldPartyController : MonoBehaviour
    {
        [SerializeField, Required] private List<AnimancerComponent> _partyMemberAnimators = new();
        
        [ShowInInspector] public AnimancerComponent CurrentAnimator => _partyMemberAnimators[CurrentPartyIndex];
        
        public int CurrentPartyIndex { get; private set; }
        
        public event Action<int> OnPartyIndexChanged = delegate { };

        private void Start()
        {
            SwapPartyMember(0);
        }

        private void OnEnable()
        {
            InputManager.Instance.Actions.Player.SwapPartyMember.performed += Input_OnSwapPartyMemberPerformed;
        }

        private void OnDisable()
        {
            if(InputManager.Instance != null)
                InputManager.Instance.Actions.Player.SwapPartyMember.performed += Input_OnSwapPartyMemberPerformed;
        }

        private void Input_OnSwapPartyMemberPerformed(InputAction.CallbackContext context)
        {
            SwapPartyMember(context.ReadValue<int>());
        }

        [Button("Swap Party Member")]
        public void SwapPartyMember(int index)
        {
            if (index < 0 || index >= _partyMemberAnimators.Count)
            {
                Debug.LogWarning($"Can't swap to invalid party member {index}");
                return;
            }

            if (index == CurrentPartyIndex)
            {
                Debug.LogWarning($"Can't swap to currently selected party member {index}");
                return;
            }
            
            CurrentPartyIndex = index;
            OnPartyIndexChangedInternal(index);
        }

        private void OnPartyIndexChangedInternal(int index)
        {
            DisableAllPartyMembers();
            _partyMemberAnimators[index].gameObject.SetActive(true);
            
            OnPartyIndexChanged?.Invoke(index);
        }

        private void DisableAllPartyMembers()
        {
            foreach (AnimancerComponent animator in _partyMemberAnimators)
                animator.gameObject.SetActive(false);
        }
    }
}