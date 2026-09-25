using TMPro;
using UnityEngine;

namespace _Scripts.Consystently.UI
{
  public class BattleLogHE : HUDElement
  {
    [Header("Battle Log")]
    [SerializeField] private RectTransform _container;
    [SerializeField] private TextMeshProUGUI _logText;

    protected override void Start()
    {
      base.Start();
      Hide();
    }
  }
}
