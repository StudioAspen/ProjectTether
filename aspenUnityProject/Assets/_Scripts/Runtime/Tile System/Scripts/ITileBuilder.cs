using UnityEngine;

namespace TileSystem
{
    public interface ITileBuilder
    {
        ITileBuilder WithPosition(Vector3 pos);
        ITileBuilder WithDamageOvertime(float value);
        ITileBuilder WithType(TileType type);
        ITileBuilder WithCost(int cost);
        ITileBuilder WithMesh(Mesh mesh);
        public Tile Build();
    }
}