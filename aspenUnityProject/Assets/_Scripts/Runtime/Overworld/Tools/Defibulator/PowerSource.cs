using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class PowerSource : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<bool> OnActivateChanged { get; private set; } = new();
    [field: SerializeField] public bool IsActive { get; private set; }

    private void Start()
    {
        OnActivateChanged?.Invoke(IsActive);
    }

    public bool TryActivate()
    {
        if (IsActive)
            return false;

        ForceActivate();
        return true;
    }

    [Button("Activate", EButtonEnableMode.Playmode)]
    public void ForceActivate()
    {
        IsActive = true;
        OnActivateChanged?.Invoke(IsActive);
    }

    [Button("Deactivate", EButtonEnableMode.Playmode)]
    public void Deactivate()
    {
        IsActive = false;
        OnActivateChanged?.Invoke(IsActive);
    }
}