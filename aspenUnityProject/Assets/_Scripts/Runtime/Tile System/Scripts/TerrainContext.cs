using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Tile_System.Scripts
{
    public class TerrainContext
    {
       //for terrains that have effects requiring stacks 
       public int Stacks { get; set; } = 0;
       public TileController tc;
       
       /// <summary>
       /// unit that made the terrain -> will be used for determining kill credit
       /// </summary>
       public UnitController applier; 

       public int Duration { get; set; } = 0;
       public TerrainSO TerrainSOData { get; set; }
       public Mesh Mesh { get; set; }
       
       public BattleUnitsList AllUnitInfo { get; set; } 
       
       /// <summary>
       /// other terrain that is being combined with current
       /// </summary>
       public TerrainSO OtherTerrainSOData { get; set; }
    }
}