using System;
using System.Collections.Generic;
using Consystently.Essentials;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Consystently.UI;

/*
 * 
 * Make it to where, when a unit is removed from the board, it is also removed from the dict (do this shit at a later point)
 * 
 */

public class BattleSimManager : Manager<BattleSimManager>
{
  public Dictionary<UnitSim, BattlefieldTile> PlayerSims { get; private set; } = new Dictionary<UnitSim, BattlefieldTile>();
  public Dictionary<UnitSim, BattlefieldTile> EnemySims { get; private set; } = new Dictionary<UnitSim, BattlefieldTile>();

  // public HashSet<UnitPiece> PlayerPieces = new HashSet<UnitPiece>();
  // public HashSet<UnitPiece> EnemyPieces = new HashSet<UnitPiece>();

  public static event Action submitted; 

  [SerializeField] Button _battleButton;

  private void Start()
  {
    _battleButton.interactable = false;
  }

  public void ActivateSim(UnitSim sim)
  {
    switch(sim.Data?.Faction)
    {
      case Faction.Ally:
        PlayerSims.TryAdd(sim, null);
      break;
      case Faction.Enemy:
        EnemySims.TryAdd(sim, null);
      break;
    }

    Debug.Log($"{sim.Data?.Name} activated.");
    _battleButton.interactable = AtLeastTwoFactionsPlaced();
  }

  public void DeactivateSim(UnitSim sim)
  {
    switch (sim.Data?.Faction)
    {
      case Faction.Ally:
        PlayerSims.Remove(sim);
      break;
      case Faction.Enemy:
        EnemySims.Remove(sim);
      break;
    }

    Debug.Log($"{sim.Data?.Name} deactivated.");
    _battleButton.interactable = AtLeastTwoFactionsPlaced();
  }

  public void UpdatePiecePlacement(UnitPiece piece, BattlefieldTile tile = null)
  {
    switch (piece.Sim.Data?.Faction)
    {
      case Faction.Ally:
        if(PlayerSims.ContainsKey(piece.Sim))
          PlayerSims[piece.Sim] = tile;
      break;
      case Faction.Enemy:
        if(EnemySims.ContainsKey(piece.Sim))
          EnemySims[piece.Sim] = tile;
      break;
    }

    _battleButton.interactable = AtLeastTwoFactionsPlaced();
  }

  public bool AtLeastTwoFactionsPlaced()
  {
    if(PlayerSims.Values.Any(s => s != null) && EnemySims.Values.Any(s => s != null))
      return true;


    return false;
  }

  public bool AllSimPiecesPlaced()
  {
    if(PlayerSims.Values.All(s => s != null) && EnemySims.Values.All(s => s != null))
      return true;
    else
      return false;
  }

  public void TryStartBattle()
  {
    if(AllSimPiecesPlaced())
    {
      StartBattle();
    }
    else
    {
      MenuManager.Instance?.OpenPanel("Placement Warning");
    }
  }

  public void StartBattle()
  {
    Debug.Log("Start Battle Code");
    EncounterManager.Instance.GenerateEncounter(BattlefieldMap.Instance.Tiles);
    submitted?.Invoke();
  }
}
