using DG.Tweening;
using UnityEngine;

namespace _Scripts.Runtime.Overworld.Tools.Grapple
{
    public class GrappleProjectile : MonoBehaviour
    {
        private GrappleTool _tool;

        public void Shoot(GrappleTool tool, Vector3 destination, float speed)
        {
            _tool = tool;
            
            float distanceToDestination = Vector3.Distance(transform.position, destination);
            float tweenDuration = distanceToDestination / speed;
            transform.DOMove(destination, tweenDuration).OnComplete(OnDestinationReached);
        }

        private void OnDestinationReached()
        {
            _tool.OnProjectileReachedDestination();
        }
    }
}