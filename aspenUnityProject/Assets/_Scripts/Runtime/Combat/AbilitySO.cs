using UnityEngine;

namespace _Scripts.Runtime.Combat
{
   [CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Misc/Ability")]
   public class AbilitySO : ScriptableObject
   {
      //e.g., fireball_0 or obliterate_0
      [SerializeField] private string id;
      public string Id => id;
      
      //name displayed in UI 
      [SerializeField] private string abilityName;
      public string Name => name;
      
      [SerializeField][TextArea(3,10)] private string description;
      public string Description => description;

      //make negative for healing moves 
      [SerializeField] private int damage;
      public int Damage => damage;

      [Tooltip("Effects due to ability. 0 = Dodge , 1 = Accuracy , 2 = Defense , 3 = Critical Damage , 4 = Strength")]
      [SerializeField] private int[] _effectsDueToAbility;
      public int[] EffectsDueToAbility => _effectsDueToAbility;

      [Tooltip("Percent change of the effect by the ability.")]
      [SerializeField] private float[] _percentOfEffect;
      public float[] PercentofEffect => _percentOfEffect;

      [SerializeField] private Element[] element; 
      public Element[] Element => element;

      [SerializeField] private float _chanceToHit;
      public float ChanceToHit => _chanceToHit;

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

        //we can add specific unique ranges later by having an array of cube coordinate directions

        public void SetName(string name) => abilityName = name;
    }
}
