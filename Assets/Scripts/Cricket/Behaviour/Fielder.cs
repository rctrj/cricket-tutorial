using Cricket.AIGame;
using Cricket.Balls;
using Cricket.GameManagers;
using UnityEngine;

namespace Cricket.Behaviour
{
    public class Fielder : MonoBehaviour
    {
        [SerializeField] private Fsm fsm;
        [SerializeField] private GameManagerAIGame gameManager;
        [SerializeField] private Ball currentBall;
        [SerializeField] private float speed;
        [SerializeField] private LayerMask ballLayerMask;

        private Transform _transform;
        private Vector3 _initPos;

        private bool _isChasing = false;
        private float _elapsed = 0f;

        private void Awake()
        {
            _transform = transform;
            _initPos = _transform.position;
        }

        public void ChaseBall(Ball ball)
        {
            Debug.Log("chase ball called");
            currentBall = ball;
            _isChasing = true;
        }

        public void Reset()
        {
            _elapsed = 0f;
            _isChasing = false;
            _transform.position = _initPos;
        }

        private void Update()
        {
            if (fsm.State != GameState.BallHit) _isChasing = false;
            if (!_isChasing) return;

            var dt = Time.deltaTime;
            _elapsed += dt;

            var ballPos = currentBall.transform.position;
            var ballPos2D = new Vector2(ballPos.x, ballPos.z);

            var currPos = _transform.position;
            var currPos2D = new Vector2(currPos.x, currPos.z);

            var dir = ballPos2D - currPos2D;
            _transform.position = currPos + new Vector3(dir.x, 0f, dir.y).normalized * (speed * dt);
        }

        private void OnTriggerEnter(Collider collision)
        {
            var collisionObjectLayerMask = 1 << collision.gameObject.layer;
            if ((ballLayerMask & collisionObjectLayerMask) == 0) return;
            gameManager.OnBallCaught(_elapsed);
        }
    }
}