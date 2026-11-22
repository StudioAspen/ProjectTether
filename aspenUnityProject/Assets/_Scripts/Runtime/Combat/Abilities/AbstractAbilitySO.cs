using System;
using System.Collections.Generic;
using _Scripts.Runtime.Combat.Statuses;
using _Scripts.Runtime.Entities.Scripts.Combat;
using _Scripts.Runtime.Tile_System.Scripts;
using _Scripts.Runtime.Math;
using UnityEngine;

namespace _Scripts.Runtime.Combat.Abilities
{
    public class AbstractAbilitySO : ScriptableObject
    {
           //e.g., fireball_0 or obliterate_0
      [SerializeField] private string id;
      public string Id => id;
      
      //name displayed in UI 
      [SerializeField] private string abilityName;
      public string Name => name;

      [SerializeField] private bool _isModifiedAbility;
      public bool IsModifiedAbility => _isModifiedAbility;

      [Tooltip("If Applicable")]
      [SerializeField] private AbilitySO _modifiedFrom;
      public bool ModifiedFrom => _modifiedFrom;

      [SerializeField][TextArea(3,10)] private string _description;
      public string Description => _description;

      [Tooltip("negative for healing")]
      [SerializeField] private int _damage;
      public int Damage => _damage;

      [SerializeField] private int _numberOfHits;
      public int NumberOfHits => _numberOfHits;

      [SerializeField] private BattleParametersList.BattleParameters[] _effectsDueToAbility;
      public BattleParametersList.BattleParameters[] EffectsDueToAbility => _effectsDueToAbility;

      [Tooltip("If the number is between 0 and 1 it is the percent change of the " +
               "battle parameter by the ability. If it is over one, it is simply an addition")]
      [SerializeField] private float[] _effectAmount;
      public float[] EffectAmount => _effectAmount;

      //prob remove 
      [SerializeField] private int[] _numberOfTurnsEffectLasts;
      public int[] NumberOfTurnsEffectLasts => _numberOfTurnsEffectLasts;

      [SerializeField] private Element[] element; 
      public Element[] Element => element;

      [SerializeField] private float _chanceToHit;
      public float ChanceToHit => _chanceToHit;

      //This field is primarily for a move called breakneck rush.
      //TODO: can add by adding an if to ability damage in CombatFormulas (use genericMathClass's hex distance calc) 
      [SerializeField] private float _hitChanceLossPerTileTravelled;
      public float HitChanceLossPerTileTraveled => _hitChanceLossPerTileTravelled;

      [SerializeField] private float _macCost;
      public float MACCost => _macCost;

      [SerializeField] private float _energyCost;
      public float EnergyCost => _energyCost;
      [SerializeField] private AbilityType abilityType;
      
      public AbilityType AbilityType => abilityType;

      //ring-based range. 0 = same tile only 1 = one ring around self, etc. 
      [Range(0, 3)] [SerializeField] private int range;
      public int Range => range;
    
      //area of effect. 0 = only 1 tile affected. 1 = 1 entire ring of tiles surrounding the tile selected affected, etc. 3 is effectively global
      [Range(0,3)] [SerializeField] private int aoe;
      public int AOE => aoe;

      [SerializeField] private bool hitsEnemies; 
      public bool HitsEnemies => hitsEnemies;
   
      //needs to be true for healing moves 
      [SerializeField] private bool hitsAllies;
      public bool HitsAllies => hitsAllies;

      //THIS here is unnecessary because self == ally 
      //needs to be true for healing moves 
      [SerializeField] private bool hitsSelf;
      public bool HitsSelf => hitsSelf;
      [SerializeField] private bool canTargetEmptyTile;
      public bool CanTargetEmptyTile => canTargetEmptyTile;
   
      [SerializeField] private bool canMiss;
      public bool CanMiss => canMiss;

      [SerializeField] private AbstractTerrainSO tileEffect;
      public AbstractTerrainSO TileEffect => tileEffect;

      // custom range. list of directions walked from the unit's tile
      // e.g. N, N, N = line of 3 tiles north
      [SerializeField] private List<List<CubeCoordDirections>> customRange;
      public List<List<CubeCoordDirections>> CustomRange => customRange;

      [SerializeField] private List<AbstractStatusSO> status; 
      public List<AbstractStatusSO> Status => status;


      public void SetName(string name) => abilityName = name;

      //mainly one target for now. 11.21.26 - no logic for additional targets. Add stuff/override if needed
      public virtual void Cast(UnitController caster, TileController target, params TileController[] additionalTargets)
      {
          target.SetTerrain(TileEffect); 
          foreach (UnitController enemy in target.UnitControllers)
          {
              CombatFormulas.AbilityDamage(caster, enemy, this);
              foreach (AbstractStatusSO i in status)
              {
                 enemy.GetData().AddStatus(i); 
              }
          }
      

          
      }

      /// <summary>
      /// checks custom range + regular radius range. Override if needed
      /// </summary>
      /// <param name="caster"></param>
      /// <param name="target"></param>
      /// <param name="additionalTargets"></param>
      /// <returns></returns>
      public virtual bool IsReachable(UnitController caster, TileController target, params TileController[] additionalTargets)
      {
          Vector3Int from = caster.TileCoords;
          // use custom range if the ability has one, if not then normal range
          bool tileWithinRange;
          if (CustomRange.Count > 0)
              tileWithinRange = CalcCustomRange(from, target.tileCoordinate);
          else
              tileWithinRange = target.tileCoordinate.HexGridDistance(from) <= Range;
          
          if (!tileWithinRange)
              return false;
          bool targetContainsUnit = target.UnitControllers.Count > 0;
          bool targetContainsAlly = false;
          bool targetContainsEnemy = false;
          foreach (UnitController uc in target.UnitControllers) //ASSUMES tiles can only contain either enemy/ally. No mixing
          {
              if (uc.GetData().Faction == caster.GetData().Faction)
                  targetContainsAlly = true;
              else
              {
                  targetContainsEnemy = true;
                  break;
              }
          }

          return (targetContainsAlly && HitsAllies) ||
                 (!targetContainsUnit && CanTargetEmptyTile) ||
                 (targetContainsEnemy && HitsEnemies);
      }

      protected virtual bool CalcCustomRange(Vector3Int origin, Vector3Int target)
      {
          Vector3Int current = origin;
          foreach (List<CubeCoordDirections> l in customRange)
          {
              foreach (CubeCoordDirections c in l)
              {
                  current += c.Vector();
                  if (current == target)
                      return true;
              }
              current = origin;
          }
          return false; 
      }
    
    }
}