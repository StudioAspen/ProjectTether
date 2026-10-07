using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    private InputSystem_Actions actions;
    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        actions = new InputSystem_Actions();
        Debug.Log(actions.asset.name);
    }

    void OnEnable()
    {
        actions.Player.Enable();
    }

    void OnDisable()
    {
        actions.Player.Disable();    
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = actions.Player.Move.ReadValue<Vector2>();
        Debug.Log(moveInput);
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        controller.Move(moveDirection * speed * Time.deltaTime);
    }
}
