using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Entities.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using UnityEngine;

namespace _Scripts.Runtime.Math
{
    public static class GenericMathClass
    {
        public static int HexGridDistance(this Vector3Int from, Vector3Int to)
        {
            return (Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y) + Mathf.Abs(from.z - to.z)) / 2;
        }

        public static CubeCoordDirections GetDirection(this Vector2 vector)
        {
            switch (vector.x, vector.y)
            {
                case(1f,1f):
                    return CubeCoordDirections.NE;
                case(1f,-1f):
                    return CubeCoordDirections.SE;
                case(-1f,-1f):
                    return CubeCoordDirections.SW;
                case(-1f, 1f):
                    return CubeCoordDirections.NW;
                case(0f, 1f):
                    return CubeCoordDirections.N;
                case(0f,-1f):
                    return CubeCoordDirections.S;
                case(1f, 0f):
                    return CubeCoordDirections.NE;
                case(-1f, 0f):
                    return CubeCoordDirections.SW;
                default:
                    return CubeCoordDirections.N;
            }
        }

        public static bool AttackReachable(this UnitController cc, TileController target)
        {
            Vector3Int from = cc.TileCoords;
            bool tileWithinRange = target.tileCoordinate.HexGridDistance(from) <=
                                   cc.GetData().CombatClass.DefaultAttackRange;
            if (!tileWithinRange || target.UnitControllers.Count == 0)
                return false;
            bool targetContainsAlly = false;
            foreach (UnitController uc in target.UnitControllers) //ASSUMES tiles can only contain either enemy/ally. No mixing
            {
                if (uc.GetData().Faction == cc.GetData().Faction)
                    targetContainsAlly = true;
                break;
            }
            return !targetContainsAlly;
        }

        //currently no unique ranges for abilities. We can change this by designing a diff. HexGridDistance function
        public static bool AbilityReachable(this UnitController cc, AbilitySO ability, TileController target)
        {
            Vector3Int from = cc.TileCoords;
            bool tileWithinRange = target.tileCoordinate.HexGridDistance(from) <= ability.Range;
            if (!tileWithinRange)
                return false;
            bool targetContainsUnit = target.UnitControllers.Count > 0;
            bool targetContainsAlly = false;
            bool targetContainsEnemy = false;
            foreach (UnitController uc in target.UnitControllers) //ASSUMES tiles can only contain either enemy/ally. No mixing
            {
                if (uc.GetData().Faction == cc.GetData().Faction)
                    targetContainsAlly = true;
                else
                {
                    targetContainsEnemy = true;
                    break;
                }
            }

            return (targetContainsAlly && ability.HitsAllies) ||
                   (!targetContainsUnit && ability.CanTargetEmptyTile) ||
                   (targetContainsEnemy && ability.HitsEnemies);
        }
    }
}