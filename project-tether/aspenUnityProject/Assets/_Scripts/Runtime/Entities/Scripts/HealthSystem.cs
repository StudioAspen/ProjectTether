using UnityEngine;

namespace Tether.CharacterSystems
{
  public class HealthSystem : MonoBehaviour, IInitializer
  {
    private UnitDataSO _unitData;
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public void Intialize(UnitDataSO unitData)
    {
      _unitData ??= unitData;

      MaxHealth = Formulae.CalculateStat(Stat.HP, _unitData.Health, _unitData.Level);
      CurrentHealth = Mathf.Clamp(Mathf.RoundToInt(_unitData.RemainingHealth), 0, MaxHealth);
    }
  }
}
