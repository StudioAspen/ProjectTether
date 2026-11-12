using _Scripts.Runtime.Managers;
using UnityEngine;

public class OverworldTransition : MonoBehaviour
{
    public void GoToScene(string scenename)
    {
        // this does what it says
        GameManager.ChangeScene(scenename);
    }
}
