#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace _Scripts.Runtime.Save
{
    public static class SaveManager
    {
        public static SaveStore SettingsStore { get; private set; }
        public static SaveStore GameStore { get; private set; }

        static SaveManager()
        {
            InitializeSaveStores();
        }

        private static void InitializeSaveStores()
        {
            SettingsStore = new SaveStore("settings", false);
            GameStore = new SaveStore("gameData", true);
        }

#if UNITY_EDITOR
        [MenuItem("Save Manager/Open Save Folder", false, 0)]
        private static void OpenSaveFolder()
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath);
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("Save Manager/Wipe Save Data", false, 20)]
        private static void WipeSaveData()
        {
            if (!EditorUtility.DisplayDialog(
                    "Wipe Save Data",
                    $"This clears the settings and game data saves in:\n{Application.persistentDataPath}",
                    "Wipe Save Data",
                    "Cancel"))
                return;

            SettingsStore.DeleteAll();
            GameStore.DeleteAll();
            Debug.Log("Save data cleared.");
        }

        [MenuItem("Save Manager/Wipe Save Data", true)]
        private static bool CanWipeSaveData() => !EditorApplication.isPlayingOrWillChangePlaymode;
#endif
    }
}
