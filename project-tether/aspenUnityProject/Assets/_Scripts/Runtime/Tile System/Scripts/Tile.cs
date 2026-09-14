using UnityEngine;

namespace TileSystem
{
    public class Tile
    {
        
        #region old code 
        public float DamageOvertime { get; private set; }
        public TileType TileType { get; private set; }
        public Vector3 Position { get; private set; }
        public int Cost { get; private set; }
        public Mesh Mesh { get; private set; }

        public Tile()
        {
            DamageOvertime = 0;
            TileType = TileType.Plain;
            Position = Vector3.zero;
            Cost = 0;
            Mesh = null;
        }
        
        

        public void SetDamageOvertime(float damageOvertime) => DamageOvertime = damageOvertime;
        public void SetTileType(TileType tileType) => TileType = tileType;
        public void SetPosition(Vector3 position) => Position = position;
        public void SetCost(int cost) => Cost = cost;
        public void SetMesh(Mesh mesh) => Mesh = mesh;
        
        #endregion

        //Does nothing for now because the MVP does not 
        //have custom tile effects 
        //TODO: set up new data members 
        public Tile(TileSO baseData)
        {
        }
    }
}
