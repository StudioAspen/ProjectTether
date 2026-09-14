using AYellowpaper.SerializedCollections;
using Consystently.Essentials;
using UnityEngine;

public class BattleSimEncounterSeedManager : Manager<BattleSimEncounterSeedManager>
{
    public SerializedDictionary<string, BattlefieldTile> _mappedTiles = new();

    //private void OnEnable()
    //{
    //    UnitPieceSlot ll = GetComponent<UnitPieceSlot>();
    //    Debug.Log(ll);
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMappedTile(string tileName, BattlefieldTile tile) => _mappedTiles.Add(tileName, tile);
    public void DeleteMappedTile(string tileName) => _mappedTiles.Remove(tileName);
    //public void AddMappedTile(UnitPieceSlot slot)
    //{
    //    BattlefieldTile tile = slot.Tile;
    //    _mappedTiles.Add(tile.gameObject.name, tile);
    //}

    //public void DeleteMappedTile(UnitPieceSlot slot)
    //{
    //    BattlefieldTile tile = slot.Tile;
    //    _mappedTiles.Remove(tile.gameObject.name);
    //}

}
