using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Misc/Ability")]
public class AbilitySO : ScriptableObject
{
   [SerializeField] private string abilityName;
   public string Name => name;
   
   [SerializeField] private string description;
   public string Description => description;

   //make negative for healing moves 
   [SerializeField] private int damage;
   public int Damage => damage;

   [SerializeField] private Element[] element; 
   public Element[] Element => element;
  
   [SerializeField] private AbilityType abilityType;
   public AbilityType AbilityType => abilityType;

   //ring-based range. 0=same tile only 1 = one ring around self, etc. 
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


   [SerializeField] private bool canTargetEmptyTile;
   public bool CanTargetEmptyTile => canTargetEmptyTile;

   //not useful in MVP. This is for when we decide to have attacks affect tiles.
   //For example, a meteor attack could set several tiles aflame. Most attacks will have this be null. 
   /*
   [SerializeField] private TileSO tileEffect;
   public TileSO TileEffect => tileEffect;
   */

   //we can add specific unique ranges later by having an array of cube coordinate directions


}
