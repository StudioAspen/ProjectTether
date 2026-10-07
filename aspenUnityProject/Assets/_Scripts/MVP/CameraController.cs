using UnityEngine;
using _Scripts.Runtime.Managers;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Update is called once per frame
    void Update()
    {
        Vector2 lookinput = InputManager.Instance.Actions.Player.Look.ReadValue<Vector2>();
    }
}
