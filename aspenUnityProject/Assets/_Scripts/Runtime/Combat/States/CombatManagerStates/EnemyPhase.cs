using System.Collections.Generic;
using _Scripts.Runtime.Combat.States.ContextData;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Math;
using _Scripts.Runtime.Tile_System.Scripts;
using UnityEngine;

namespace _Scripts.Runtime.Combat.States.CombatManagerStates
{
    //current implementation is for the mvp 
    public class EnemyPhase : BattlePhase, IEnemyFunctionProvider
    {

        //since we're using a stack already, we could represent enemy 'phases' with them quite easily
        //e.g., when the enemy is low on hp, pop the aggressive state and drop them into a defensive one
        private Stack<EnemyStateSO> stateStack = new Stack<EnemyStateSO>();
        private const int ArbitraryOffset = 10;


        private List<EnemyStateSO> behaviourStates;
        public EnemyUnitController Euc {get; private set;}  

        public EnemyPhase(CombatContext combatContext) : base(combatContext) { }

        private EnemyCombatContext ecb;
        public override void Enter()
        {
            ecb = new EnemyCombatContext(CombatContext, this);
            Debug.Log("enemy phase entered");
            stateStack.Clear();
            Euc = (EnemyUnitController)CombatContext.CombatFunctionProvider.GetCurrentUnit();
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
                stateStack.Peek().Enter(ecb);
        }

        //to be used when the enemy transitions 'phases' in the future.
        //e.g., an enemy is attacked and loses enough hp to transition into 
        //a cautious state
        public override void PopState()
        {
            if (stateStack.Count == 0)
                return;
            stateStack.Pop().Exit(ecb);
            if(stateStack.Count == 0)
                Debug.Log("Enemy should be dead before/at this stage in the future implementation"); 
            else
                stateStack.Peek().Enter(ecb);
        }
        
        public void SetTarget(Vector3Int target)
        {
            ecb.Target = target;
        }

        #region reusable functions for various enemy states 
        //attack will probably be the same for every EnemyStateSO
        public bool Attack()
        {
            if (Euc.GetData().DefaultAttackRange() < ecb.Target.HexGridDistance(Euc.TileCoords))
                return false;
            foreach (UnitController receiver in CombatContext.TileControllers[CombatContext.TileCubeCoords[ecb.Target]].UnitControllers) 
                CombatFormulas.Damage(Euc, Euc.GetData().DefaultAttackTypes(), receiver);  
            return true;
        }

        public bool CanMove(Vector3Int from, Vector3Int to, int range)
        {
            return CombatContext.TileCubeCoords.ContainsKey(to) && CombatContext.CombatFunctionProvider.GetTileController(to).IsMoveable(from, range, Faction.Enemy);
        }

        public void HandleMove(Vector3Int from, Vector3Int to, int range)
        {
            if(!CanMove(from, to, range))
                return;
            TileController selectedTile = CombatContext.CombatFunctionProvider.GetTileController(to);
            selectedTile.AddUnit(CombatContext.CombatFunctionProvider.GetCurrTileController().RemoveUnit(Euc));
            Euc.TryMove(selectedTile.Position(),selectedTile.tileCoordinate); 
            selectedTile.RepositionUnits(ArbitraryOffset);
        }

        //I guess bro
        public ICombatFunctionProvider GetGM()
        {
            return CombatContext.CombatFunctionProvider;
        }
        #endregion
    }
}