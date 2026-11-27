using System.Collections.Generic;
using _Scripts.Runtime.Characters.Classes.Scripts;
using _Scripts.Runtime.Combat;
using Slinky.NodeWeb;
using UnityEngine;

namespace _Scripts.Runtime.Characters.Scripts
{
    [CreateAssetMenu(fileName = "SuitBaseData", menuName = "Scriptable Objects/Misc/SuitBaseData")]
    public class SuitBaseDataSO : ScriptableObject
    {
        [Tooltip("check CombatClassType")]
        [SerializeField] private CombatClassType _classType;

        public CombatClassType ClassType => _classType;
        
        [SerializeField] private CombatClassSO _combatClassData;
        public CombatClassSO CombatClassData => _combatClassData;
        
        //range limits are arbitrary  
        [SerializeField, Range(10, 150)] private int _energyCapacity; 
        public int EnergyCapacity => _energyCapacity;
        
        [SerializeField, Range(1, 10)] private int _skillCapacity;
        public int SkillCapacity => _skillCapacity;

        [SerializeField, Range(1, 10)] private int _abilityCapacity;
        public int AbilityCapacity => _abilityCapacity;
       
        [field: SerializeField, Tooltip("The skill tree for each suit")] public NodeWebGraphSO SkillTree {  get; private set; }
    }
}
