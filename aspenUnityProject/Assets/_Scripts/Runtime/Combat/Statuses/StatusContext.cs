using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;

namespace _Scripts.Runtime.Combat.Statuses
{
    public class StatusContext
    {
        /// <summary>
        /// for statuses that have stacks 
        /// </summary>
        public int StatusStack { get; set; } = 0;

        public int TurnsPassed { get; set; } = 0;
       
       public int Duration { get; set; }
       
       /// <summary>
       /// unit that applied this status
       /// </summary>
       public UnitController Applier { get; set; }
       
       public UnitController Applied { get; set; }
       
       /// <summary>
       /// tile containing the units on which to apply statuses 
       /// </summary>
       public TileController Target { get; set; }

       /// <summary>
       /// may be useful if the status does crazy shit like swapping unit positions 
       /// </summary>
       public BattleUnitsList AllUnitInfo { get; set; } 
       
       public StatusSO StatusSOData { get; set; }
    }
}