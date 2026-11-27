using _Scripts.Runtime.Entities.Scripts.Combat;

namespace _Scripts.Runtime.Combat
{
    public class DeathContext
    {
        public DeathCause DeathCause { get; set; }
        public UnitController Murderer { get; set; } = null;
    }
}