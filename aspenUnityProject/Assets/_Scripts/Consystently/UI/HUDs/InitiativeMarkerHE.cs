using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Consystently.UI
{
  public class InitiativeMarkerHE : HUDElement
  {
    [Header("Initiative Marker")]
    [SerializeField] protected Image _portrait;
    [SerializeField] protected Slider _healthBar;
      // Start is called once before the first execution of Update after the MonoBehaviour is created
      protected override void Start()
      {
          base.Start();
          //Need logic to know what unit this marker belongs to, and to update its health to match the unit's current HP
      }
  }
}

