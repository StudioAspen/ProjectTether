using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Persistent_Data;
using _Scripts.Runtime.Entities.Scripts.Overworld;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.Runtime.Combat
{
    public class EncounterStarter : MonoBehaviour
    {
        [SerializeField, Required] private EncounterSO _encounterData;

        [Button("Start Encounter"), EnableIf("@UnityEngine.Application.isPlaying")]
        public void StartEncounter()
        {
            if (_encounterData == null)
            {
                Debug.LogWarning($"EncounterSO missing in EncounterStarter");
                return;
            }
            
            GameSession.Instance.SetCurrentEncounterData(_encounterData);
            GameManager.Instance.ChangeGameState(GameManager.State.Combat);
        }
    }
}
