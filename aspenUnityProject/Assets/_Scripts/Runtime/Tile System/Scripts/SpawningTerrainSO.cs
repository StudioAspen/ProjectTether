using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts.Combat;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Runtime.Tile_System.Scripts
{
    
    [CreateAssetMenu(fileName = "SpawningTerrainSO", menuName = "Scriptable Objects/Terrain/SpawningTerrain")]
    public class SpawningTerrainSO : AbstractTerrainSO
    {
        
       [Tooltip("The entities this terrain type can spawn")] 
       [SerializeField] private List<UnitController> _entitiesSpawned;
       public List<UnitController> EntitiesSpawned => _entitiesSpawned;

       [SerializeField] private int _spawnRadius;
       public int SpawnRadius => _spawnRadius;
       
       public override void OnEnter(UnitController unitController)
       {
           base.OnEnter(unitController);
           //extra logic here 
       }

       public override void Execute(UnitController unitController)
       {
           base.Execute(unitController);
           //logic for spawning
       }

       public override void OnExit(UnitController unitController)
       {
           base.OnExit(unitController);
           //extra logic here 
       }
       
    }
}