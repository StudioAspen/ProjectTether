using System;
using Tether.CharacterSystems;
using UnityEngine;

//this script must be dragged to an object before making it a prefab and dragging the prefab to a unit SO.
public class AllyUnitController : UnitController {
   private AllyUnit stats;
   

   //to be called by the combat maanager 
   public override void Initialize(UnitDataSO baseStats)
   {
      stats = new AllyUnit(baseStats);
   }

   /*
    TODO:
    add movement that is disabled on combat game state  
   */

   public override void TakeDamage(int damage)
   {
      stats?.ChangeHealthRemaining(damage); 
   }

   //will differ from SetTile in that it will consider game logic with conditionals 
   public override void TryMove(Vector3 position, Vector3Int coords)
   {
      SetTile(coords);
      MoveInvoke(position);
      HasMoved = true;
   }

   public override void MoveInvoke(Vector3 position)
   {
      Move(position);
      OnMove(this);
   }

   //used by managers to move the unit without triggering events
   public override void Move(Vector3 position)
   {
     transform.position = position; 
   }
   
   public override Unit GetData()
   {
      return stats; 
   }

   public override void ResetValues()
   {
      HasMoved = false;
      stats.EndDefend();
   }
}
