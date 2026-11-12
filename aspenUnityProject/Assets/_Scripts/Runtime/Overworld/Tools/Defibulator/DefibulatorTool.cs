using System;
using System.Collections.Generic;
using _Scripts.Runtime.Managers;
using _Scripts.Runtime.Overworld;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class DefibulatorTool : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Required] private OverworldPlayerController _playerController;
    [SerializeField, Required] private Transform _detectionOrigin;
    
    [Header("Config")]
    [SerializeField] private float _detectionRange = 10f;
    [SerializeField] private float _detectionWidth = 10f;
    [SerializeField] private float _detectionHalfAngle = 85f;
    [SerializeField] private float _cooldownDuration = 2f;
    
    public enum State
    {
        Ready,
        Activated,
        Cooldown
    }
    [SerializeField, ReadOnly] private State _currentState = State.Ready;

    private PowerSource _targetPowerSource;
    private float _cooldownTimer;
    
    private void OnEnable()
    {
        InputManager.Instance.Actions.Player.PrimaryToolAction.performed += Input_PrimaryToolActionPerformed;
    }

    private void OnDisable()
    {
        if(InputManager.Instance != null)
            InputManager.Instance.Actions.Player.PrimaryToolAction.performed -= Input_PrimaryToolActionPerformed;
    }
    
    private void Update()
    {
        UpdateState(_currentState);
    }

    private void FixedUpdate()
    {
        FixedUpdateState(_currentState);
    }

    private void OnDrawGizmos()
    {
        if (_playerController == null)
        {
            Debug.LogWarning($"{GetType()} is missing reference to PlayerController");
            return;
        }
        
        Gizmos.color = _currentState == State.Ready ? Color.green : Color.red;
        ConeEllipseDetection.DrawGizmos(_detectionOrigin.position, _playerController.transform.forward, _detectionRange,
            _detectionWidth, _detectionHalfAngle);
    }

    private void ChangeState(State newState, bool forceChange = false)
    {
        if (!forceChange && _currentState == newState)
            return;
        
        OnStateExit(_currentState);
        _currentState = newState;
        OnStateEnter(_currentState);
    }

    private void OnStateEnter(State state)
    {
        switch (state) 
        {
            case State.Ready:
                _targetPowerSource = null;
                break;
            case State.Activated:
                ChangeState(State.Cooldown);
                break;
            case State.Cooldown:
                _cooldownTimer = 0f;
                break;
        }
    }

    private void UpdateState(State state)
    {
        switch (state) 
        {
            case State.Ready:
                break;
            case State.Activated:
                break;
            case State.Cooldown:
                _cooldownTimer += Time.deltaTime;
                if (_cooldownTimer >= _cooldownDuration)
                {
                    ChangeState(State.Ready);
                    return;
                }
                break;
        }
    }
    
    private void FixedUpdateState(State state)
    {
        switch (state) 
        {
            case State.Ready:
                break;
            case State.Activated:
                break;
            case State.Cooldown:
                break;
        }
    }

    private void OnStateExit(State state)
    {
        switch (state) 
        {
            case State.Ready:
                break;
            case State.Activated:
                break;
            case State.Cooldown:
                break;
        }
    }
    
    private void Input_PrimaryToolActionPerformed(InputAction.CallbackContext context)
    {
        TryActivate();
    }

    private void TryActivate()
    {
        if (_currentState != State.Ready)
            return;

        if (!TryGetClosestPowerSource(out _targetPowerSource))
            return;

        if (!_targetPowerSource.TryActivate())
            return;
        
        ChangeState(State.Activated);
    }

    private bool TryGetClosestPowerSource(out PowerSource outputSource)
    {
        outputSource = null;
        
        List<GameObject> detectedObjects = ConeEllipseDetection.FindObjects(
            _detectionOrigin.position, 
            _playerController.transform.forward, 
            _detectionRange, 
            _detectionWidth, 
            _detectionHalfAngle,
            Physics.AllLayers
        );
        
        HashSet<PowerSource> powerSources = new();
        foreach (GameObject detectedObject in detectedObjects)
        {
            if(detectedObject.TryGetComponent(out PowerSource powerSource))
                powerSources.Add(powerSource);
        }
        
        float closestDistance = float.MaxValue;
        foreach (PowerSource powerSource in powerSources)
        {
            float distance = Vector3.Distance(powerSource.transform.position, _playerController.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                outputSource = powerSource;
            }
        }
        
        return outputSource != null;
    }
}
