using UnityEngine;
using System.Collections.Generic;

namespace _Scripts.Runtime.Persistent_Data
{
    [System.Serializable]
    public class PlayerData
    {
        public Vector3 OverworldPosition = Vector3.zero;
        public List<string> DefeatedOverworldEnemyIds = new List<string>();
    }
}
