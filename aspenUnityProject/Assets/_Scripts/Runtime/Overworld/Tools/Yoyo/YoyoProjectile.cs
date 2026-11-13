using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class YoyoProjectile : MonoBehaviour
{
    private YoyoTool _tool;
    private Vector3 _destination;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Init(YoyoTool tool, Vector3 destination)
    {
        _tool = tool;
        _destination = destination;
    }
    
    public void Extend(float speed, Ease ease)
    {
        float duration = Vector3.Distance(_destination, transform.position) / speed;
        _rigidbody.DOMove(_destination, duration).SetEase(ease).OnComplete(OnDestinationReached);
    }

    private void OnDestinationReached()
    {
        _tool.OnProjectileDestinationReached();
    }

    public void Retract(float speed, Ease ease)
    {
        float duration = Vector3.Distance(_tool.transform.position, transform.position) / speed;
        _rigidbody.DOMove(_tool.transform.position, duration).SetEase(ease);
    }
}