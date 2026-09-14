using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Consystently.UI
{
  public interface IPanelHandler
  {
    void OpenPanel(GameMenu gameMenu);
    void ClosePanel(GameMenu gameMenu);
  }

  [RequireComponent(typeof(CanvasGroup))]
  [DisallowMultipleComponent]
  public class GameMenu : MonoBehaviour
  {
    [Header("Game Menu")]
    [SerializeField] protected string menuName;
    public string Name => menuName;

    [SerializeField] protected Panel defaultPanel;
    public Panel DefaultPanel => defaultPanel;

    public HashSet<Panel> PanelSet = new HashSet<Panel>();

    public Stack<Panel> PanelStack = new Stack<Panel>();

    public bool Opened { get; protected set; } = true;

    protected CanvasGroup _canvasGroup;

    protected virtual void Start()
    {
      _canvasGroup ??= GetComponent<CanvasGroup>();
      MenuManager.Instance.AddMenuToSet(this);

      foreach (Panel panel in GetComponentsInChildren<Panel>())
        panel.Initialize(this);
        
      Close();
    }

    protected virtual void OnDestroy()
    {
      MenuManager.Instance.RemoveMenuFromSet(this);
    }

    public bool Open()
    {
      if (Opened)
      {
        // Debug.LogWarning($"{menuName} is already open.");
        return false;
      }
      else if (!defaultPanel)
      {  
        // Debug.LogError($"Default Panel has not been assigned.");
        return false;
      }
      else
      {
        ClearStack();
        OpenPanel(defaultPanel);
        Opened = true;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
        // Debug.Log($"{menuName} successfully opened.");
        return true;
      }
    }

    public bool Close()
    {
      if (!Opened)
      {
        // Debug.LogWarning($"{menuName} is already closed.");
        return false;
      }
      else
      {
        ClearStack();
        Opened = false;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
        // Debug.Log($"{menuName} successfully closed.");
        return true;
      }
    }

    public void AddPanelToSet (Panel panel)
    {
      if (!PanelSet.Contains(panel))
      {
        PanelSet.Add(panel);
        // Debug.Log ($"{panel.Name} has been added to the {menuName} Menu's panel stack.");
      }
      // else
        // Debug.LogWarning($"Add failed. {panel.Name} is already present in the {menuName} Menu's panel stack!");
    }

    public void RemovePanelFromSet (Panel panel)
    {
      if (PanelSet.Contains(panel))
      {
        PanelSet.Remove(panel);
        // Debug.Log ($"{panel.Name} has been removed from the {menuName} Menu's panel stack.");
      }
      // else
        // Debug.LogWarning($"Remove failed. {panel.Name} was not found in the {menuName} Menu's panel stack!");
    }





    public void OpenPanel (Panel newPanel)
    {
      if (!PanelStack.Contains(newPanel))
      {
        // Debug.Log($"Opening {newPanel.Name} panel...");
        if (newPanel.Open())
        {
          if(PanelStack.Count > 0) 
            PanelStack.Peek().Hide();
          
          PanelStack.Push(newPanel);
          newPanel.StackIndex = PanelStack.Count;
        }
      }
      // else
        // Debug.LogWarning($"{newPanel.Name} panel is already in the open in the {menuName} Menu's panel stack.");
    }

    public void ClosePanel (Panel panel)
    {
      if (PanelStack.Peek() == panel)
      {
        // Debug.Log($"Closing {panel.Name} panel...");
        if(panel.Close())
        {
          PanelStack.Pop();
          panel.StackIndex = 1;

          if(PanelStack.Count > 0) 
            PanelStack.Peek().Show();
        }
      }
      // else
        // Debug.LogWarning($"{panel.Name} panel is not at the top of the {menuName} Menu's panel stack.");
    }
    
    public void OpenPanel(string panelName)
    {
      Panel panel = PanelSet.FirstOrDefault(p => p.Name == panelName);

      if (panel)
        OpenPanel(panel);
      // else
        // Debug.LogWarning($"Panel with the name '{panelName}' not found in the {menuName} Menu's panel set.");
    }

    public void ClosePanel(string panelName)
    {
      Panel panel = PanelSet.FirstOrDefault(p => p.Name == panelName);

      if (panel)
        ClosePanel(panel);
      // else
        // Debug.LogWarning($"Panel with the name '{panelName}' not found in the {menuName} Menu's panel set.");
    }

    public void ClearStack ()
    {
      foreach (Panel panel in PanelStack.Reverse())
        ClosePanel(panel);
    }
  }
}


