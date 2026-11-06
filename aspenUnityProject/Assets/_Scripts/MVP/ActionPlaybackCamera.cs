
using _Scripts.Runtime.Managers;
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using _Scripts.Runtime.Entities.Scripts.Combat;

public class ActionPlaybackCamera : MonoBehaviour
{
    //variables for the camera and playback
    private CinemachineCamera virtualCamera;
    private Coroutine playbackCoroutine;

    //how long the camera stays on the unit
    public float playbackDuration = 2f;

    void Awake()
    {
        //gets the cinemachine camera
        virtualCamera = GetComponent<CinemachineCamera>();
        virtualCamera.Priority = 0;
    }

    //for following the unit when it does an action
    public void PlayAction(UnitController actingUnit)
    {
        //checks if there is a unit
        if (actingUnit == null)
            return;

        //stops the previous playback
        if (playbackCoroutine != null)
            StopCoroutine(playbackCoroutine);

        //camera follows the unit doing the action
        virtualCamera.Target.TrackingTarget = actingUnit.transform;
        virtualCamera.Priority = 10;

        //starts the playback timer
        playbackCoroutine = StartCoroutine(EndPlayback());
    }

    //for switching the camera back after the action
    private IEnumerator EndPlayback()
    {
        yield return new WaitForSeconds(playbackDuration);

        virtualCamera.Priority = 0;
        playbackCoroutine = null;
    }

    void OnEnable()
    {
        //checks when a unit starts an action
        CombatManager.unitActionStarted += PlayAction;
    }

    void OnDisable()
    {
        //stops checking when the script is disabled
        CombatManager.unitActionStarted -= PlayAction;

        //stops playback if the script is disabled
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }

        //resets the camera priority
        if (virtualCamera != null)
            virtualCamera.Priority = 0;
    }
}
