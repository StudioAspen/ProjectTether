using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Consystently.UI
{
  public class UnitVitals : MonoBehaviour
  {
    [Header("Vitals")]
    [SerializeField] private Image _portrait;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _energyText;

    public void Initialize()
    {
      
    }
  }
}