using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Persistent_Data;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Runtime.Combat
{
    public class EncounterStarter : MonoBehaviour
    {
        [SerializeField, Required] private EncounterSO _encounterData;

        [Button("Start Encounter", EButtonEnableMode.Playmode)]
        public void StartEncounter()
        {
            if (_encounterData == null)
            {
                Debug.LogWarning($"EncounterSO missing in EncounterStarter");
                return;
            }
            
            Encounter encounter = _encounterData.GenerateEncounter();
            
            PersistentData.SetCurrentEncounterData(encounter);
            GameManager.Instance.ChangeGameState(GameManager.State.Combat);
        }
    }
}