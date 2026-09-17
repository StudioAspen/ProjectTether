using UnityEngine;
using System.Collections.Generic;

//Entity but pure c# class. Abstract class -> split into ally and enemy 
//Missing Energy/Health system
public abstract class Unit {
//TODO: add buff feature (MAKE scriptable objects for buffs/debuffs) then store array in unit

    public Sprite Portrait { get; private set; }
    public string Name { get; set; }
    public Faction Faction { get; protected set; }
    public int Level { get; protected set; }
    public int XpToNextLevel { get; protected set; }
    public int TotalXp { get; protected set; }
    
    public Dictionary<Stat, Tier> StatGrowths { get; protected set; }
    public Dictionary<Element, Affinity> Affinities { get; protected set; }
    
    //statMs
    public int Health { get; protected set; }
    public int Energy { get; protected set; }
    public int Strength { get; protected set; }
    public int Defense { get; protected set; }
    public int Tech { get; protected set; } 
    public int Resistance { get; protected set; }
    public int Speed { get; protected set; } 
    public int Luck {get; protected set;}
    public int Precision {get; protected set;}
    public int Evasion {get; protected set;}

    public int HealthRemaining { get; protected set; } 
    public int EnergyRemaining { get; protected set; }
    
    public bool IsBlocking { get; protected set; } 
    public bool IsDead {get; protected set;}
    public List<AbilitySO> Moves { get; protected set; }
    
    public CombatClassSO CombatClass { get; protected set; }




//   public event Action HasDied;

   //level will draw from save
   protected Unit(UnitDataSO unit)
   {
       Portrait =  unit.Portrait;
       Name = unit.Name;
       //Fill in faction in child
       Level = unit.Level;
       XpToNextLevel = unit.ExPtsToNextLevel;
       TotalXp = unit.TotalExPtsGained;

       StatGrowths = unit.Aptitudes;
       Affinities = unit.Affinities;
       
       //TODO: 100% add a function in Formulae for calculating all stats 
       //actual stat values 
       Health = Formulae.CalculateStat(Stat.HP, StatGrowths[Stat.HP], Level);
       Energy = Formulae.CalculateStat(Stat.EN, StatGrowths[Stat.EN], Level);
       Strength = Formulae.CalculateStat(Stat.STR, StatGrowths[Stat.STR], Level);
       Defense = Formulae.CalculateStat(Stat.DEF, StatGrowths[Stat.DEF], Level);
       Tech = Formulae.CalculateStat(Stat.TEC, StatGrowths[Stat.TEC], Level);
       Resistance = Formulae.CalculateStat(Stat.RES, StatGrowths[Stat.RES], Level);
       Speed = Formulae.CalculateStat(Stat.SPE, StatGrowths[Stat.SPE], Level);
       Luck = Formulae.CalculateStat(Stat.LCK, StatGrowths[Stat.LCK], Level);
       Precision = Formulae.CalculateStat(Stat.PRC, StatGrowths[Stat.PRC], Level);

       HealthRemaining = Health;
       EnergyRemaining = Energy;

       Moves = unit.Moves;
       CombatClass = unit.CombatClass;
   }

   //placeholder
  
   protected void SetFaction(Faction value) { Faction = value; }


   public abstract void ChangeHealthRemaining(int value);
   public abstract void ChangeEnergyRemaining(int value);

   public abstract void Defend();
   public abstract void EndDefend();

   public Element[] DefaultAttackTypes()
   {
       return CombatClass.DefaultAttackElements; 
   }

   public int DefaultAttackRange()
   {
       return CombatClass.DefaultAttackRange;
   }


}
