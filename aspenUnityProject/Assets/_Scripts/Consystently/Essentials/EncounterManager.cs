using System;
using System.Collections.Generic;
using _Scripts.Runtime.Misc;
using Tether.CharacterSystems;
using UnityEngine;

namespace Consystently.Essentials
{
    public class EncounterManager : Manager<EncounterManager>
    {
        //probably stupid way of implementing this 
        private UnitDataSO[,] initializerData = new UnitDataSO[19,4];
        
        //NOT useful yet. Will be useful when we decide to add tile effects 
        private TileSO[] tileInitializerData = new TileSO[19];
        
        //initial unit formation generated from BattlefieldMap for the mvp 
        private Encounter encounter;
        public static event Action encountered; 
        
        void OnEnable()
        {
            GameManager.ChangedGameState += HandleState;
        }


        void HandleState(GameState gameState)
        {
            //switch statement here if it ever becomes useful
        }

       //returns 2d Unit array (for the data) 
        public Encounter GetEncounter()
        {
            return encounter;
        }


        public UnitDataSO[,] GetInitializerData()
        {
            return initializerData;
        }

        //called from EnemyUnitController
        //update to generate encounter type 
        public void StartEncounter(EncounterSO encounterSo)
        {
           GenerateEncounter(encounterSo);
           encountered?.Invoke();
        }

        //if ever add randomized enemy positioning for encounters, we will have to update and use this function. 
        /* 
        public void StartEncounter(Encounter encounter)
        {
           this.encounter = encounter; 
           GameManager.Instance.ChangeGameState(new CombatGameState(GameManager.Instance)); 
        }
        */

        public void StartEncounter()
        {
            encountered?.Invoke();
        }

        //TODO: when we add tile effects, add tile data to generate methods
        //for mvp primarily 
        public void GenerateEncounter(List<BattlefieldTile> tiles)
        {
           encounter = new Encounter();
           Debug.Log($"tiles length: {tiles.Count}");
           for (int tile = 0; tile < tiles.Count; tile++)
           {
               if (tiles[tile].FilledSlots.Count < 1)
                   continue; 
               List<UnitPieceSlot> filledSlots = tiles[tile].FilledSlots;
               
               for (int unit = 0; unit < filledSlots.Count; unit++)
               {
                   UnitSim sim = filledSlots[unit].Piece?.Sim;
                   encounter.AddUnit(tile, (sim?.Data.Model));
                   initializerData[tile, unit] = (sim?.Data);
//                   Debug.Log($"tile:{tile} | unit:{unit}");
//                  Debug.Log(FilledSlots[unit].Piece.Sim.Data.Model);
               }
           }
//           encounter.Validate();
        }

        //TODO: finish this function when we get to the overworld or level selection  
        public void GenerateEncounter(EncounterSO encounterSo)
        {
           encounter = new Encounter();
           
        }

    }
}