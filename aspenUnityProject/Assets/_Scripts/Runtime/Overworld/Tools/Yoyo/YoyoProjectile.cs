using DG.Tweening;
using UnityEngine;

namespace _Scripts.Runtime.Overworld.Tools.Yoyo
{
    [RequireComponent(typeof(Rigidbody))]
    public class YoyoProjectile : MonoBehaviour
    {
        private YoyoTool _tool;
        private Vector3 _destination;
    
        public void Init(YoyoTool tool, Vector3 destination)
        {
            _tool = tool;
            _destination = destination;
        }
    
        public void Extend(float speed, Ease ease)
        {
            transform.DOKill();
            float duration = Vector3.Distance(_destination, transform.position) / speed;
            transform.DOMove(_destination, duration).SetEase(ease).OnComplete(OnExtendFinished);
        }

        private void OnExtendFinished()
        {
            _tool.OnProjectileDestinationReached();
        }

        public void Retract(float speed, Ease ease)
        {
            transform.DOKill();
            float duration = Vector3.Distance(_tool.transform.position, transform.position) / speed;
            transform.DOMove(_tool.transform.position, duration).SetEase(ease).OnComplete(OnRetractComplete);
        }

        private void OnRetractComplete()
        {
            _tool.OnRetractComplete();
            Destroy(gameObject);
        }
    }
}