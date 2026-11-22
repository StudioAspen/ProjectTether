using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Statuses
{
    [CreateAssetMenu(fileName = "CCStatus", menuName = "Scriptable Objects/Statuses/CCStatus")]
    
    //statuses that are stuns, slows, etc.
    public class CCStatusSO : AbstractStatusSO
    {
        [SerializeField] private bool _isStun;
        public bool IsStun => _isStun;
        
        //other crowd control effects here
        public override void OnAdd(Unit unit)
        {
            //use one stunned bool in unit (other forms of cc determine whether they are stunned depending on unit's immunities)
            base.OnAdd(unit);
            if(_isStun)
                unit.SetStunned(true);
        }
        
        
        public override void Tick(Unit unit)
        {
            if (_isStun)
                unit.SetStunned(true);
            base.Tick(unit);
       }

        public override void OnRemove(Unit unit)
        {
            unit.SetStunned(false); 
        }
    }
}