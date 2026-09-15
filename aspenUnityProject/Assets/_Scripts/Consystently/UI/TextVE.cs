namespace Consystently.UI
{
  using TMPro;
  using UnityEngine;
  
  [RequireComponent(typeof(TextMeshProUGUI))]
  public class TextVE : VisualElement
  {
    [Header("Text")]
    public TextMeshProUGUI Component { get; private set; }

    protected override void Awake()
    {
      base.Awake();
      Component ??= GetComponent<TextMeshProUGUI>();
    }
  }
}
