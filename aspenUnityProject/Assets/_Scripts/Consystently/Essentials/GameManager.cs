namespace Consystently.Essentials
{
  using UnityEditor;
  using UnityEngine;
  using System;
  using System.Collections.Generic;

  public class GameManager : Manager<GameManager>
  {
    public bool GameIsPaused { get; private set; } = false;
    
    public static event Action<GameState> ChangedGameState;

    private List<GameState> gameStates = new List<GameState>();

    private GameState currentGameGameState;
    //push to stack for certain states 
    private GameState previousGameState;
    private readonly Stack<GameState> previousGameStates = new Stack<GameState>();


    void Instantiate()
    {
      EncounterManager.encountered += EnterCombat;
      BattleSimManager.submitted += EnterCombat;
      gameStates.Add(new MainMenuGameState(this));
      gameStates.Add(new CombatGameState(this));
      
      ChangeGameState(gameStates[0]);
    }
    
    public void PauseGame ()
    {
      GameIsPaused = !GameIsPaused;
      Time.timeScale = GameIsPaused ? 0f : 1f;
    }

    //have separate public methods that will decide the state being changed to 
    //PERHAPS have an array of all game states and remove the parameter; swap between them with logic in the method
    private void ChangeGameState(GameState newGameState)
    {
      if (currentGameGameState == newGameState)
        return;
      currentGameGameState?.Exit();
      previousGameState = currentGameGameState;
      currentGameGameState = newGameState;
      currentGameGameState?.Enter();
      ChangedGameState?.Invoke(newGameState);
    }

    //primarily for pause screens, menu screens, and other states that can transition to any other state 
    private void ReturnGameState()
    {
      if (previousGameStates.Count < 1)
        return;
      currentGameGameState?.Exit();
      currentGameGameState = previousGameStates.Pop();
      currentGameGameState?.Enter(); 
      ChangedGameState?.Invoke(currentGameGameState);
    }

    public void PushOldState()
    {
      previousGameStates.Push(previousGameState);
    }

    //probably add an enum or something for the states later
    public void EnterCombat()
    {
      ChangeGameState(gameStates[1]);      
    }

    protected override void Awake()
    {
      base.Awake();
      Instantiate();
    }

    void Update()
    {
      currentGameGameState?.Update();
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
