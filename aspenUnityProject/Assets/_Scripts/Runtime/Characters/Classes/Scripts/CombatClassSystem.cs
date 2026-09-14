using UnityEngine;
using System.Collections.Generic;

public class CombatClassSystem : MonoBehaviour, IInitializer
{
    CombatClass _selectedClass;
    List<CombatClassType> _availableClasses = new();

    UnitDataSO _stats;

    public CombatClass SelectedClass => _selectedClass;

    public void Intialize(UnitDataSO stats)
    {
        _stats = stats;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _selectedClass = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool IsCombatClassAvailable(CombatClassType selectedCombatClass) => _availableClasses.Contains(selectedCombatClass);
    bool IsCombatClassEquipped(CombatClassType selectedCombatClass) => _selectedClass.ClassType == selectedCombatClass;

    public void SwitchClass(CombatClassType selectedClass)
    {
        if (_availableClasses.Count != 0)
        {
            if (IsCombatClassAvailable(selectedClass) && !IsCombatClassEquipped(selectedClass))
            {
                Debug.Log("Found Class Successful");
                if (_selectedClass != null) _selectedClass.RemoveClassStatBuff(_stats);
                _selectedClass = CombatClassManager.Instance.CombatClassDict[selectedClass];
                _selectedClass.AddClassStatBuff(_stats);
            }
            else
            {
                Debug.Log("Found Class Unsuccessful");
            }
        }
    }

    public void AddClass(CombatClassType selectedClass) => _availableClasses.Add(selectedClass);
}