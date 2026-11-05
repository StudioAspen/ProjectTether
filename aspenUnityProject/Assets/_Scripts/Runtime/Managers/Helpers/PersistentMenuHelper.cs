using _Scripts.Runtime.UI;
using UnityEngine;

namespace _Scripts.Runtime.Managers.Helpers
{
    public class PersistentMenuHelper : MonoBehaviour
    {
        public void FocusPauseMenu()
        {
            Menu.Focus(PersistentMenuManager.Instance.PauseMenu);
        }

        public void FocusSettingsMenu()
        {
            Menu.Focus(PersistentMenuManager.Instance.SettingsMenu);
        }
    }
}