using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Runtime.Combat
{
    /// <summary>
    /// The canonical center-out tile ordering used by combat runtime and encounter data.
    /// Tile indices in encounter assets map to these cube coordinates.
    /// </summary>
    public static class CombatTileCoordinates
    {
        public const int Radius = 2;
        public const int TileCount = 3 * Radius * (Radius + 1) + 1;

        private static readonly IReadOnlyList<Vector3Int> CoordinatesByTileIndex = BuildCoordinates();

        public static IReadOnlyList<Vector3Int> GetAll() => CoordinatesByTileIndex;

        public static Vector3Int GetCoordinate(int tileIndex)
        {
            if (tileIndex < 0 || tileIndex >= TileCount)
                throw new ArgumentOutOfRangeException(nameof(tileIndex));

            return CoordinatesByTileIndex[tileIndex];
        }

        public static bool TryGetTileIndex(Vector3Int coordinate, out int tileIndex)
        {
            for (int i = 0; i < CoordinatesByTileIndex.Count; i++)
            {
                if (CoordinatesByTileIndex[i] != coordinate)
                    continue;

                tileIndex = i;
                return true;
            }

            tileIndex = -1;
            return false;
        }

        private static IReadOnlyList<Vector3Int> BuildCoordinates()
        {
            List<Vector3Int> coordinates = new List<Vector3Int>(TileCount) { Vector3Int.zero };
            Vector3Int current = Vector3Int.zero;

            for (int ring = 1; ring <= Radius; ring++)
            {
                current += CubeCoordDirections.NE.Vector();
                coordinates.Add(current);

                for (int southEastSteps = ring - 1; southEastSteps > 0; southEastSteps--)
                {
                    current += CubeCoordDirections.SE.Vector();
                    coordinates.Add(current);
                }

                for (int direction = (int)CubeCoordDirections.S; direction < 6; direction++)
                {
                    for (int steps = ring; steps > 0; steps--)
                    {
                        current += ((CubeCoordDirections)direction).Vector();
                        coordinates.Add(current);
                    }
                }
            }

            return coordinates.AsReadOnly();
        }
    }
}
