namespace _Scripts.Runtime.Combat
{
    public enum CombatActions
    {
       Attack,
       Defend,
       Move,
       Ability,
       Item,
       View, //possibly redundant(?) because it was moved to an input that bypasses the switch 
       Overclock
    }
}