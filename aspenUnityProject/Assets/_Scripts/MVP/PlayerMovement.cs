using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5; //  Controls how fast the player moves:

    Transform mainCamera;
    private InputSystem_Actions actions;
    CharacterController controller;

    void Awake()
    {
        // Grabs the compoments and camera reference this sript needs:
        controller = GetComponent<CharacterController>();
        actions = new InputSystem_Actions();
        mainCamera = Camera.main.transform;
       
    }

    void OnEnable()
    {
        // Turns on the player's input actions:
        actions.Player.Enable();
    }

    void Update()
    {
        // Player's movement input is read:
        Vector2 moveInput = actions.Player.Move.ReadValue<Vector2>();
        
        // Gets the camera's forward direction and keeps movement on the ground:
        Vector3 cameraForward = mainCamera.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        // Gets the camera's right direction and keeps movement on the ground:
        Vector3 cameraDirection = mainCamera.right;
        cameraDirection.y = 0;
        cameraDirection.Normalize();

        // Uses the camera's direction to decide where the player moves:
        Vector3 moveDirection = cameraDirection  * moveInput.x + cameraForward * moveInput.y;

        // Moves the player using the CharacterController:
        controller.Move(moveDirection * speed * Time.deltaTime);
        
    }
    
}
