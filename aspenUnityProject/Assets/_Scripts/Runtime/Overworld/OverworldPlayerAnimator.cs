using System;
using Animancer;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Runtime.Overworld
{
    public class OverworldPlayerAnimator : MonoBehaviour
    {
        [SerializeField, Required] private OverworldPlayerController _playerController;
        [SerializeField, Required] private OverworldPartyController _partyController;

        public enum State
        {
            Idle,
            Walk
        }
        [SerializeField, ReadOnly] private State _currentState = State.Idle;

        [SerializeField, SerializedDictionary("State", "Animation")]
        private SerializedDictionary<State, ClipTransition> _animations = new();
        [SerializeField] private float _defaultTransitionDuration = 0.1f;

        private void Start()
        {
            ChangeState(State.Idle, true);
        }

        private void OnEnable()
        {
            _partyController.OnPartyIndexChanged += OnPartyIndexChanged;
        }

        private void OnDisable()
        {
            _partyController.OnPartyIndexChanged -= OnPartyIndexChanged;
        }

        private void Update()
        {
            switch (_currentState)
            {
                case State.Idle:
                    if (_playerController.IsMoving)
                    {
                        ChangeState(State.Walk);
                        return;
                    }
                    break;
                case State.Walk:
                    if (!_playerController.IsMoving)
                    {
                        ChangeState(State.Idle);
                        return;
                    }
                    break;
            }
        }

        private void OnPartyIndexChanged(int newIndex)
        {
            PlayEnterStateAnimation(_currentState);
        }

        private void ChangeState(State newState, bool forceChange = false)
        {
            if (!forceChange && _currentState == newState)
                return;
            
            _currentState = newState;
            PlayEnterStateAnimation(newState);
        }
        
        private void PlayEnterStateAnimation(State state)
        {
            _partyController.CurrentAnimator.Play(_animations[state], _defaultTransitionDuration);
        }
    }
}