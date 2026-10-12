using System.Collections.Generic;
using _Scripts.Runtime.Characters.Classes.Scripts;
using UnityEngine;

namespace _Scripts.Runtime.Combat
{
    [System.Serializable]
    public struct StatModifier
    {
        public CombatClassType classType;
        public List<StatMultiplier> statsModified;
    }

    [System.Serializable]
    public struct StatMultiplier
    {
        public float multiplier;
        public Stat stat;
    }
    
    //no clue what to call this 
    [System.Serializable]
    public struct StatusPoint
    {
        public int turnToAdd;
        public StatusSO statusToAdd;
    }

    [CreateAssetMenu(fileName = "StatusSO", menuName = "Scriptable Objects/Misc/StatusSO")]
    public class StatusSO : ScriptableObject
    {
        [SerializeField] private string _statusId; 
        public string StatusId => _statusId;
    
        [SerializeField] private string _statusName;
        public string StatusName => _statusName;

        [SerializeField] private string _statusDescription;
        public string StatusDescription => _statusDescription;

        [Tooltip("1 for no extra damage taken")]
        [SerializeField] private float _damageTakenMultiplier;
        public float DamageTakenMultiplier => _damageTakenMultiplier;
    
        [SerializeField] private int _damageTakenPerTurn;
        public int  DamageTakenPerTurn => _damageTakenPerTurn;

        //-1 for infinite duration 
        [SerializeField] private int _duration;
        public int Duration => _duration;

        [SerializeField] private bool _isStun;
        public bool IsStun => _isStun;

        [SerializeField] private bool hasStackableEffects;
        public bool HasStackableEffects => hasStackableEffects;
    
        [Tooltip("FOR PLAYER UNITS")]
        [SerializeField] private List<StatModifier> _statsModified;
        public List<StatModifier> StatsModified => _statsModified;
    
        [Tooltip("FOR ENEMIES (enemies do not have combat classes")]
        [SerializeField] private List<StatMultiplier> _GenericStatsModified;
        public  List<StatMultiplier> GenericStatsModified => _GenericStatsModified;

        public List<StatMultiplier> GetStatMultiplier(CombatClassType ctat) 
        {
            foreach (var c in _statsModified)
            {
                if (c.classType == ctat)
                    return c.statsModified;
            }

            return null;
        }
        
        //overclock -> burnout + stunned 
        [Tooltip("For statuses that cause other statuses. turnToAdd is used by Status class (irl example: cold -> frostbite")] [SerializeField]
        private List<StatusPoint> _statusTimeline;
        public List<StatusPoint> StatusTimeline => _statusTimeline;






    }
}