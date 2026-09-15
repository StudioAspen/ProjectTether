using Consystently.Essentials.Math;
using Tether.CharacterSystems;
using UnityEngine;

namespace Consystently.Essentials
{
    public class EnemyAggressiveState : ActionState
    {
        private EnemyUnitController enemy;
        private Vector3Int target;
        public EnemyAggressiveState(CombatManager combatManager, EnemyPhase battlePhase) : base(combatManager,
            battlePhase)
        {
        }
        
        //in enter instead of constructor because constructed once 
        public override void Enter()
        {
            enemy = (EnemyUnitController)CombatManager.TurnOrder[CombatManager.CurrentUnitTurn];
            target = GetNearestTarget();
        }

        public override void Update()
        {
            if (enemy.TileCoords.HexGridDistance(target) > enemy.GetData().DefaultAttackRange())
                Move(target);
        }

        public override void Exit()
        {
            ((EnemyPhase)BattlePhase).PopState();
        }

        //brainless targeting for now 
        private Vector3Int GetNearestTarget()
        {
            //impossibly large distance for 19 tiles. Any number above 4 or 5 should work 
            int distance = 17017;
            Vector3Int currentTarget = new Vector3Int();
            foreach (AllyUnitController auc in CombatManager.PlayerUnits)
            {
                int compare = auc.TileCoords.HexGridDistance(enemy.TileCoords);
                if (compare < distance)
                {
                    distance = compare;
                    currentTarget = auc.TileCoords;
                }
            }
            return currentTarget;
        }

        private void Attack()
        {
        }

        private void Move(Vector3Int newPos)
        {
           enemy.TryMove(CombatManager.GetTileController(newPos).Position(),  newPos);
        }

        //ignore for now 
        private void UseAbility()
        {
            
        }

    }

}