using System;

//TODO: xp system
public class EnemyUnit : Unit
{
   
   public event Action<EnemyUnit> OnDeath;
   public event Action<EnemyUnit> OnDefend;
   
   public EnemyUnit(UnitDataSO unit) : base(unit)
   {
      SetFaction(Faction.Enemy);
   }

   public override void ChangeHealthRemaining(int value)
   {
      HealthRemaining -= value; 
      if(HealthRemaining <= 0)
         OnDeath?.Invoke(this);
   }

   public override void Defend()
   {
      IsBlocking = true;
      OnDefend?.Invoke(this);
   }

   public override void EndDefend()
   {
      IsBlocking = false;
   }

   //can modify depending on difficulty desired 
   public override void ChangeEnergyRemaining(int value)
   {
      EnergyRemaining -= value;
   }
    
   
}
