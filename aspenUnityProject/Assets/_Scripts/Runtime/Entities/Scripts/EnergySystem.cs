using _Scripts.Runtime.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts
{
  public class EnergySystem : MonoBehaviour, IInitializer
  {
    private UnitDataSO _unitData;
    public int MaxEnergy { get; private set; }
    public int CurrentEnergy{ get; private set; }

    public void Intialize(UnitDataSO unitData)
    {
      _unitData ??= unitData;

      MaxEnergy = Formulae.CalculateStat(Stat.EN, _unitData.Energy, _unitData.Level);
      CurrentEnergy = Mathf.Clamp(Mathf.RoundToInt(_unitData.RemainingEnergy), 0, MaxEnergy);
    }
  }
}
