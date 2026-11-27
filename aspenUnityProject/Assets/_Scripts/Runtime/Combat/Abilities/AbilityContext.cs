using System.Collections.Generic;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;

namespace _Scripts.Runtime.Combat.Abilities
{
    /// <summary>
    /// the means by which combat data is manipulated by additional ability effects 
    /// </summary>
    public class AbilityContext
    {
        public UnitController Caster { get; set; }
        public TileController Target { get; set; }
        public AbilitySO AbilityData { get; set; }
        
        public BattleUnitsList AllUnitInfo { get; set; }
        public List<TileController> AdditionalTargets {get; set; }
        
        //add as needed
    }
}