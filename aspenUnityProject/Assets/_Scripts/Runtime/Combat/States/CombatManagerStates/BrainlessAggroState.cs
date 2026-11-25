using System;
using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Math;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    [CreateAssetMenu(fileName="BrainlessAggroState",menuName="Scriptable Objects/Behavior/BrainlessAggro")]
    public class BrainlessAggroState : EnemyStateSO
    {

        public override void Enter(EnemyCombatContext ecb)
        {
            var u = ecb.GetCurrentUnit().GetData();
            Debug.Log($"{u.Name} {u.HealthRemaining}");
            ecb.Target=GetNearestTarget(ecb);
            if (ecb.GetCurrentUnit().GetData().HealthRemaining * 1.0f <= ecb.GetCurrentUnit().GetData().Health * PercentHPAtExit)
                ecb.EFunProvider.PopState();

            DoBattle(ecb);
        }

        /*
        //can probably remove from both Enemy/Player phase but keep for debugging I guess
        public override void Update(EnemyPhase ep)
        {
        }
        */

        public override void Exit(EnemyCombatContext ecb)
        {
            //do nothing for now 
        }

        private void DoBattle(EnemyCombatContext ecb)
        {
            EnemyUnitController euc = ecb.GetCurrentUnit();
            if (euc.TileCoords.HexGridDistance(ecb.Target) > euc.GetData().DefaultAttackRange() && !euc.HasMoved)
                Move(ecb);
            if (ecb.EFunProvider.Attack())
                Debug.Log("enemy attacked");
            else
            {
                euc.GetData().Defend();
                Debug.Log("enemy defended");
            }

            Debug.Log("change turn");
            ecb.CombatContext.cfb.ChangeTurn();

        }

        //brainless targeting for now. Target closest player unit 
        private Vector3Int GetNearestTarget(EnemyCombatContext ecb)
        {
            //impossibly large distance for 19 tiles. Any number above 4 or 5 should work 
            int distance = 17017;
            Vector3Int currentTarget = new Vector3Int();
            foreach (AllyUnitController auc in ecb.CombatContext.AllyUnits)
            {
                int compare = auc.TileCoords.HexGridDistance(ecb.GetCurrentUnit().TileCoords);
                if (compare < distance)
                {
                    distance = compare;
                    currentTarget = auc.TileCoords;
                }
            }
            return currentTarget;
        }

        //brainless aggro, so constantly moves towards player unit
        private void Move(EnemyCombatContext ecb)
        {
           Vector3Int currPos = ecb.GetCurrentUnit().TileCoords; 
           Vector3Int newPos = new Vector3Int();
           int currDistance = currPos.HexGridDistance(ecb.Target);
           foreach(CubeCoordDirections direction in Enum.GetValues(typeof(CubeCoordDirections)))
           {
               Vector3Int triedVec = currPos + direction.Vector();
               if (ecb.EFunProvider.CanMove(currPos, triedVec, 1) && triedVec.HexGridDistance(ecb.Target) < currDistance)
               {
                   newPos = triedVec;
                   break;
               }
           }
           ecb.EFunProvider.HandleMove(currPos, newPos, 1);
        }

        //ignore for now 
        private void UseAbility()
        {
            
        }

    }

}