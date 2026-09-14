using System.Collections.Generic;
using System.Linq;
using Consystently.Essentials;
using Consystently.UI;
using UnityEngine;

public class BattlefieldMap : MonoBehaviour
{
    public static BattlefieldMap Instance { get; private set; }
    public List<BattlefieldTile> Tiles { get; private set; }

    [Header("Battlefield Map")]
    [SerializeField] ButtonVE _cancelButton;
    [SerializeField] ImageVE _helperTextBackground;

    private void Awake()
    {
        Instance ??= this;
        Tiles ??= GetComponentsInChildren<BattlefieldTile>().ToList();
    }

    void Start()
    {
        SetTileDetailsMode();
    }

    public void ResetBattlefield()
    {
        foreach (BattlefieldTile tile in Tiles)
            tile.ResetTile();
    }

    public void GetAvailableTileSlots(UnitPiece piece)
    {
        List<BattlefieldTile> availableTiles = new List<BattlefieldTile>();

        foreach (BattlefieldTile tile in Tiles)
            if (tile.CheckForSlot(piece))
                availableTiles.Add(tile);

        if (availableTiles.Count > 0)
            SetPiecePlacementMode(piece, availableTiles);
        else
            Debug.LogWarning("No available tiles found.");
    }

    public void SetTileDetailsMode()
    {
        _cancelButton.Hide();
        _helperTextBackground.Hide();

        foreach (BattlefieldTile tile in Tiles)
        {
            tile.Button.onClick.RemoveAllListeners();
            tile.Button.interactable = true;
            tile.Button.onClick.AddListener(() => TileDetails.Instance.DisplayTileDetails(tile));
        }
    }

    public void SetPiecePlacementMode(UnitPiece piece, List<BattlefieldTile> availableTiles)
    {
        _cancelButton.Show();
        _helperTextBackground.Show();

        foreach (BattlefieldTile tile in Tiles)
        {
            tile.Button.onClick.RemoveAllListeners();

            if (availableTiles.Contains(tile))
                tile.Button.onClick.AddListener(() =>
                {
                    tile.PlacePiece(piece);
                    EndPiecePlacement();
                });
            else
                tile.Button.interactable = false;
        }
    }

    public void EndPiecePlacement()
    {
        SetTileDetailsMode();
    }

}
