using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts
{
  [CreateAssetMenu(fileName = "NewPlayableCharacterData", menuName = "Scriptable Objects/Unit/Playable Character", order = 0)]
  public class PlayableCharacterUnitSO : UnitDataSO
  {
    public override Faction Faction => Faction.Ally;
  }
}
