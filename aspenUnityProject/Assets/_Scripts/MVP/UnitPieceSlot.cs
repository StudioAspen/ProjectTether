using System;
using UnityEngine;

public class UnitPieceSlot : MonoBehaviour
{
    [HideInInspector]
    public UnitPiece Piece { get; private set; }
    public UnitSim Sim { get; private set; }
    public BattlefieldTile Tile { get; private set; }
    public event Action<UnitPiece, UnitPieceSlot> OnPieceSet, OnPieceRemoved;

    public void Awake()
    {
        Piece ??= GetComponentInChildren<UnitPiece>();
        Piece?.SetSlot(this);
    }

    public void SetSim(UnitSim sim) => Sim ??= sim;

    public void SetTile(BattlefieldTile tile) => Tile ??= tile;

    public void SetPiece(UnitPiece piece = null)
    {
        if (Piece && piece)
            return;
        else
            Piece = piece;

        // BattleSimManager.Instance.AddSimToDict(piece.Sim);
        // BattleSimEncounterSeedManager.Instance.AddMappedTile(Tile.gameObject.name, Tile);

        OnPieceSet?.Invoke(piece, this);
    }

    public void RemovePiece(UnitPiece piece)
    {
        if (Piece != piece || !piece)
            return;

        // BattleSimManager.Instance.RemoveSimFromDict(piece.Sim);
        // BattleSimEncounterSeedManager.Instance.DeleteMappedTile(Tile.gameObject.name);

        Piece = null;
        OnPieceRemoved?.Invoke(piece, this);
    }

    public void TryPlacePiece()
    {
        //If this slot belongs to a sim, return the sim's piece to the slot.
        if (Sim.Piece && !Piece)
            ReturnPiece();
        else
            BattlefieldMap.Instance.GetAvailableTileSlots(Piece);
    }

    public void ReturnPiece() => Sim?.Piece.Move(this);
}
