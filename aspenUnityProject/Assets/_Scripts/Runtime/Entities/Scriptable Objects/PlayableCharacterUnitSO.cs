using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayableCharacterData", menuName = "Scriptable Objects/Unit/Playable Character", order = 0)]
public class PlayableCharacterUnitSO : UnitDataSO
{
  public override Faction Faction => Faction.Ally;
}
