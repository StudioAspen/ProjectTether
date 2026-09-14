using UnityEngine;

namespace TileSystem
{
    // Follows the builder design pattern
    public class TileBuilder : ITileBuilder
    {
        Tile _tile;

        public TileBuilder()
        {
            _tile = new Tile();
        }

        public Tile Build()
        {
            return _tile;
        }

        public ITileBuilder WithDamageOvertime(float value)
        {
            _tile.SetDamageOvertime(value);
            return this;
        }

        public ITileBuilder WithPosition(Vector3 pos)
        {
            _tile.SetPosition(pos);
            return this;
        }

        public ITileBuilder WithType(TileType type)
        {
            _tile.SetTileType(type);
            return this;
        }

        public ITileBuilder WithCost(int cost)
        {
            _tile.SetCost(cost);
            return this;
        }

        public ITileBuilder WithMesh(Mesh mesh)
        {
            _tile.SetMesh(mesh);
            return this;
        }
    }
}
