using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using _Scripts.Consystently.Essentials;
using _Scripts.Runtime.Combat.States;
using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Managers.Game_States;

namespace _Scripts.Runtime.Managers
{

  public class GameManager : Manager<GameManager>
  {
    public event Action<GameState> ChangedGameState;

    private List<GameState> gameStates = new List<GameState>();

    public GameState _currentGameState { get; private set; }
    private readonly Stack<GameState> _previousGameStates = new Stack<GameState>();
    private bool _menuOpened = false;

    public bool DesignerMode { get; private set; }


    protected override void Awake()
    {
      base.Awake();
      gameStates.Add(new MainMenuGameState(new GameStateContext(_previousGameStates)));
      gameStates.Add(new CombatGameState(new GameStateContext(_previousGameStates)));
      gameStates.Add(new OpenMenuGameState(new GameStateContext(_previousGameStates)));
     ChangeGameState(gameStates[0]);
    }
    
    void OnEnable()
    {
     EncounterManager.encountered += EnterCombat;
     BattleSimManager.submitted += EnterCombat;
    }
    
    //don't know if this matters with a singleton but who knows 
    void OnDisable()
    {
      EncounterManager.encountered -= EnterCombat; 
      BattleSimManager.submitted -= EnterCombat; 
    }

    void Update()
    {
      _currentGameState?.Update();
    }

    //have separate public methods that will decide the state being changed to 
    private void ChangeGameState(GameState newGameState)
    {
      if (_currentGameState == newGameState)
        return;
      _currentGameState?.Exit();
      _currentGameState = newGameState;
      _currentGameState?.Enter();
      ChangedGameState?.Invoke(newGameState);
    }

    //primarily for pause screens, menu screens, and other states that can transition to any other state 
    private void ReturnGameState()
    {
      if (_previousGameStates.Count < 1)
        return;
      _currentGameState?.Exit();
      _currentGameState = _previousGameStates.Pop();
      _currentGameState?.Enter(); 
      ChangedGameState?.Invoke(_currentGameState);
    }

    public void ToggleMenu()
    {
      _menuOpened = !_menuOpened;
      Debug.Log(_menuOpened);

      if (_currentGameState == gameStates[0])
        return;

      //no menu ui yet (different from main menu, which is the starting menu) 
      /*
      var enabledMaps = _inputSystemActions.asset.actionMaps.Where(map => map.enabled).ToList();
      if(_menuOpened)
      {
        foreach (var map in enabledMaps)
          map.Disable();
        _inputSystemActions.Global.Enable();
        _currentGameState = gameStates[2];
      }else{
         foreach (var map in enabledMaps)
           map.Enable();
         _currentGameState.Exit();
         _currentGameState = _previousGameStates.Peek();
      }
      */
    }

    //probably add an enum or something for the states later
    void EnterCombat(bool isBattleSim)
    {
      ChangeGameState(gameStates[1]);
      DesignerMode = isBattleSim;
    }


    public void QuitApplication ()
    {
      #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
      #else
        Application.Quit();
      #endif
    }
  }
}
