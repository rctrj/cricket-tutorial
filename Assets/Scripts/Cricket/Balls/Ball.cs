using Utils;
using UnityEngine;

namespace Cricket.Balls
{
    [DefaultExecutionOrder(-1)]
    public class Ball : MonoBehaviour, IPoolable
    {
        private const float BallMovingTolerance = 0;

        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private LayerMask boundaryLayerMask;
        [SerializeField] private LayerMask ballDestroyerLayerMask;

        public Pool<Ball> Pool;

        private int _dropCount;

        private Rigidbody _rb;

        public bool HitByBat { get; private set; }
        public bool WasDropped => _dropCount != 0;
        public bool HasStopped => Rigidbody.velocity.magnitude <= BallMovingTolerance;
        public bool CrossedBoundary { get; private set; }

        public Rigidbody Rigidbody => _rb;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        private void OnTriggerEnter(Collider collision)
        {
            var collisionObjectLayerMask = 1 << collision.gameObject.layer;

            CheckBoundary(collisionObjectLayerMask);
            CheckDestruction(collisionObjectLayerMask);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var collisionObjectLayerMask = 1 << collision.gameObject.layer;
            CheckHitWithGround(collisionObjectLayerMask);
        }

        public void Reset()
        {
            _dropCount = 0;
            CrossedBoundary = false;

            HitByBat = false;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.useGravity = false;

            gameObject.SetActive(false);
        }

        /// <summary>
        ///     To be called whenever hit by bat
        /// </summary>
        public void Hit()
        {
            _dropCount = 0;
            HitByBat = true;
        }

        public void Free() => Pool.Free(this);

        private void CheckHitWithGround(int collisionObjectLayerMask)
        {
            if ((groundLayerMask & collisionObjectLayerMask) == 0) return;
            if (!CrossedBoundary) _dropCount++;
        }

        private void CheckBoundary(int collisionObjectLayerMask)
        {
            if ((boundaryLayerMask & collisionObjectLayerMask) == 0) return;
            CrossedBoundary = true;
        }

        private void CheckDestruction(int collisionObjectLayerMask)
        {
            if ((ballDestroyerLayerMask & collisionObjectLayerMask) == 0) return;
            Pool.Free(this);
        }
    }
}