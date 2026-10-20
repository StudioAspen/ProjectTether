using _Scripts.Runtime.Persistent_Data;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Runtime.Entities.Scripts.Combat
{
    /// <summary>
    /// For dev use
    /// </summary>
    public class BattleDevTools : MonoBehaviour
    {
        [Button("Win", EButtonEnableMode.Playmode)]
        public void WinBattle()
        {
            GameSession.Instance.MarkCurrentOverworldEnemyDefeated();
        }

        [Button("Lose", EButtonEnableMode.Playmode)]
        public void LoseBattle()
        {
            GameSession.Instance.PlayerData.OverworldPosition = Vector3.zero; // So it doesnt respawn on the undefeated enemy
            GameSession.Instance.ClearCurrentOverworldEnemyId();
        }
    }
}