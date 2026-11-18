using _Scripts.Runtime.Characters.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Tile_System.Scripts
{
    public class Terrain
    {

        public AbstractTerrainSO AbstractTerrainData { get; private set; } 
        /// <summary>
        /// Duration of the terrain. Negative if duration is infinite.
        /// </summary>
        public int Duration { get; private set; }
        
        public Mesh Mesh { get; private set; }
        
        

        //TODO: set up new data members 
        public Terrain(AbstractTerrainSO baseData)
        {
            Instantiate(baseData);
        }

        private void Instantiate(AbstractTerrainSO baseData) 
        {
           Duration = baseData.Duration; 
           AbstractTerrainData = baseData;
        }

        /// <summary>
        /// effects that happen when the unit enters the tile 
        /// </summary>
        public void OnEnter(UnitController unitController)
        {
           AbstractTerrainData.OnEnter(unitController); 
        }

        
        /// <summary>
        /// effects that continually happen
        /// </summary>
        public void Execute(UnitController unitController)
        {
            AbstractTerrainData.Execute(unitController);
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
