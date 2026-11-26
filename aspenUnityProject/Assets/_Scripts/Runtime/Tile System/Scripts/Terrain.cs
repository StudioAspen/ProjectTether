using _Scripts.Runtime.Characters.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Tile_System.Scripts
{
    public class Terrain
    {

        public TerrainSO TerrainData { get; private set; } 
        /// <summary>
        /// Duration of the terrain. Negative if duration is infinite.
        /// </summary>
        public int Duration { get; private set; }
        
        public Mesh Mesh { get; private set; }
        
        

        //TODO: set up new data members 
        public Terrain(TerrainSO baseData)
        {
            Instantiate(baseData);
        }

        private void Instantiate(TerrainSO baseData) 
        {
           Duration = baseData.Duration; 
           TerrainData = baseData;
        }

        /// <summary>
        /// effects that happen when the unit enters the tile 
        /// </summary>
        public void OnEnter(UnitController unitController)
        {
           TerrainData.OnEnter(unitController); 
        }

        
        /// <summary>
        /// effects that continually happen
        /// </summary>
        public void Execute(UnitController unitController)
        {
            TerrainData.Execute(unitController);
            Duration--;
//            CombatFormulas.TerrainDamage(TerrainData.DamageOvertime, TerrainData.DamageTypes, unitController);
            //TODO: summoning not implemented yet
        }

        /// <summary>
        /// effects that happen when the unit leaves the tile
        /// </summary>
        public void OnExit(UnitController unitController)
        {
            AbstractTerrainData.OnExit(unitController);           
        }

        /// <summary>
        /// called when a terrain overrides another. Can either combine or replace, depending on values in SO
        /// </summary>
        /// <param name="other"></param>
        public void CombineWith(AbstractTerrainSO other)
        {
            if (AbstractTerrainData.Id == other.Id)
            {
                Duration += other.Duration;
            }
            else
            {
                foreach (var c in AbstractTerrainData.Combinations)
                {
                    if(c.toCombine.Id == other.Id)
                        AbstractTerrainData = c.fromCombine;
                }
            }
        }
    }
}
