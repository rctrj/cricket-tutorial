using UnityEngine;
using UnityEngine.Events;

namespace Cricket.Balls
{
    public class BallDestroyer : MonoBehaviour
    {
        [SerializeField] private LayerMask ballLayerMask;
        [SerializeField] private UnityEvent<Ball> onBallDestroy;

        private void OnTriggerEnter(Collider collision)
        {
            var collisionObjectLayerMask = 1 << collision.gameObject.layer;
            if ((ballLayerMask & collisionObjectLayerMask) == 0) return;
            onBallDestroy?.Invoke(collision.gameObject.GetComponent<Ball>());
        }
    }
}