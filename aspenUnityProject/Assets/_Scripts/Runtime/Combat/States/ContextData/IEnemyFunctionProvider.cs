using UnityEngine;

namespace _Scripts.Runtime.Combat.States.ContextData
{
    public interface IEnemyFunctionProvider
    {
        public void HandleMove(Vector3Int from, Vector3Int to, int range);
        public bool CanMove(Vector3Int from, Vector3Int to, int range);
        public bool Attack();
        
        public void PopState(); 
    }
}