using UnityEngine;

namespace _Scripts.Runtime.Overworld.Tools
{
    public abstract class Tool : MonoBehaviour
    {
        public ToolUser User { get; private set; }

        public void Activate(ToolUser user)
        {
            User = user;
            OnActivate(user);
        }
        
        private protected abstract void OnActivate(ToolUser user);
    }
}