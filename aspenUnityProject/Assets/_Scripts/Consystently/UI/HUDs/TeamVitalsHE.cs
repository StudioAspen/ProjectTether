using UnityEngine;

namespace Consystently.UI
{
  public class TeamVitalsHE : HUDElement
  {
    [Header("Team Vitals")]
    [SerializeField] protected RectTransform _vitalsPanel;
    [SerializeField] protected UnitVitals _unitVitalsPrefab;

    public UnitVitals[] UnitVitalsObjects { get; protected set; } = new UnitVitals[4];

    protected override void Start()
    {
      base.Start();
      for (int i = 0; i < UnitVitalsObjects.Length; i++)
      {
        UnitVitalsObjects[i] ??= Instantiate(_unitVitalsPrefab, _vitalsPanel);
        UnitVitalsObjects[i].Initialize();
      }
    }
  }
}