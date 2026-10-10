using _Scripts.Runtime.UI;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Helpers
{
    /// <summary>
    /// Attach this to the default Menu in the scene
    /// </summary>
    [RequireComponent(typeof(Menu))]
    public class DefaultSceneMenuSetter : MonoBehaviour
    {
        private Menu _menu;

        private void Awake()
        {
            _menu = GetComponent<Menu>();
            _menu.SetPreviousPanel(null);
            if (Menu.CurrentActiveMenu == null)
            {
                _menu.Focus();
            }
            else
            {
                Menu.CurrentActiveMenu.SetPreviousPanel(_menu);
            }
        }
    }
}