using DG.Tweening;
using UnityEngine;

namespace _Scripts.Runtime.Overworld.Tools.Grapple
{
    public class GrappleProjectile : MonoBehaviour
    {
        private GrappleTool _tool;
        private Grappleable _grappleable;

        public void Shoot(GrappleTool tool, Grappleable destinationGrappleable, Vector3 destination, float speed)
        {
            _tool = tool;
            _grappleable = destinationGrappleable;
            
            float distanceToDestination = Vector3.Distance(transform.position, destination);
            float tweenDuration = distanceToDestination / speed;
            transform.DOMove(destination, tweenDuration).OnComplete(()=>
            {
                transform.position = destination;
                OnDestinationReached();
            });
        }

        private void OnDestinationReached()
        {
            LockOntoGrappleable();
            _tool.OnProjectileReachedDestination();
        }

        private void LockOntoGrappleable()
        {
            if (_grappleable == null)
                return;

            // Preserve the projectile's world pose at impact while making it follow the target.
            transform.SetParent(_grappleable.transform, true);
        }
    }
}
