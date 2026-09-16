using System;
using Consystently.Essentials.Math;
using Tether.CharacterSystems;
using UnityEngine;

namespace Consystently.Essentials
{
    [CreateAssetMenu(fileName="BrainlessAggroState",menuName="Scriptable Objects/Behavior/BrainlessAggro")]
    public class BrainlessAggroState : EnemyStateSO
    {

        //percentage 
        [SerializeField] private float percentHPAtExit;
        //add weights or something 
        
        //I believe we need to pass ep because of how scriptable objects work
        public override void Enter(EnemyPhase ep)
        {
            ep.SetTarget(GetNearestTarget(ep));
            if (ep.Euc.GetData().HealthRemaining * 1.0 <= ep.Euc.GetData().Health * percentHPAtExit)
                ep.PopState();

            DoBattle(ep);
        }

        /*
        //can probably remove from both Enemy/Player phase but keep for debugging I guess
        public override void Update(EnemyPhase ep)
        {
        }
        */

        public override void Exit(EnemyPhase ep)
        {
            //do nothing for now 
        }

        private void DoBattle(EnemyPhase ep)
        {
            if (ep.Euc.TileCoords.HexGridDistance(ep.Target) > ep.Euc.GetData().DefaultAttackRange() && !ep.Euc.HasMoved)
                Move(ep.Target, ep);
            if (ep.Attack())
                Debug.Log("enemy attacked");
            else
            {
                ep.Euc.GetData().Defend();
                Debug.Log("enemy defended");
            }

            ep.GetGM().ChangeTurn();
        }

        //brainless targeting for now 
        private Vector3Int GetNearestTarget(EnemyPhase ep)
        {
            //impossibly large distance for 19 tiles. Any number above 4 or 5 should work 
            int distance = 17017;
            Vector3Int currentTarget = new Vector3Int();
            foreach (AllyUnitController auc in ep.GetGM().PlayerUnits)
            {
                int compare = auc.TileCoords.HexGridDistance(ep.Euc.TileCoords);
                if (compare < distance)
                {
                    distance = compare;
                    currentTarget = auc.TileCoords;
                }
            }
            return currentTarget;
        }

        private void Move(Vector3Int target, EnemyPhase ep)
        {
           Vector3Int currPos = ep.Euc.TileCoords; 
           Vector3Int newPos = new Vector3Int();
           int currDistance = currPos.HexGridDistance(ep.Target);
           foreach(CubeCoordDirections direction in Enum.GetValues(typeof(CubeCoordDirections)))
           {
               Vector3Int triedVec = currPos + direction.Vector();
               if (ep.CanMove(currPos, triedVec, 1) && triedVec.HexGridDistance(ep.Target) < currDistance)
               {
                   newPos = triedVec;
                   break;
               }
           }
           ep.HandleMove(currPos, newPos, 1);
        }

        //ignore for now 
        private void UseAbility()
        {
            
        }

    }

}