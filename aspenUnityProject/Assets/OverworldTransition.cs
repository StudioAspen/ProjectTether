using _Scripts.Runtime.Managers;
using UnityEngine;
using _Scripts.Runtime.Persistent_Data;
using Unity.VisualScripting;


public class OverworldTransition : MonoBehaviour
{
    [SerializeField] Vector3 NewGamePosition;
    public void GoToScene(string scenename)
    {
        // this does what it says
        GameManager.ChangeScene(scenename);
    }

    public void GoToSceneWithPosition(string scenename)
    {
        // since the player controller is set to used saved position at the start we want to change it if they are starting a new game
        GameSession.Instance.PlayerData.OverworldPosition=NewGamePosition;
        GameSession.Instance.Save();
        GameManager.ChangeScene(scenename);

    }
}
