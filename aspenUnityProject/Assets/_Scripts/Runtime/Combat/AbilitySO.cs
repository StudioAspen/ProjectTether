using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace _Scripts.Runtime.Combat
{
   [CreateAssetMenu(fileName = "Ability" , menuName = "Scriptable Objects/Misc/Ability")]
   public class AbilitySO : ScriptableObject
   {
      //e.g., fireball_0 or obliterate_0
      [SerializeField] private string id;
      public string Id => id;
      
      //name displayed in UI 
      [SerializeField] private string abilityName;
      public string Name => name;

      [SerializeField] private bool _isModifiedAbility;
      public bool IsModifiedAbility => _isModifiedAbility;

      [Tooltip("If Applicable")]
      [SerializeField] private AbilitySO _modifiedFrom;
      public bool ModifiedFrom => _modifiedFrom;

      [SerializeField][TextArea(3,10)] private string _description;
      public string Description => _description;

      //make negative for healing moves 
      [SerializeField] private int _damage;
      public int Damage => _damage;

      [SerializeField] private int _numberOfHits;
      public int NumberOfHits => _numberOfHits;

      [SerializeField] private BattleParametersList.BattleParameters[] _effectsDueToAbility;
      public BattleParametersList.BattleParameters[] EffectsDueToAbility => _effectsDueToAbility;

      [Tooltip("If the number is between 0 and 1 it is the percent change of the " +
               "battle parameter by the ability. If it is over one, it is simply an addition")]
      [SerializeField] private float[] _effectAmount;
      public float[] EffectAmount => _effectAmount;

      [SerializeField] private int[] _numberOfTurnsEffectLasts;
      public int[] NumberOfTurnsEffectLasts => _numberOfTurnsEffectLasts;

      [SerializeField] private Element[] element; 
      public Element[] Element => element;

      [SerializeField] private float _chanceToHit;
      public float ChanceToHit => _chanceToHit;

      //This field is primarily for a move called breakneck rush.
      [SerializeField] private float _hitChanceLossPerTileTravelled;
      public float HitChanceLossPerTileTraveled => _hitChanceLossPerTileTravelled;

      [SerializeField] private float _macCost;
      public float MACCost => _macCost;

      [SerializeField] private float _energyCost;
      public float EnergyCost => _energyCost;
      [SerializeField] private AbilityType abilityType;
      public AbilityType AbilityType => abilityType;

      //ring-based range. 0 = same tile only 1 = one ring around self, etc. 
      [Range(0, 3)] [SerializeField] private int range;
      public int Range => range;
    
      //area of effect. 0 = only 1 tile affected. 1 = 1 entire ring of tiles surrounding the tile selected affected, etc. 3 is effectively global
      [Range(0,3)] [SerializeField] private int aoe;
      public int AOE => aoe;

      [SerializeField] private bool hitsEnemies; 
      public bool HitsEnemies => hitsEnemies;
   
      //needs to be true for healing moves 
      [SerializeField] private bool hitsAllies;
      public bool HitsAllies => hitsAllies;

      //needs to be true for healing moves 
      [SerializeField] private bool hitsSelf;
      public bool HitsSelf => hitsSelf;
      [SerializeField] private bool canTargetEmptyTile;
      public bool CanTargetEmptyTile => canTargetEmptyTile;
   
      [SerializeField] private bool canMiss;
      public bool CanMiss => canMiss;

        //not useful in MVP. This is for when we decide to have attacks affect tiles.
        //For example, a meteor attack could set several tiles aflame. Most attacks will have this be null. 
        /*
     [SerializeField] private TileSO tileEffect;
     public TileSO TileEffect => tileEffect;
     */

      // custom range. list of directions walked from the unit's tile
      // e.g. (0, 1), (0, 1), (0, 1) = line of 3 tiles north
      [SerializeField] private List<Vector2> customRange;
      public List<Vector2> CustomRange => customRange;


        public void SetName(string name) => abilityName = name;
    
    }
    
}
