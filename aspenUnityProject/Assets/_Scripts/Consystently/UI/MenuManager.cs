namespace Consystently.UI
{
  using UnityEngine;
  using System.Collections.Generic;
  using Essentials;
  using System.Linq;

  [RequireComponent(typeof(InterfaceFunctions))]
  //Why is this a singleton? 
  public class MenuManager : Manager<MenuManager>
  {
    // Only one Menu shall be active at a time.
    public GameMenu ActiveMenu { get; protected set; }

    // All Menu Objects shall be added to this List.
    public HashSet<GameMenu> MenuSet { get; protected set; } = new HashSet<GameMenu>();

    public void Start() => Debug.Log("Press the Space Bar to open the Main Menu");
    public void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        OpenMenu(MenuSet.FirstOrDefault(item => item.Name == "Main Menu"));
        Debug.Log("test");
      }
    }

    public void AddMenuToSet (GameMenu menu)
    {
      if (!MenuSet.Contains(menu))
      {
        MenuSet.Add(menu);
        // Debug.Log ($"{menu.Name} has been added to the Menu Manager's hash set.");
      }
      // else
        // Debug.LogWarning($"Add failed. {menu.Name} is already present in the Menu Manager's hash set!");
    }

    public void RemoveMenuFromSet (GameMenu menu)
    {
      if (MenuSet.Contains(menu))
      {
        MenuSet.Remove(menu);
        // Debug.Log ($"{menu.Name} has been removed the Menu Manager's hash set.");
      }
      // else
        // Debug.LogWarning($"Remove failed. {menu.Name} was not found in the Menu Manager's hash set!");
    }

    public void OpenMenu (GameMenu gameMenu)
    {
      if (ActiveMenu != gameMenu)
      {
        // Debug.Log($"Opening {gameMenu.Name}...");
        if(gameMenu.Open())
        {
          CloseActiveMenu();
          ActiveMenu = gameMenu;
        }
      }
    }

    public void OpenPanel (string panelName) => ActiveMenu.OpenPanel(panelName);

    public void CloseActiveMenu ()
    {
      if (ActiveMenu)
      {
        // Debug.Log($"Closing {ActiveMenu.Name}...");
        if(ActiveMenu.Close())
          ActiveMenu = null;
      }
    }
  }
}
