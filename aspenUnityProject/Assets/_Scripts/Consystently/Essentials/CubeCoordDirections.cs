<<<<<<< HEAD
=======
using System;
using UnityEngine;

>>>>>>> production/battle-scene
namespace Consystently.Essentials
{
    public enum CubeCoordDirections
    {
       SE,
       S,
       SW,
       NW,
       N,
       NE
    }
<<<<<<< HEAD
}
=======

    public static class CubeCoordDirectionsExtensions
    {
       public static Vector3Int Vector(this CubeCoordDirections cubeCoordDirections) => cubeCoordDirections switch
       {
          CubeCoordDirections.SE => new Vector3Int(0, -1, 1),
          CubeCoordDirections.S => new Vector3Int(-1, 0, 1),
          CubeCoordDirections.SW => new Vector3Int(-1, 1, 0),
          CubeCoordDirections.NW => new Vector3Int(0, 1, -1),
          CubeCoordDirections.N => new Vector3Int(1, 0, -1),
          CubeCoordDirections.NE => new Vector3Int(1, -1, 0),
          _ => throw new ArgumentOutOfRangeException(nameof(cubeCoordDirections), cubeCoordDirections, null)
       };

    }
}

/*
#region directions
private readonly Vector3Int[] directions = new Vector3Int[] {
   new Vector3Int(0,-1,1), //SE 
   new Vector3Int(-1,0,1), //S
   new Vector3Int(-1,1,0), //SW
   new Vector3Int(0, 1,-1), //NW
   new Vector3Int(1, 0,-1), //N
   new Vector3Int(1, -1, 0) //NE
};

*/
>>>>>>> production/battle-scene
