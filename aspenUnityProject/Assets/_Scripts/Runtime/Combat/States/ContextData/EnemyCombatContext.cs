using _Scripts.Runtime.Entities.Scripts;
using UnityEngine;

namespace _Scripts.Runtime.Combat.States.ContextData
{
    public class EnemyCombatContext
    {
        public CombatContext CombatContext  { get; private set; }
        public IEnemyFunctionProvider EFunProvider { get; private set; }

        public Vector3Int Target { get; set; }

        public EnemyCombatContext(CombatContext combatContext, IEnemyFunctionProvider eFunProvider)
        {
            CombatContext = combatContext;
            EFunProvider = eFunProvider;
        }

        public EnemyUnitController GetCurrentUnit()
        {
            return (EnemyUnitController)CombatContext.CombatFunctionProvider.GetCurrentUnit();
        }
    }
}