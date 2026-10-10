using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using _Scripts.Consystently.Essentials;
using _Scripts.Runtime.Combat.States;
using _Scripts.Runtime.Managers.Game_States;
using Eflatun.SceneReference;
using NaughtyAttributes;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Scripts.Runtime.Managers
{
    public class GameManager : Manager<GameManager>
    {
        public enum State
        {
            MainMenu,
            Combat,
            Pause,
            Overworld
        }
        
        public event Action<State> OnGameStateChanged = delegate { };

        private Dictionary<State, GameState> _gameStates = new();

        [field: SerializeField, ReadOnly] public State CurrentGameState { get; private set; }
        
        [ShowNonSerializedField]
        private readonly Stack<State> _previousGameStates = new Stack<State>();
        
        [field: Header("Scenes")]
        [field: SerializeField] public SceneReference MainMenuScene { get; private set; }
        [field: SerializeField] public SceneReference OverworldScene { get; private set; }
        [field: SerializeField] public SceneReference BattleScene { get; private set; }

        public bool DesignerMode { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            
            _gameStates[State.MainMenu] = new MainMenuGameState();
            _gameStates[State.Combat] = new CombatGameState();
            _gameStates[State.Pause] = new PauseGameState();
            _gameStates[State.Overworld] = new OverworldGameState();
            
            ChangeGameState(State.MainMenu);
        }

        void OnEnable()
        {
            InputManager.Instance.Actions.Global.Pause.performed += Input_OnPause;
            
            EncounterManager.encountered += EnterCombat;
            BattleSimManager.submitted += EnterCombat;
        }

        //don't know if this matters with a singleton but who knows 
        void OnDisable()
        {
            InputManager.Instance.Actions.Global.Pause.performed -= Input_OnPause;
            
            EncounterManager.encountered -= EnterCombat; 
            BattleSimManager.submitted -= EnterCombat; 
        }
        
        void Update()
        {
            _gameStates[CurrentGameState]?.Update();
        }

        public void ChangeGameState(State newState)
        {
            if (CurrentGameState == newState)
                return;
            
            _gameStates[CurrentGameState]?.Exit();
            
            if (_gameStates[CurrentGameState] != null)
                _previousGameStates.Push(CurrentGameState);
            
            CurrentGameState = newState;
            _gameStates[CurrentGameState]?.Enter();
            
            OnGameStateChanged?.Invoke(newState);
        }

        //primarily for pause screens, menu screens, and other states that can transition to any other state 
        public void ReturnGameState()
        {
            if (_previousGameStates.Count < 1)
              return;
            
            _gameStates[CurrentGameState]?.Exit();
            
            CurrentGameState = _previousGameStates.Pop();
            _gameStates[CurrentGameState]?.Enter(); 
            
            OnGameStateChanged?.Invoke(CurrentGameState);
        }

        //probably add an enum or something for the states later
        void EnterCombat(bool isBattleSim)
        {
            ChangeGameState(State.Combat);
            DesignerMode = isBattleSim;
        }

        private void Input_OnPause(InputAction.CallbackContext context)
        {
            if (CurrentGameState == State.Pause)
                return;
            
            PauseGame();
        }

        public void PauseGame()
        {
            // Pausing doesn't work when in main menu
            if (CurrentGameState == State.MainMenu)
                return;
            
            ChangeGameState(State.Pause);
        }

        public void GotoMainMenu()
        {
            if (CurrentGameState == State.MainMenu)
                return;

            ChangeGameState(State.MainMenu);
        }

        public void StartGame()
        {
            if (CurrentGameState != State.MainMenu)
                return;
            
            ChangeGameState(State.Overworld);
        }

        public static void ChangeScene(string sceneName)
        {
            if (sceneName == SceneManager.GetActiveScene().name)
                return;
            
            SceneManager.LoadScene(sceneName);
        }
        
        public static void QuitApplication()
        {
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
