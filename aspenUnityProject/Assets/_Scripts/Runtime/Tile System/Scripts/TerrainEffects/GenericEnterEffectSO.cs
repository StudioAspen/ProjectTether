using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine; 

namespace _Scripts.Runtime.Tile_System.Scripts.TerrainEffects
{
    /// <summary>
    /// create one instance only. Generic enter effect that can be slotted to 
    /// </summary>
    [CreateAssetMenu(fileName = "TerrainGenericEnterEffect", menuName = "Scriptable Objects/Terrain/TerrainEffects/GenericEnterEffect")]
    public class GenericEnterEffectSO : TerrainEffectSO
    {
        public override void Execute(TerrainContext context, params UnitController[] specificUnit)
        {
            if (specificUnit == null)
                return;
            TerrainSO tsd = context.TerrainSOData;
            
            //only apply on units entering
            foreach(UnitController uc in specificUnit)
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
                CombatFormulas.TerrainDamage(tsd.DamageOnEnter,tsd.DamageTypes,uc);
            }
        }
    }
}