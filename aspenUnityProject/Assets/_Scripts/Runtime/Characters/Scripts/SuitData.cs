using System;
using System.Collections.Generic;
using _Scripts.Runtime.Combat;

namespace _Scripts.Runtime.Characters.Scripts
{
    public class SuitData
    {
       public int EnergyCapacity { get; private set; }
       public int SkillsCapacity { get; private set; }
       public int AbilityCapacity { get; private set; }
       public bool IsOverclocked { get; private set; }
       
       
       //TODO:
       //NodeTree 
       //dictionary of equipped skills (wrapper skill class for skillSO), key = skill id
       
       
       //dictionary of equipped abilities, key = ability id 
       public Dictionary<string, Ability> EquippedAbilities { get; private set; }
       

       /// <summary>
       /// To be used during the initialization of the characters at the start of the game (new game)
       /// </summary>
       /// <param name="suitBaseData"></param>
       public SuitData(SuitBaseDataSO suitBaseData)
       {
           EnergyCapacity =  suitBaseData.EnergyCapacity;
           SkillsCapacity = suitBaseData.SkillCapacity;
           AbilityCapacity = suitBaseData.AbilityCapacity;
           //At the start, the equipped skills will only be the innate skill at the center of the node tree
           //TODO:
           //add abilities + skills to dictionaries here
       }

       public void EquipAbility(Ability ability)
       {
          EquippedAbilities.TryAdd(ability.Id, ability); 
          if(EquippedAbilities.Count > AbilityCapacity)
              IsOverclocked = true;
       }

       public void UnequipAbility(string abilityID)
       {
         EquippedAbilities.Remove(abilityID);
         if (EquippedAbilities.Count <= AbilityCapacity)
             IsOverclocked = false; 
       }

       public void SetAbilityCapacity(int capacity)
       {
          AbilityCapacity = capacity; 
       }

       public void AddAbilityCapacity(int amount)
       {
           AbilityCapacity += amount;
       }
       
       
           
           
       //TODO:
       //method for equipping skills
       //method for removing skills
       
    }
}
