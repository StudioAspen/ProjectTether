using _Scripts.Runtime.Managers;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Consystently.Essentials;
using UnityEngine;

public class CombatClassManager : Singleton<CombatClassManager>
{
   public Dictionary<CombatClassType, CombatClass> CombatClassDict {  get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CombatClassDict = new Dictionary<CombatClassType, CombatClass>();

        CombatClassDict.Add(CombatClassType.Breacher, new Breacher());
    }
}
