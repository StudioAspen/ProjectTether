using _Scripts.Runtime.Managers;
using UnityEngine;

namespace _Scripts.Runtime.UI
{
    public class CursorController : MonoBehaviour
    {
        void Awake()
        {
            CombatManager.hoverTileChanged += Move;
        }

        void OnDestroy()
        {
            CombatManager.hoverTileChanged -= Move;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void Move(Vector3 newPosition)
        {
            gameObject.transform.position = newPosition; 
        }
    
    }
}
