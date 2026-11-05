#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Runtime.Entities.Scripts.Overworld
{
    public static class OverworldEnemyIdValidator
    {
        public static void ValidateBuildScenes()
        {
            Scene originalActiveScene = SceneManager.GetActiveScene();
            List<Scene> scenesOpenedForValidation = new List<Scene>();
            Dictionary<string, string> firstLocationById = new Dictionary<string, string>();
            List<string> issues = new List<string>();

            try
            {
                foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
                {
                    if (!buildScene.enabled)
                        continue;

                    Scene scene = SceneManager.GetSceneByPath(buildScene.path);
                    if (!scene.IsValid() || !scene.isLoaded)
                    {
                        scene = EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Additive);
                        scenesOpenedForValidation.Add(scene);
                    }

                    ValidateScene(scene, firstLocationById, issues);
                }
            }
            finally
            {
                for (int i = scenesOpenedForValidation.Count - 1; i >= 0; i--)
                    EditorSceneManager.CloseScene(scenesOpenedForValidation[i], true);

                if (originalActiveScene.IsValid() && originalActiveScene.isLoaded)
                    SceneManager.SetActiveScene(originalActiveScene);
            }

            if (issues.Count == 0)
            {
                Debug.Log("Overworld enemy ID validation passed for all enabled Build Settings scenes.");
                return;
            }

            StringBuilder report = new StringBuilder();
            report.AppendLine($"Found {issues.Count} overworld enemy ID issue(s):");
            foreach (string issue in issues)
                report.AppendLine($"- {issue}");

            Debug.LogError(report.ToString());
        }

        private static void ValidateScene(
            Scene scene,
            Dictionary<string, string> firstLocationById,
            List<string> issues)
        {
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                OverworldEnemyController[] enemies = root.GetComponentsInChildren<OverworldEnemyController>(true);
                foreach (OverworldEnemyController enemy in enemies)
                {
                    string location = $"{scene.path} ({GetHierarchyPath(enemy.transform)})";
                    string id = enemy.PersistentId;

                    if (string.IsNullOrWhiteSpace(id))
                    {
                        issues.Add($"Missing ID on {location}");
                        continue;
                    }

                    if (firstLocationById.TryGetValue(id, out string firstLocation))
                        issues.Add($"Duplicate ID '{id}' on {firstLocation} and {location}");
                    else
                        firstLocationById.Add(id, location);
                }
            }
        }

        private static string GetHierarchyPath(Transform target)
        {
            string path = target.name;
            while (target.parent != null)
            {
                target = target.parent;
                path = $"{target.name}/{path}";
            }

            return path;
        }
    }
}
#endif
