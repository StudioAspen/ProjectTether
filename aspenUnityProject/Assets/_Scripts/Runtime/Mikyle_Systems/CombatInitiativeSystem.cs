using UnityEngine;
using _Scripts.Runtime.Entities.Scripts.Combat;

public class CombatInitiativeSystem : MonoBehaviour
{
    /*  TODO LIST:  Combat Initiative System 
     *   @@   Living Units (Player and Enemy) will have an Initiative value 
     *          that determines the order of their turns in combat.
     *   -   Current Turn: 
     *   -   Initative Order: 
     *   -   Next Turn calculation:
     *      
     *  This system will provide the next unit 
    */

    [Header("Combat Initiative System")]
    [Tooltip("I need a list of all units in combat to determine the order of turns.")]
    [SerializeField] private UnitController[] unitsInCombat;

    private int currentTurnIndex = 0;

    private void Start()
    {
        // Sort the units based on their initiative values (higher initiative goes first)
        System.Array.Sort(unitsInCombat, (a, b) => b.GetData().Speed.CompareTo(a.GetData().Speed));
    }

    private void Update()
    {
        /*  TODO: Handle turn progression   */
        // Log the current turn unit's name and initiative value
        if (unitsInCombat.Length > 0)
        {
            UnitController currentUnit = unitsInCombat[currentTurnIndex];
            Debug.Log($"Current Turn: {currentUnit.GetData().Name} (Initiative: {currentUnit.GetData().Speed})");
        }
    }
}
