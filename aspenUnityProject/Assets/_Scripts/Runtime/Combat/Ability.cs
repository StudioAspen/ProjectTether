using _Scripts.Runtime.Tile_System.Scripts;

namespace _Scripts.Runtime.Combat
{
    public class Ability
    {
       public string Id { get; private set; } 
       public string Name { get; private set; }
       public string Description { get; private set; }
       public int Damage { get; private set; }
       public Element[] Elements { get; private set; }
       public AbilityType AbilityType { get; private set; }
       public int AbilityRange { get; private set; }
       public int AOE { get; private set; } 
       public bool HitsEnemies { get; private set; }
       public bool HitsAllies  { get; private set; }
       public bool CanTargetEmptyTiles { get; private set; }
       public bool CanMiss { get; private set; }
       
//       public TileSO TileEffect {  get; private set; }

       public Ability(AbilitySO ability)
       {
           Id = ability.Id;
           Name = ability.Name;
           Description = ability.Description;
           Damage = ability.Damage;
           Elements = ability.Element;
           AbilityType = ability.AbilityType;
           AbilityRange = ability.Range;
           AOE = ability.AOE;
           HitsEnemies = ability.HitsEnemies;
           HitsAllies = ability.HitsAllies;
           CanTargetEmptyTiles = ability.CanTargetEmptyTile;
           CanMiss = ability.CanMiss;
           
           //TileEffect = ability.TileEffect
       }
       //upgrading ability -> probably change the ability to a diff. ability if more than just damage increase is desired 
    }
}