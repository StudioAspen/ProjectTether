namespace Consystently.UI
{
  using Consystently.Essentials;
  using UnityEngine;

  [DisallowMultipleComponent]
  public class InterfaceFunctions : MonoBehaviour
  {
   // public void PauseGame() => GameManager.Instance?.PauseGame();

    public void QuitApplication() => GameManager.Instance?.QuitApplication();

    public void OpenMenu(GameMenu gameMenu) => MenuManager.Instance?.OpenMenu(gameMenu);

    public void CloseActiveMenu() => MenuManager.Instance?.CloseActiveMenu();

    public void OpenPanel(Panel panel) => panel.Menu?.OpenPanel(panel);

    public void ClosePanel(Panel panel) => panel.Menu?.ClosePanel(panel);

    public void ClearStack(GameMenu gameMenu) => gameMenu.ClearStack();

    public void ClearStack(Panel panel) => panel.Menu?.ClearStack();
  }
}

