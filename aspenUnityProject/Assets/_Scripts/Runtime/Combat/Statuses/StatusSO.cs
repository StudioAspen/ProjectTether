using System.Collections.Generic;
using _Scripts.Runtime.Characters.Classes.Scripts;
using _Scripts.Runtime.Combat.Statuses.StatusEffects;
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
        public StatusSO statusToAdd;
    }

    [System.Serializable]
    public struct ElementAffinityChange
    {
        public Element elementToChange;
        public Affinity changeInAffinity;
    }

    //concrete class for default statuses. Do not delete
    [CreateAssetMenu(fileName = "Status", menuName = "Scriptable Objects/Statuses/Status")]
    public class StatusSO : ScriptableObject
    {

        [SerializeField] private string _statusId;
        public string StatusId => _statusId;

        [SerializeField] private string _statusName;
        public string StatusName => _statusName;

        [SerializeField] [TextArea(2, 4)] private string _statusDescription;
        public string StatusDescription => _statusDescription;

        [Tooltip("1 for no extra damage taken")] [SerializeField]
        private float _damageTakenMultiplier;

        public float DamageTakenMultiplier => _damageTakenMultiplier;

        [SerializeField] private int _damageTakenPerTurn;
        public int DamageTakenPerTurn => _damageTakenPerTurn;

        [Tooltip("burning status would do fire damage for example")] [SerializeField]
        private Element[] _damageTypes;

        public Element[] DamageTypes => _damageTypes;

        [Tooltip("-1 for infinite duration")] [SerializeField]
        private int _duration;

        public int Duration => _duration;

        [SerializeField] private bool _isStun;
        public bool IsStun => _isStun;

        [SerializeField] private bool hasStackableEffects;
        public bool HasStackableEffects => hasStackableEffects;

        [Tooltip("FOR PLAYER UNITS")] [SerializeField]
        private List<StatModifier> _statsModified;

        public List<StatModifier> StatsModified => _statsModified;

        [Tooltip("FOR ENEMIES (enemies do not have combat classes")] [SerializeField]
        private List<StatMultiplier> _GenericStatsModified;

        public List<StatMultiplier> GenericStatsModified => _GenericStatsModified;



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
        [Tooltip(
            "For statuses that cause other statuses. turnToAdd is used by Status class (irl example: cold -> frostbite")]
        [SerializeField]
        private List<StatusPoint> _statusTimeline;

        public List<StatusPoint> StatusTimeline => _statusTimeline;

        [SerializeField] private List<StatusEffectSO> _statusEffectsOnAdd = new();
        public List<StatusEffectSO> StatusEffectsOnAdd => _statusEffectsOnAdd;

        [SerializeField] private List<StatusEffectSO> _statusEffectsOnTick = new();
        public List<StatusEffectSO> StatusEffectsOnTick => _statusEffectsOnTick;

        [SerializeField] private List<StatusEffectSO> _statusEffectsOnRemove = new();
        public List<StatusEffectSO> StatusEffectsOnRemove => _statusEffectsOnRemove;


        /// <summary>
        /// called on add. Needs status effects slotted in the corresponding list to do anything
        /// </summary>
        /// <param name="statusContext"></param>
        public virtual void OnAdd(StatusContext statusContext)
        {
            if (_statusEffectsOnAdd == null)
                return;
            foreach (var s in _statusEffectsOnAdd)
            {
                s.Execute(statusContext);
            }

        }

        /// <summary>
        /// called per turn 
        /// </summary>
        /// <param name="statusContext"></param>
        public virtual void Tick(StatusContext statusContext)
        {
            if (_statusEffectsOnTick == null)
                return;
            foreach (var s in _statusEffectsOnTick)
            {
                s.Execute(statusContext);
            }
        }

        /// <summary>
        /// called only on removal (either cleansed(?) or run out of duration) 
        /// </summary>
        /// <param name="statusContext"></param>
        public virtual void OnRemove(StatusContext statusContext)
        {
            if (_statusEffectsOnRemove== null)
                return;
            foreach (var s in _statusEffectsOnTick)
            {
                s.Execute(statusContext);
            }
        }
    }
}