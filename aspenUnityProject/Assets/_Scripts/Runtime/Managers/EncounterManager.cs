using System;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Consystently.Essentials;
using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Combat.States;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Managers.Game_States;
using _Scripts.Runtime.Tile_System.Scripts;


namespace _Scripts.Runtime.Managers
{
    /// <summary>
    /// the class for passing data from the overworld (and battle sim) into battle scene  
    /// </summary>
    public class EncounterManager : Manager<EncounterManager>
    {
        //probably stupid way of implementing this 
        private UnitDataSO[,] initializerData = new UnitDataSO[19,4];
        
        //NOT useful yet. Will be useful when we decide to add tile effects 
        private TileSO[] tileInitializerData = new TileSO[19];
        
        //initial unit formation generated from BattlefieldMap for the mvp 
        private Encounter encounter;
        
        //false from not battle sim
        //static because too lazy to fix initialization issues with .instance 
        public static event Action<bool> encountered; 
        
        //probably useless 
        void OnEnable()
        {
            GameManager.Instance.ChangedGameState += HandleState;
        }


        //useless 
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


        //TODO: when we add tile effects, add tile data to generate methods
        /// <summary>
        /// used for the battle sim  
        /// </summary>
        /// <param name="tiles"></param>
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
        /// <summary>
        /// generates an encounter from a encounterSO. Used for the overworld
        /// </summary>
        /// <param name="encounterSo"></param>
        public void GenerateEncounter(EncounterSO encounterSo)
        {
           encounter = new Encounter();
           
        }

    }
}