using System;
using System.Collections.Generic;
using Consystently.Essentials.Math;
using Tether.CharacterSystems;
using TileSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Consystently.Essentials
{
    //current implementation is for the mvp 
    public class EnemyPhase : BattlePhase
    {

        //since we're using a stack already, we could represent enemy 'phases' with them quite easily
        //e.g., when the enemy is low on hp, pop the aggressive state and drop them into a defensive one
        private Stack<EnemyStateSO> stateStack = new Stack<EnemyStateSO>();
        private const int ArbitraryOffset = 10;


        private List<EnemyStateSO> behaviourStates;
        public EnemyUnitController Euc {get; private set;}  
        public Vector3Int Target {get; private set;}

        public EnemyPhase(CombatManager combatManager) : base(combatManager) { }

        public override void Enter()
        {
            Debug.Log("enemy phase entered");
            stateStack.Clear();
            Euc = (EnemyUnitController)CombatManager.GetCurrentUnit();
            Debug.Log(((EnemyUnit)Euc.GetData()).behaviourStates[0]);
            behaviourStates = ((EnemyUnit)Euc.GetData()).behaviourStates;
            PushState();
        }

        /*
        public override void Update()
        {
            if(stateStack.Count > 0) 
                stateStack.Peek().Update(this);
        }
        */

        public override void Exit()
        {
            stateStack.Clear();
        }

        //this will simply reset their behavior stack
        public override void PushState()
        {
            stateStack.Clear();
            foreach (EnemyStateSO eso in behaviourStates)
            {
                stateStack.Push(eso);
                Debug.Log(eso);
            }
            if (stateStack.Count > 0)
                stateStack.Peek().Enter(this);
        }

        //to be used when the enemy transitions 'phases' in the future.
        //e.g., an enemy is attacked and loses enough hp to transition into 
        //a cautious state
        public void PopState()
        {
            if (stateStack.Count == 0)
                return;
            stateStack.Pop().Exit(this);
            if(stateStack.Count == 0)
                Debug.Log("Enemy should be dead before/at this stage in the future implementation"); 
            else
                stateStack.Peek().Enter(this);
        }
        
        public void SetTarget(Vector3Int target)
        {
            Target = target;
        }

        //will be the same for every EnemyStateSO
        public bool Attack()
        {
            if (Euc.GetData().DefaultAttackRange() < Target.HexGridDistance(Euc.TileCoords))
                return false;
            foreach (UnitController receiver in CombatManager.tileControllers[CombatManager.TileCubeCoords[Target]].UnitControllers) 
                CombatFormulas.Damage(Euc, Euc.GetData().DefaultAttackTypes(), receiver);  
            return true;
        }

        public bool CanMove(Vector3Int from, Vector3Int to, int range)
        {
            return CombatManager.TileCubeCoords.ContainsKey(to) && CombatManager.GetTileController(to).IsMoveable(from, range, Faction.Enemy);
        }

        public void HandleMove(Vector3Int from, Vector3Int to, int range)
        {
            if(!CanMove(from, to, range))
                return;
            TileController selectedTile = CombatManager.GetTileController(to);
            selectedTile.AddUnit(CombatManager.GetCurrTileController().RemoveUnit(Euc));
            Euc.TryMove(selectedTile.Position(),selectedTile.tileCoordinate); 
            selectedTile.RepositionUnits(ArbitraryOffset);
        }
        
        //I guess man 
        public CombatManager GetGM()
        {
            return CombatManager;
        }
            
    }
}