using _Scripts.Runtime.Combat;
using _Scripts.Runtime.Entities.Scripts.Combat;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts
{
  //old code. Delete if there is no use in the future
  [RequireComponent(typeof(HealthSystem), typeof(EnergySystem))]
  public abstract class Entity : MonoBehaviour
  {
    [Header("Entity")]
    [SerializeField] private UnitDataSO _unitData;
    public UnitDataSO UnitData => _unitData;
    protected HealthSystem _healthSystem;
    protected EnergySystem _energySystem;

    protected virtual void Awake()
    {
      _healthSystem ??= GetComponent<HealthSystem>();
      _energySystem ??= GetComponent<EnergySystem>();

      _healthSystem.Intialize(_unitData);
      _energySystem.Intialize(_unitData);
    }

  }
}
