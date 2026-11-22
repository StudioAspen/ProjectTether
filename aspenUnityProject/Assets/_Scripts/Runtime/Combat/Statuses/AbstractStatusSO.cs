using System.Collections.Generic;
using _Scripts.Runtime.Characters.Classes.Scripts;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Statuses
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
        public float adder; //For when it is not multiplied
        public Stat stat;
    }
    
    //no clue what to call this 
    [System.Serializable]
    public struct StatusPoint
    {
        public int turnToAdd;
        public AbstractStatusSO statusToAdd;
    }

    [System.Serializable]
    public struct ElementAffinityChange
    {
        public Element elementToChange;
        public Affinity changeInAffinity;
    }
    
    public abstract class AbstractStatusSO : ScriptableObject
    {
        [SerializeField] private string _statusId; 
        public string StatusId => _statusId;
    
        [SerializeField] private string _statusName;
        public string StatusName => _statusName;

        [SerializeField] [TextArea(2,4)]private string _statusDescription;
        public string StatusDescription => _statusDescription;

        [Tooltip("1 for no extra damage taken")]
        [SerializeField] private float _damageTakenMultiplier;
        public float DamageTakenMultiplier => _damageTakenMultiplier;
    
        [SerializeField] private int _damageTakenPerTurn;
        public int  DamageTakenPerTurn => _damageTakenPerTurn;

        [Tooltip("burning status would do fire damage for example")]
        [SerializeField] private Element[] _damageTypes;
        public Element[] DamageTypes => _damageTypes;

        [Tooltip("-1 for infinite duration")]
        [SerializeField] private int _duration;
        public int Duration => _duration;

        [Tooltip("for statuses like insulated")] 
        [SerializeField] private bool _durationLoweredOnDamage;
        public bool DurationLoweredOnDamage => _durationLoweredOnDamage;

        [Tooltip("for stuff like streetwise gambit. Triggers when killing something else")]
        [SerializeField] private bool _triggerOnKill;
        public bool TriggerOnKill => _triggerOnKill;

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


        public virtual void OnAdd(Unit unit)
        {
            if (unit.Faction == Faction.Ally)
            {
                foreach (var sm in GetStatMultiplier(((AllyUnit)unit).CurrentSuit.GetClassType()))
                {
                    unit.MultiplyEStat(sm.stat, sm.multiplier);
                }
            }
            else
            {
                foreach(var sm in GenericStatsModified)
                    unit.MultiplyEStat(sm.stat, sm.multiplier);
            }
        }


        /// <summary>
        /// proc status effect 
        /// </summary>
        public virtual void Tick(Unit unit)
        {
            CombatFormulas.StatusDamage(_damageTakenPerTurn, _damageTypes, unit);
        }

        public virtual void OnRemove(Unit unit)
        {
            if (unit.Faction == Faction.Ally)
            {
                foreach (var sm in GetStatMultiplier(((AllyUnit)unit).CurrentSuit.GetClassType()))
                {
                    unit.RevertEStat(sm.stat, sm.multiplier);
                }
            }
            else
            {
                foreach(var sm in GenericStatsModified)
                    unit.RevertEStat(sm.stat, sm.multiplier);
            }
        }

        /// <summary>
        /// for effects that are not per turn and require conditions like trigger on kill
        /// </summary>
        public virtual void Trigger(Unit unit)
        {
           //stat buffs here. Change unit effective stat  
        }



    }
}