using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Abilities.AdditionalAbilityEffects
{
    public class ExampleAbilityEffect2SO : AbilityEffectSO
    {
        [SerializeField] private List<UnitDataSO> _unitsSummoned;

        public override void Execute(AbilityContext ab)
        {
            foreach (var unitSummoned in _unitsSummoned)
            {
               UnitController unitController = Instantiate(unitSummoned.Model, ab.Target.Position(), Quaternion.Euler(-90F,0,0));
               ab.AllUnitInfo.AddUnit(unitController);
               ab.Target.AddUnit(unitController);
               ab.Target.RepositionUnits();
            }
        }

    }
}