using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace _Scripts.Runtime.Misc
{
    //for creating new encounters within the game.
    //because unitPositions starts at index 0, there may be off-by-one errors. 
    //If they persist, initiate values of unitPositions as -1.
    //TODO: get rid of magic numbers 
    public class Encounter
    {
        //19 tiles, max 4 units per tile
        private GameObject[,] tiles = new GameObject[19, 4];
        
        //represents the positions within the tile - the indices cap at 3
        //used for determining the ordering of units when adding them to the tile
        private int[] unitPositions = new int[19];
        
        public GameObject this[int tile, int unitPosition]
        {
            get => tiles[tile, unitPosition];
        }

        //if replace not successful, return false. bool used for potential future conditional popups triggered by managers. 
        public bool ChangeUnit(int tile, int unitPosition, GameObject newUnit)
        {
            if (unitPosition > unitPositions[tile]-1)
            {
                Debug.Log($"no unit to replace at ({tile}, {unitPosition})");
                return false;
            }
            tiles[tile, unitPosition] = newUnit;
            return true;
        }

        public bool AddUnit(int tile, GameObject newUnit)
        {
            if (unitPositions[tile] >= 4)
            {
                Debug.Log($"max units at tile {tile}");
                return false;
            }
            tiles[tile,unitPositions[tile]] = newUnit;
            unitPositions[tile]++;
            for (int i = unitPositions[tile]; i < 4; i++)
            {
                if (tiles[tile, i] != null)
                {
                    unitPositions[tile]++;
                    break;
                }
            }
            return true;
        }


        public bool RemoveUnit(int tile, int unitPosition)
        {
            if (unitPositions[tile] < 1 || tiles[tile, unitPosition] == null)
            {
                Debug.Log($"no unit to remove at ({tile}, {unitPosition})");
                return false;
            }
            tiles[tile,unitPosition] = null;
            unitPositions[tile] = unitPosition+1;
            return true;
        }

        public int TotalTiles()
        {
            return tiles.GetLength(0);
        }

        public int MaxUnitsPerTile()
        {
            return tiles.GetLength(1); 
        }

        public int UnitCountAtTile(int tile)
        {
            return unitPositions[tile];
        }

        public void Validate()
        {
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(tiles[i,j] != null)
                        Debug.Log($"tile {i}: {unitPositions[i]} ");
                }
            }
        }
        
    }
}