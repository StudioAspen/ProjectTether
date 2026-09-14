using UnityEngine;
using TileSystem;
using Consystently.Essentials;

public class TileManager : Singleton<TileManager>
{
    // For all future tiles, follow a flow similar to PLAIN_TILE
    #region PLAIN_TILE
    [Header("Plain Tile Settings")]
    [SerializeField] TileType _plainTileType;
    [SerializeField] int _plainCost;
    [SerializeField] Mesh _plainMesh;
    #endregion

    #region LAVA_TILE
    [Header("Lava Tile Settings")]
    [SerializeField] TileType _lavaTileType;
    [SerializeField] int _lavaCost;
    [SerializeField] float _lavaDamageOvertime;
    [SerializeField] Mesh _lavaMesh;
    #endregion



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public Tile CreatePlainTile()
    {
        return new TileBuilder()
            .WithType(TileType.Plain)
            .WithCost(_plainCost)
            .WithMesh(_plainMesh)
            .Build();
    }

    public Tile CreateLavaTile()
    {
        return new TileBuilder()
            .WithType(TileType.Lava)
            .WithCost(_lavaCost)
            .WithMesh(_lavaMesh)
            .WithDamageOvertime(_lavaDamageOvertime)
            .Build();
    }
}
