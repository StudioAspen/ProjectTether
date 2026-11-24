using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Abilities.AdditionalAbilityEffects
{
    [CreateAssetMenu(fileName = "ExampleAbilityEffect2" , menuName = "Scriptable Objects/Combat/Abilities/AbilityEffects/ExampleAbilityEffect2")]
    public class ExampleAbilityEffect2SO : AbilityEffectSO
    {
        [SerializeField] private List<UnitDataSO> _unitsSummoned;

        /// <summary>
        /// don't actually use this for stuff like microbots. It might be easier to use terrains for microbots 
        /// </summary>
        /// <param name="ab"></param>
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