namespace Consystently.UI
{
  using UnityEngine;

  [RequireComponent(typeof(CanvasGroup))]
  [DisallowMultipleComponent]
  public class VisualElement : MonoBehaviour
  {
    public Panel Panel { get; private set; }

    public void AssignRootPanel(Panel panel) => Panel ??= panel;

    protected CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
      _canvasGroup ??= GetComponent<CanvasGroup>();
    }

    public virtual void Show()
    {
      _canvasGroup.interactable = true;
      _canvasGroup.blocksRaycasts = true;
      _canvasGroup.alpha = 1;
    }

    public virtual void Hide()
    {
      _canvasGroup.interactable = false;
      _canvasGroup.blocksRaycasts = false;
      _canvasGroup.alpha = 0;
    }
  }
}