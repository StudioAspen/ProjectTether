using _Scripts.Runtime.Characters.Scripts;
using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts.Combat
{
  [CreateAssetMenu(fileName = "NewPlayableCharacterData", menuName = "Scriptable Objects/Unit/Playable Character", order = 0)]
  public class PlayableCharacterUnitSO : UnitDataSO
  {
    public override Faction Faction => Faction.Ally;

    [Tooltip("need one suit for each class")]
    [SerializeField] private SuitBaseDataSO[] _suits; 
    public SuitBaseDataSO[] Suits => _suits;

  }
}
