using Utils;
using UnityEngine;

namespace Cricket.Balls
{
    public class Ball : MonoBehaviour, IPoolable
    {
        private const float BallMovingTolerance = 0;

        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private LayerMask boundaryLayerMask;
        [SerializeField] private LayerMask ballDestroyerLayerMask;

        public Pool<Ball> Pool;

        private bool _crossedBoundary;
        private int _dropCount;

        private Rigidbody _rb;

        public bool HitByBat { get; private set; }
        public bool WasDropped => _dropCount == 0;
        public bool HasStopped => Rigidbody.velocity.magnitude <= BallMovingTolerance;

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
            _crossedBoundary = false;

            HitByBat = false;
            _rb.velocity = Vector3.zero;
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

        private void CheckHitWithGround(int collisionObjectLayerMask)
        {
            if ((groundLayerMask & collisionObjectLayerMask) == 0) return;
            if (!_crossedBoundary) _dropCount++;
            _rb.useGravity = true;
        }

        private void CheckBoundary(int collisionObjectLayerMask)
        {
            if ((boundaryLayerMask & collisionObjectLayerMask) == 0) return;
            _crossedBoundary = true;
        }

        private void CheckDestruction(int collisionObjectLayerMask)
        {
            if ((ballDestroyerLayerMask & collisionObjectLayerMask) == 0) return;
            Pool.Free(this);
        }

        private void Log(object obj) => Debug.Log("[Ball] " + obj);
    }
}