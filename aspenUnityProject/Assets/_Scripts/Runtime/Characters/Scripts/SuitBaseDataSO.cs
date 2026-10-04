using System.Collections.Generic;
using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Characters.Scripts
{
    [CreateAssetMenu(fileName = "SuitBaseData", menuName = "Scriptable Objects/Misc/SuitBaseData")]
    public class SuitBaseDataSO : ScriptableObject
    {
        //range limits are arbitrary  
        [SerializeField, Range(10, 150)] private int _energyCapacity; 
        public int EnergyCapacity => _energyCapacity;
        
        [SerializeField, Range(1, 10)] private int _skillCapacity;
        public int SkillCapacity => _skillCapacity;

        [SerializeField, Range(1, 10)] private int _abilityCapacity;
        public int AbilityCapacity => _abilityCapacity;
        
        //List 
    }
}
