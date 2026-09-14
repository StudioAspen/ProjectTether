namespace Consystently.UI
{
  using TMPro;
  using UnityEngine;
  using UnityEngine.EventSystems;
  using UnityEngine.UI;

  [RequireComponent(typeof(Button), typeof(Image), typeof(EventTrigger))]
  public class ButtonVE : VisualElement
  {
    [Header("Button")]
    [SerializeField] protected Image _iconChild;
    public Image IconChild => _iconChild;
    [SerializeField] protected TextMeshProUGUI _textChild;
    public TextMeshProUGUI TextChild => _textChild;

    public Image BackgroundImage { get; protected set; }
    public Button Component { get; protected set; }
    public EventTrigger Trigger { get; protected set; }

    protected override void Awake()
    {
      base.Awake();
      BackgroundImage ??= GetComponentInChildren<Image>();
      Component ??= GetComponent<Button>();
      Trigger ??= GetComponent<EventTrigger>();

      Component.interactable = Component.onClick.GetPersistentEventCount() > 0;
    }
  }
}
