using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Tile_System.Scripts.TerrainEffects
{
    [CreateAssetMenu(fileName = "TerrainGenericStandEffect", menuName = "Scriptable Objects/Terrain/TerrainEffects/GenericStandEffect")]
    public class GenericStandEffectSO : TerrainEffectSO
    {
        public override void Execute(TerrainContext context, params UnitController[] specificUnit)
        {
            TerrainSO tsd = context.TerrainSOData;
        
            //only apply on units entering
            foreach(UnitController uc in context.tc.UnitControllers)
            {
                foreach (StatusSO status in tsd.Statuses)
                {
                    //probably create context instead but whatever 
                    Status s = new Status(status);
                    s.StatusData.Applied = uc;
                    s.StatusData.Applier = context.applier;
                    s.StatusData.AllUnitInfo = context.AllUnitInfo;
                    uc.GetData().AddStatus(s.StatusData);
                }
                CombatFormulas.TerrainDamage(tsd.DamageOvertime,tsd.DamageTypes,uc);
            }
        }
    }
}