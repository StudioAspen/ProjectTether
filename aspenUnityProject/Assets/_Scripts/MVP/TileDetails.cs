using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Consystently.UI;

public class TileDetails : Panel
{
  public static TileDetails Instance { get; private set; }
  public List<UnitSim> Sims { get; private set; }

  protected override void Awake()
  {
    base.Awake();
    Instance ??= this;
    Sims ??= GetComponentsInChildren<UnitSim>().ToList();
  }

  public void DisplayTileDetails(BattlefieldTile tile)
  {
    Menu.OpenPanel(this);

    List<UnitPieceSlot> slots = tile.FilledSlots;
    for (int i = 0; i < Sims.Count; i++)
    {

      if (i >= slots.Count)
        Sims[i].ClearDetails();
      else
      {
        Sims[i].UpdateDetails(slots[i].Piece.Sim.Data);
      }
    
    }
  }
}
