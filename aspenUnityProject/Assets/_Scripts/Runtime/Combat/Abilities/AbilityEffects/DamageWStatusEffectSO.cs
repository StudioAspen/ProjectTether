using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Abilities.AbilityEffects
{
    /// <summary>
    /// Create only one instance. Basic damage with status effect that also changes terrain of target tile. 
    /// </summary>
    [CreateAssetMenu(fileName = "DamageWStatus" , menuName = "Scriptable Objects/Combat/Abilities/AbilityEffects/DamageWStatusEffect")]
    public class DamageWStatusEffectSO :  AbilityEffectSO {
    
        
        public override void Execute(AbilityContext ab)
        {
            bool newTerrain = false;
            if (ab.AbilityData.TileEffect != null)
            {
                TerrainContext tContext = new TerrainContext()
                {
                    tc = ab.Target,
                    TerrainSOData = ab.AbilityData.TileEffect,
                    AllUnitInfo = ab.AllUnitInfo,
                    applier = ab.Caster,
                };
                if (ab.Target.Terrains.Count > 0)
                {
                    foreach (var t in ab.Target.Terrains)
                    {
                        t.TerrainData.OtherTerrainSOData = ab.AbilityData.TileEffect; 
                        t.CombineWith(t.TerrainData);
                    }
                }
                ab.Target.AddTerrain(tContext);
                newTerrain = true;
            }

            foreach (UnitController enemy in ab.Target.UnitControllers)
            {
                CombatFormulas.AbilityDamage(ab.Caster, enemy, ab.AbilityData);
                if (newTerrain)
                    ab.Target.Terrains[^1].OnEnter(enemy);
                foreach (StatusSO i in ab.AbilityData.Status)
                {
                    StatusContext statusContext = new StatusContext()
                    {
                        Applier = ab.Caster,
                        Applied = enemy,
                        Target = ab.Target,
                        StatusSOData = i, 
                        AllUnitInfo = ab.AllUnitInfo,
                    };
                    enemy.GetData().AddStatus(statusContext);
                }
            }

        }    
    }
}