using Tether.CharacterSystems;
using UnityEngine;

namespace _Scripts.Runtime.Misc
{
    //ares will contain spawn pools that contain enemy game objects. 
    [CreateAssetMenu(fileName = "New Area", menuName = "Scriptable Objects/Misc/Area")]
    public class AreaSO : ScriptableObject
    {
        [SerializeField] private UnitDataSO[] enemyPool;

        public UnitDataSO[] EnemyPool => enemyPool;
    }
}