using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UnitPiece : MonoBehaviour
{
    public UnitSim Sim { get; private set; }
    public UnitPieceSlot Slot { get; private set; }
    public Image Icon { get; private set; }
    public Sprite Sprite => Icon.sprite;
    public Color Color => Icon.color;
    public Faction Faction => Sim.Data.Faction;

    public void Awake()
    {
        Icon ??= GetComponent<Image>();
    }

    public void SetSim(UnitSim sim) => Sim ??= sim;

    public void SetSlot(UnitPieceSlot slot) => Slot = slot;

    public void Move(UnitPieceSlot newSlot)
    {
        if (!newSlot.Piece)
        {
            transform.SetParent(newSlot.transform, false);
            Slot.RemovePiece(this);
            newSlot.SetPiece(this);
            SetSlot(newSlot);
        }
    }

    public void Move(BattlefieldTile tile)
    {
        if (tile.CheckForSlot(this))
            tile.PlacePiece(this);
    }
}
