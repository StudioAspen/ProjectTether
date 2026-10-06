using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Characters.Scripts
{
    [CreateAssetMenu(fileName = "SuitBaseData", menuName = "Scriptable Objects/Misc/SuitBaseData")]
    public class SuitBaseDataSO : ScriptableObject
    {
        //possibly remove if we decide to use an enum for classes and have them in order
        [Tooltip("check CombatClassType")]
        [SerializeField, Range(0,7)] private int _classType;

        public int ClassType => _classType;
        
        //range limits are arbitrary  
        [SerializeField, Range(10, 150)] private int _energyCapacity; 
        public int EnergyCapacity => _energyCapacity;
        
        [SerializeField, Range(1, 10)] private int _skillCapacity;
        public int SkillCapacity => _skillCapacity;

        [SerializeField, Range(1, 10)] private int _abilityCapacity;
        public int AbilityCapacity => _abilityCapacity;
        
        //TODO:
        //Node tree
    }
}
