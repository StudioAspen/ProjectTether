using System;
using _Scripts.Runtime.Managers;
using UnityEngine;

namespace _Scripts.Runtime.UI
{
    [RequireComponent(typeof(InputManager))]
    public class UIInputBridge : MonoBehaviour
    {
        private InputManager _inputManager;

        private void Awake()
        {
            _inputManager = GetComponent<InputManager>();
        }

        private void Start()
        {
            _inputManager.OnNewControllerDetected += InputManager_OnNewControllerDetected;
        }

        private void OnDestroy()
        {
            _inputManager.OnNewControllerDetected -= InputManager_OnNewControllerDetected;
        }
    
        private void InputManager_OnNewControllerDetected(InputManager.Controller newController)
        {
            Menu.SetCurrentSelectedObject();
        }
    }
}