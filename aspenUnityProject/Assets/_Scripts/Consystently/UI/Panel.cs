namespace Consystently.UI
{
  using System.Collections.Generic;
  using System.Linq;
  using UnityEngine;

  public class Panel : VisualElement
  {
    [Header("Panel")]
    [SerializeField] protected string panelName;
    public string Name => panelName;
    public GameMenu Menu { get; protected set; }

    /// <summary>
    /// This panel's index in its menu's panel stack. 0 is reserved for the menu itself.
    /// </summary>
    [Range(1, 99)]
    public int StackIndex;

    [SerializeField] protected bool hideInStack = true;

    public bool Opened { get; protected set; } = true;

    public HashSet<VisualElement> Elements { get; protected set; } = new HashSet<VisualElement>();

    public void Initialize(GameMenu menu)
    {
      Menu ??= menu;

      if (Menu == menu)
      {
        Menu.AddPanelToSet(this);

        Elements = GetComponentsInChildren<VisualElement>().ToHashSet();
        foreach (VisualElement e in Elements)
          e.AssignRootPanel(this);
        
        Close();
      }
    }

    protected virtual void OnDestroy()
    {
      Menu.RemovePanelFromSet(this);
    }

    public bool Open()
    {
      if(Opened)
      {
        // Debug.LogWarning($"{panelName} panel is already open.");
        return false;
      }
      else
      {
        Show();
        Opened = true;
        // Debug.Log($"{panelName} panel successfully opened.");
        return true;
      }
    }

    public bool Close()
    {
      if(!Opened)
      {
        // Debug.LogWarning($"{panelName} panel is already closed.");
        return false;
      }
      else
      {
        Hide();
        Opened = false;
        // Debug.Log($"{panelName} panel successfully closed.");
        return true;
      }
    }

    public override void Show()
    {
      _canvasGroup.interactable = true;
      _canvasGroup.blocksRaycasts = true;
      if(hideInStack)
      {
        _canvasGroup.alpha = 1;
        // Debug.Log($"{panelName} panel shown.");
      }
    }

    public override void Hide()
    {
      _canvasGroup.interactable = false;
      _canvasGroup.blocksRaycasts = false;
      if(hideInStack)
      {
        _canvasGroup.alpha = 0;
        // Debug.Log($"{panelName} panel hidden.");
      }
    }
  }
}