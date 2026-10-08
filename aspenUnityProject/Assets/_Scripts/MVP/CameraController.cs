using UnityEngine;
using _Scripts.Runtime.Managers;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    public float lookSensitivity = 2;


    // Update is called once per frame
    void Update()
    {
        Vector2 lookinput = InputManager.Instance.Actions.Player.Look.ReadValue<Vector2>();
        Debug.Log(InputManager.Instance.Actions.Player.enabled);
        Debug.Log(lookinput);
        float rotationAmount = lookinput.x * lookSensitivity;
        transform.Rotate(0, rotationAmount, 0);

    }

    void OnEnable()
    {
        InputManager.Instance.Actions.Player.Enable();
    }

    void OnDisable()
    {
        InputManager.Instance.Actions.Player.Disable();
    }
}
