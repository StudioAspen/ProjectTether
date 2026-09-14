using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using Consystently.UI;
using Unity.VisualScripting;
using System;

[RequireComponent(typeof(Button))]
public class BattlefieldTile : MonoBehaviour
{
    public Button Button { get; private set; }
    public Faction Faction { get; private set; } = Faction.Neutral;

    public Stack<UnitPieceSlot> VacantSlots { get; private set; } = new Stack<UnitPieceSlot>();
    public List<UnitPieceSlot> FilledSlots { get; private set; } = new List<UnitPieceSlot>();

    void Awake()
    {
        foreach (UnitPieceSlot slot in GetComponentsInChildren<UnitPieceSlot>().OrderByDescending(s => s.gameObject.name))
        {
            VacantSlots.Push(slot);
            slot.SetTile(this);
            slot.OnPieceRemoved += RemovePiece;
        }

        Button ??= GetComponent<Button>();
        Button.image.alphaHitTestMinimumThreshold = 0.5f;
    }

    internal void ResetTile()
    {
        int count = FilledSlots.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            UnitPieceSlot slot = FilledSlots[i];
            VacantSlots.Push(slot);
            FilledSlots.Remove(slot);
            slot.Piece.Sim.ReturnPiece();
        }

        Faction = Faction.Neutral;
    }

    public bool CheckForSlot(UnitPiece piece)
    {
        if (Faction != Faction.Neutral && piece.Faction != Faction)
            return false;
        else if (FilledSlots.Count == 4)
            return false;
        else if (FilledSlots.Contains(piece.Slot))
            return false;
        else
            return true;
    }

    public void PlacePiece(UnitPiece piece)
    {
        if (VacantSlots.TryPop(out UnitPieceSlot result))
        {
            piece.Move(result);
            FilledSlots.Add(result);
            BattleSimManager.Instance.UpdatePiecePlacement(piece, this);

            if (Faction == Faction.Neutral)
                Faction = piece.Faction;
        }
    }

    public void RemovePiece(UnitPiece piece, UnitPieceSlot slot)
    {
      BattleSimManager.Instance.UpdatePiecePlacement(piece, null);
      if (slot && FilledSlots.Contains(slot))
      {
        UnitPieceSlot last = FilledSlots.Last();

        if (last != slot)
        {
          int index = FilledSlots.IndexOf(slot);

          for (int i = index; i < FilledSlots.Count - 1; i++)
          {
            UnitPieceSlot current = FilledSlots[i];
            UnitPieceSlot next = FilledSlots[i + 1];

            next.Piece.Move(current);
          }
        }

        VacantSlots.Push(last);
        FilledSlots.Remove(last);
      }

      if (FilledSlots.Count == 0)
          Faction = Faction.Neutral;
    }


}
