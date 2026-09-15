using UnityEngine;


namespace Consystently.UI
{
  [RequireComponent(typeof(CanvasGroup))]
  [DisallowMultipleComponent]
  public class HUDElement : MonoBehaviour
  {
    [Header("HUD Element")]
    [SerializeField] protected string hudName;
    public string Name => hudName;

    public HUD Hud { get; protected set; }
    protected CanvasGroup _canvasGroup;
    protected virtual void Start()
    {
      _canvasGroup ??= GetComponent<CanvasGroup>();
    }

    protected virtual void Show()
    {
      _canvasGroup.interactable = true;
      _canvasGroup.blocksRaycasts = true;
      _canvasGroup.alpha = 1;
    }

    protected virtual void Hide()
    {
      _canvasGroup.interactable = false;
      _canvasGroup.blocksRaycasts = false;
      _canvasGroup.alpha = 0;
    }
  }
}