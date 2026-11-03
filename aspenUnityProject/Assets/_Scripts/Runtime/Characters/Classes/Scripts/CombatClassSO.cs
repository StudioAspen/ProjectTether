using _Scripts.Runtime.Combat;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Runtime.Characters.Classes.Scripts
{
    [CreateAssetMenu(fileName = "CombatClass", menuName = "Scriptable Objects/Misc/Combat Class")]
    public class CombatClassSO : ScriptableObject
    {
    
        //TODO: add stat modifier later 
        [SerializeField] private Element[] defaultAttackElements;
        public Element[] DefaultAttackElements => defaultAttackElements;

        [FormerlySerializedAs("className")] [SerializeField] private string _className;
        public string ClassName => _className;

        //0 = cannot attack anything, 1 = 1 ring around unit, 3 = effectively global range for 19 tiles
        [FormerlySerializedAs("defaultAttackRange")] [SerializeField, Range(0,3)]
        private int _defaultAttackRange;

        public int DefaultAttackRange => _defaultAttackRange;
        
        [SerializeField]
        protected Skill _innateSkill;
        public Skill InnateSkill => _innateSkill;

        [SerializeField]
        protected List<Skill> _classSkills;
        public List<Skill> classSkills => _classSkills;

        [SerializeField]
        protected List<AbilitySO> _classAbilityList;
        public List<AbilitySO> ClassAbilityList => _classAbilityList;

        [FormerlySerializedAs("classDescription")] [SerializeField] [TextArea(3 , 10)] private string _classDescription;
        public string ClassDescription => _classDescription;


        [Header("Overclock", order = 4)]
        [Space(5)]
        [Tooltip("What Overclock is effecting (That is not stats): 1 = Attack Chance , 2 = Attack Repitition Frequency")]
        [SerializeField]
        protected List<int> _whatOverclockEffects;
        public List<int> WhatOverclockEffects => WhatOverclockEffects;

        [Space(5)]
        [Tooltip("Overclock Effects Percent")]
        [SerializeField]
        protected List<float> _overclockEffectsOverwrites;
        public List<float> OverclockEffectsOverwrites => _overclockEffectsOverwrites;

        [Tooltip("Overclock Explanations")]
        [SerializeField][TextArea(0, 10)] private string[] explanations;

        [Tooltip("stats raised by overclock. 0=hp, 1=en, 2=str, 3=def, 4=tec, 5=res, 6=spe, 7=luck, 8=prc, 9=eva")]
        [SerializeField] private int[] _overclockedStats;
        public int[] OverclockedStats => _overclockedStats;

        [Tooltip("Percent that the stat is raised")]
        [SerializeField] private float[] _overclockedStatsPercent;
        public float[] OverclockedStatsPercent => _overclockedStatsPercent;

        // RBS-Overclock
        [SerializeField] private float[] _overclockedModifier;
        public float[] OverclockedModifier => _overclockedModifier;

        [Header("Class Stats", order = 5)]
        [Space(10)]
        [Tooltip("HP: Total amount of damage a unit can sustain before falling in battle.")]
        [SerializeField]
        private int _healthStat;

        /// <summary>
        /// HP: Total amount of damage a unit can sustain before falling in battle.
        /// </summary>
        public int HealthStat => _healthStat;


        [Space(5)]
        [Tooltip("EN: Total amount of resources a unit can spend to use special abilities.")]
        [SerializeField]
        protected int _energyStat;

        /// <summary>
        /// EN: Total amount of resources a unit can spend to use special abilities.
        /// </summary>
        public int EnergyStat => _energyStat;


        [Space(5)]
        [Tooltip("STR: A unit's physical power.")]
        [SerializeField]
        protected int _strengthStat;

        /// <summary>
        /// STR: A unit's physical power.
        /// </summary>
        public int StrengthStat => _strengthStat;


        [Space(5)]
        [Tooltip("DEF: A unit's durability against physical power.")]
        [SerializeField]
        protected int _defenseStat;

        /// <summary>
        /// DEF: A unit's durability against physical power.
        /// </summary>
        public int DefenseStat => _defenseStat;


        [Space(5)]
        [Tooltip("TEC: A unit's technical power.")]
        [SerializeField]
        protected int _techStat;

        /// <summary>
        /// TEC: A unit's technical power.
        /// </summary>
        public int TechStat => _techStat;


        [Space(5)]
        [Tooltip("RES: A unit's durability against technical power.")]
        [SerializeField]
        protected int _resistanceStat;

        /// <summary>
        /// RES: A unit's durability against technical power.
        /// </summary>
        public int ResistanceStat => _resistanceStat;


        [Space(5)]
        [Tooltip("SPE: How often a unit moves during battle.")]
        [SerializeField]
        protected int _speedStat;

        /// <summary>
        /// SPE: How often a unit moves during battle.
        /// </summary>
        public int SpeedStat => _speedStat;


        [Space(5)]
        [Tooltip("LCK: A unit's affinity for chance.")]
        [SerializeField]
        protected int _luckStat;

        /// <summary>
        /// LCK: A unit's affinity for chance.
        /// </summary>
        public int LuckStat => _luckStat;


        [Space(5)]
        [Tooltip("PRC: A unit's accuracy for targeting attacks & abilities.")]
        [SerializeField]
        protected int _precisionStat;

        /// <summary>
        /// PRC: A unit's accuracy for targeting attacks & abilities.
        /// </summary>
        public int PrecisionStat => _precisionStat;


        [Space(5)]
        [Tooltip("EVA: A unit's evasiveness to incoming attacks & abilities.")]
        [SerializeField]
        protected int _evasionStat;

        /// <summary>
        /// EVA: A unit's evasiveness to incoming attacks & abilities.
        /// </summary>
        public int EvasionStat => _evasionStat;

        public Dictionary<Stat , int> ClassStats => new Dictionary<Stat , int>()
        {
              { Stat.HP, _healthStat },
              //TODO: remove energy 
              { Stat.EN, _energyStat },
              { Stat.STR, _strengthStat },
              { Stat.DEF, _defenseStat },
              { Stat.TEC, _techStat },
              { Stat.RES, _resistanceStat },
              { Stat.SPE, _speedStat },
              { Stat.LCK, _luckStat },
              { Stat.PRC, _precisionStat },
              { Stat.EVA, _evasionStat },
        };

        [Header("Class Aptitudes", order = 5)]
        [Space(10)]
        [Tooltip("HP: Total amount of damage a unit can sustain before falling in battle.")]
        [SerializeField]
        private Tier _health;

        /// <summary>
        /// HP: Total amount of damage a unit can sustain before falling in battle.
        /// </summary>
        public Tier Health => _health;


        [Space(5)]
        [Tooltip("EN: Total amount of resources a unit can spend to use special abilities.")]
        [SerializeField]
        protected Tier _energy;

        /// <summary>
        /// EN: Total amount of resources a unit can spend to use special abilities.
        /// </summary>
        public Tier Energy => _energy;


        [Space(5)]
        [Tooltip("STR: A unit's physical power.")]
        [SerializeField]
        protected Tier _strength;

        /// <summary>
        /// STR: A unit's physical power.
        /// </summary>
        public Tier Strength => _strength;


        [Space(5)]
        [Tooltip("DEF: A unit's durability against physical power.")]
        [SerializeField]
        protected Tier _defense;

        /// <summary>
        /// DEF: A unit's durability against physical power.
        /// </summary>
        public Tier Defense => _defense;


        [Space(5)]
        [Tooltip("TEC: A unit's technical power.")]
        [SerializeField]
        protected Tier _tech;

        /// <summary>
        /// TEC: A unit's technical power.
        /// </summary>
        public Tier Tech => _tech;


        [Space(5)]
        [Tooltip("RES: A unit's durability against technical power.")]
        [SerializeField]
        protected Tier _resistance;

        /// <summary>
        /// RES: A unit's durability against technical power.
        /// </summary>
        public Tier Resistance => _resistance;


        [Space(5)]
        [Tooltip("SPE: How often a unit moves during battle.")]
        [SerializeField]
        protected Tier _speed;

        /// <summary>
        /// SPE: How often a unit moves during battle.
        /// </summary>
        public Tier Speed => _speed;


        [Space(5)]
        [Tooltip("LCK: A unit's affinity for chance.")]
        [SerializeField]
        protected Tier _luck;

        /// <summary>
        /// LCK: A unit's affinity for chance.
        /// </summary>
        public Tier Luck => _luck;


        [Space(5)]
        [Tooltip("PRC: A unit's accuracy for targeting attacks & abilities.")]
        [SerializeField]
        protected Tier _precision;

        /// <summary>
        /// PRC: A unit's accuracy for targeting attacks & abilities.
        /// </summary>
        public Tier Precision => _precision;


        [Space(5)]
        [Tooltip("EVA: A unit's evasiveness to incoming attacks & abilities.")]
        [SerializeField]
        protected Tier _evasion;

        /// <summary>
        /// EVA: A unit's evasiveness to incoming attacks & abilities.
        /// </summary>
        public Tier Evasion => _evasion;

        public Dictionary<Stat, Tier> Aptitudes => new Dictionary<Stat, Tier>()
        {
              { Stat.HP, _health },
              //TODO: remove energy 
              { Stat.EN, _energy },
              { Stat.STR, _strength },
              { Stat.DEF, _defense },
              { Stat.TEC, _tech },
              { Stat.RES, _resistance },
              { Stat.SPE, _speed },
              { Stat.LCK, _luck },
              { Stat.PRC, _precision },
              { Stat.EVA, _evasion },
        };

        [Header("Class Affinities", order = 6)]
        [Space(10)]
        [Tooltip("This unit's affinity to Blunt damage.")]
        [SerializeField]
        protected Affinity _blunt;

        /// <summary>
        /// This unit's affinity to Blunt damage.
        /// </summary>
        public Affinity Blunt => _blunt;

        [Space(5)]
        [Tooltip("This unit's affinity to Slash damage.")]
        [SerializeField]
        protected Affinity _slash;

        /// <summary>
        /// This unit's affinity to Slash damage.
        /// </summary>
        public Affinity Slash => _slash;


        [Space(5)]
        [Tooltip("This unit's affinity to Pierce damage.")]
        [SerializeField]
        protected Affinity _pierce;

        /// <summary>
        /// This unit's affinity to Pierce damage.
        /// </summary>
        public Affinity Pierce => _pierce;


        [Space(5)]
        [Tooltip("This unit's affinity to Blast damage.")]
        [SerializeField]
        protected Affinity _blast;

        /// <summary>
        /// This unit's affinity to Blast damage.
        /// </summary>
        public Affinity Blast => _blast;


        [Space(5)]
        [Tooltip("This unit's affinity to Water damage.")]
        [SerializeField]
        protected Affinity _water;

        /// <summary>
        /// This unit's affinity to Water damage.
        /// </summary>
        public Affinity Water => _water;


        [Space(5)]
        [Tooltip("This unit's affinity to Earth damage.")]
        [SerializeField]
        protected Affinity _earth;

        /// <summary>
        /// This unit's affinity to Earth damage.
        /// </summary>
        public Affinity Earth => _earth;


        [Space(5)]
        [Tooltip("This unit's affinity to Wind damage.")]
        [SerializeField]
        protected Affinity _wind;

        /// <summary>
        /// This unit's affinity to Wind damage.
        /// </summary>
        public Affinity Wind => _wind;


        [Space(5)]
        [Tooltip("This unit's affinity to Fire damage.")]
        [SerializeField]
        protected Affinity _fire;

        /// <summary>
        /// This unit's affinity to Fire damage.
        /// </summary>
        public Affinity Fire => _fire;

        public Dictionary<Element, Affinity> Affinities => new Dictionary<Element, Affinity>()
        {
          { Element.Blunt, _blunt },
          { Element.Slash, _slash },
          { Element.Pierce, _pierce },
          { Element.Blast, _blast },
          { Element.Water, _water },
          { Element.Earth, _earth },
          { Element.Wind, _wind },
          { Element.Fire, _fire },
        };
    }
}
