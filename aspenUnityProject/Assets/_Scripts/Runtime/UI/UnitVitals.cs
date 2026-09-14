using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Consystently.UI
{
  public class UnitVitals : MonoBehaviour
  {
    [Header("Vitals")]
    [SerializeField] private Image portrait;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI energyText;
  }
}