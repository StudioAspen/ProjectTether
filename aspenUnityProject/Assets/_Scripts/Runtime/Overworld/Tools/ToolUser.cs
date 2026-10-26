using System;
using _Scripts.Runtime.Managers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Runtime.Overworld.Tools
{
    public class ToolUser : MonoBehaviour
    {
        [field: SerializeField, ReadOnly, Required, InfoBox("Autoreferenced from children")] public Tool Tool { get; private set; }

        private void OnValidate()
        {
            Tool = GetComponentInChildren<Tool>();
        }

        private void Awake()
        {
            Tool = GetComponentInChildren<Tool>();
        }

        private void OnEnable()
        {
            InputManager.Instance.Actions.Player.UseTool.performed += Input_UseTool;
        }

        private void OnDisable()
        {
            InputManager.Instance.Actions.Player.UseTool.performed -= Input_UseTool;
        }

        private void Input_UseTool(InputAction.CallbackContext context)
        {
            ActivateTool();
        }

        public void ActivateTool()
        {
            if (Tool == null)
                return;
            
            Tool.Activate(this);
        }
    }
}