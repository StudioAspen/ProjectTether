using System;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Persistent_Data;
using UnityEngine;

namespace _Scripts.Runtime.Overworld
{
    [RequireComponent(typeof(CharacterController))]
    public class OverworldPlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [Range(1f, 10f)]
        [SerializeField] private float _characterMoveSpeed = 5f; // Controls how fast the Player's Character moves:
        [SerializeField] private float _characterRotationSpeed = 10f;

        [SerializeField] private float _gravity = -9.81f;
    
        private float _verticalVelocity;
    
        private Transform _mainCamera;
        private CharacterController _controller;

        private Vector2 _moveInput;

        private bool _isUsingCustomMovement;
    
        void Awake()
        {
            // Grabs the components and camera reference this script needs:
            _controller = GetComponent<CharacterController>();
            _mainCamera = Camera.main.transform;
            
            transform.position = GameSession.Instance.PlayerData.OverworldPosition;
        }

        void Update()
        {
            ReadInput();
            ApplyMovement();
        }

        private void OnDestroy()
        {
            GameSession.Instance.PlayerData.OverworldPosition = transform.position;
            GameSession.Instance.Save();
        }

        private void ReadInput()
        {
            // Player's movement input is read:
            _moveInput = InputManager.Instance.Actions.Player.Move.ReadValue<Vector2>();
        }
        
        private void ApplyMovement()
        {
            if (_isUsingCustomMovement)
                return;
            
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
            moveDirection = Vector3.ClampMagnitude(moveDirection, 1);

            if(_controller.isGrounded && _verticalVelocity < 0)
                _verticalVelocity = -2;
            _verticalVelocity += _gravity * Time.deltaTime;

            // Moves the player using the CharacterController:
            Vector3 velocity = moveDirection * _characterMoveSpeed;
            velocity.y = _verticalVelocity;

            MoveWithVelocity(velocity, true);
        }

        /// <summary>
        /// Call this in Update()
        /// </summary>
        public void MoveWithVelocity(Vector3 velocity, bool applyRotation = true)
        {
            _controller.Move(velocity * Time.deltaTime);
            
            if (!applyRotation)
                return;
            
            Vector3 planarVelocity = velocity;
            planarVelocity.y = 0;
            if (planarVelocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(planarVelocity, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _characterRotationSpeed * Time.deltaTime);
            }
        }
        
        public void OverrideWithCustomMovement(bool isUsingCustomMovement) => _isUsingCustomMovement = isUsingCustomMovement;
    }
}
