using System;
using UnityEngine;
using Sirenix.OdinInspector;
using _Scripts.Runtime.Persistent_Data;

namespace _Scripts.Runtime.Entities.Scripts.Overworld
{
    public class OverworldEnemyController : MonoBehaviour
    {
        [field: SerializeField] public string PersistentId { get; private set; }

        public void SetAsCurrentEncounterEnemy()
        {
            GameSession.Instance.SetCurrentOverworldEnemyId(PersistentId);
        }

        private void OnEnable()
        {
            if (GameSession.Instance.IsOverworldEnemyDefeated(PersistentId))
                gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        [Button("Generate New Persistent ID"), EnableIf("@!UnityEngine.Application.isPlaying")]
        private void GenerateNewPersistentId() => PersistentId = Guid.NewGuid().ToString("N");

        [Button("Validate Enemy IDs in Build Scenes"), EnableIf("@!UnityEngine.Application.isPlaying")]
        private void ValidateEnemyIdsInBuildScenes()
        {
            OverworldEnemyIdValidator.ValidateBuildScenes();
        }
#endif
    }
}
