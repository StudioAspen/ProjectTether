using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class PowerSource : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnActivated { get; private set; } = new();
    [field: SerializeField, ReadOnly] public bool IsActive { get; private set; }
    
    public bool TryActivate()
    {
        if (IsActive)
            return false;
        
        OnActivated?.Invoke();
        return true;
    }
}