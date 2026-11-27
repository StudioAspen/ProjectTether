using _Scripts.Runtime.Characters.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Runtime.Tile_System.Scripts
{
    //possibly redundant 
    public class Terrain
    {

        public TerrainContext TerrainData { get; private set; } 
        /// <summary>
        /// Duration of the terrain. Negative if duration is infinite.
        /// </summary>
        

        //TODO: set up new data members 
        public Terrain(TerrainSO baseData)
        {
            TerrainData = new TerrainContext()
            {
                TerrainSOData = baseData,
                Duration = baseData.Duration,
                Mesh = baseData.Mesh
            };
        }

        public Terrain(TerrainContext terrain)
        {
            TerrainData = terrain;
        }
        

        /// <summary>
        /// effects that happen when the unit enters the tile 
        /// </summary>
        public void OnEnter(UnitController unitController)
        {
           TerrainData.TerrainSOData.OnEnter(TerrainData, unitController); 
        }
        
       /// <summary>
        /// effects that happen when the unit leaves the tile
        /// </summary>
        public void OnExit(UnitController unitController)
        {
            TerrainData.TerrainSOData.OnExit(TerrainData, unitController);           
        }

        /// <summary>
        /// called when a terrain overrides another. Can either combine or replace, depending on values in SO
        /// </summary>
        /// <param name="other"></param>
        public void CombineWith(TerrainContext context)
        {
            TerrainData.TerrainSOData.OnCombine(context);
        }
    }
}
