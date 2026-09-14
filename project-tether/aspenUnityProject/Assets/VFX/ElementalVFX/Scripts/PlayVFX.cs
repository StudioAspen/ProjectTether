using UnityEngine;
using UnityEngine.VFX; // <---- Use this library.
using UnityEngine.InputSystem;


public class PlayVFX : MonoBehaviour
{
    public VisualEffect vfx; // <---- Create a VisualEffect object. 
    public InputActionReference trigger;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnEnable()
    {
        trigger.action.started += Space;
    }
    
    void OnDisable()
    {
        trigger.action.started -= Space; // To avoid and action like 'Space' being squiggled, create a method of the same name down below.
    }

    
    private void Space(InputAction.CallbackContext context) // A method of the Action you created is needed down here. 
    {
        Debug.Log("Space");
        vfx.Play(); // <---- Call the .Play function from the VisualEffect class. 
        
    }
}
