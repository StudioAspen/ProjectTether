using System;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Persistent_Data;
using UnityEngine;

namespace _Scripts.Runtime.Overworld
{
    [RequireComponent(typeof(CharacterController))]
    public class OverworldPlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f; // Controls how fast the player moves:
        [SerializeField] private float _gravity = -9.81f;
    
        private float _verticalVelocity;
    
        private Transform _mainCamera;
        private CharacterController _controller;

        private Vector2 _moveInput;
    
        void Awake()
        {
            // Grabs the components and camera reference this script needs:
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main.transform;
        }

        private void Start()
        {
            transform.position = GameSession.PlayerData.OverworldPosition;
        }

        void Update()
        {
            ReadInput();
            ApplyMovement();
            
            // Update our session position
            GameSession.PlayerData.OverworldPosition = transform.position;
        }

        private void ReadInput()
        {
            // Player's movement input is read:
            _moveInput = InputManager.Instance.Actions.Player.Move.ReadValue<Vector2>();
        }
        
        private void ApplyMovement()
        {
            // Gets the camera's forward direction and keeps movement on the ground:
            Vector3 cameraForward = _mainCamera.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            // Gets the camera's right direction and keeps movement on the ground:
            Vector3 cameraDirection = _mainCamera.right;
            cameraDirection.y = 0;
            cameraDirection.Normalize();

            // Uses the camera's direction to decide where the player moves:
            Vector3 moveDirection = cameraDirection  * _moveInput.x + cameraForward * _moveInput.y;
            moveDirection = Vector3.ClampMagnitude(moveDirection,1);

            if(_controller.isGrounded && _verticalVelocity < 0)
                _verticalVelocity = -2;
            _verticalVelocity += _gravity * Time.deltaTime;

            // Moves the player using the CharacterController:
            Vector3 velocity = moveDirection * _speed;
            velocity.y = _verticalVelocity;

            _controller.Move(velocity * Time.deltaTime);
        }
    }
}
