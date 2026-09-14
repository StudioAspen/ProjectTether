namespace Consystently.UI
{
  using UnityEngine;
  using UnityEngine.UI;

  [RequireComponent(typeof(Image))]
  public class ImageVE : VisualElement
  {
    // [Header("Image")]
    public Image Component { get; protected set; }

    protected override void Awake()
    {
      base.Awake();
      Component ??= GetComponent<Image>();
    }
  }
}