using System;
using _Scripts.Consystently.Essentials;
using _Scripts.Runtime.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Managers
{
    public class PersistentMenuManager : Manager<PersistentMenuManager>
    {
        [field: SerializeField] public Menu PauseMenu { get; private set; }
        [field: SerializeField] public Menu SettingsMenu { get; private set; }
    }
}