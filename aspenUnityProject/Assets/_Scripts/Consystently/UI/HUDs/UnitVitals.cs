using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Consystently.UI
{
  public class UnitVitals : MonoBehaviour
  {
    [Header("Vitals")]
    [SerializeField] private Image _portrait;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _energyBar;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _energyText;
    [SerializeField] private List<GameObject> _effects;

    public void Initialize()
    {
      
    }
  }
}