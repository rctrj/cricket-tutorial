using Utils;
using UnityEngine;

namespace Cricket.Balls
{
    public class BallThrower : MonoBehaviour
    {
        [SerializeField] private Ball ballPrefab;

        private Pool<Ball> _ballPool;
        private Transform _transform;

        private void Awake()
        {
            _ballPool = new Pool<Ball>(CreateNewBall);
            _transform = transform;
        }

        public void ThrowBall(Vector3 target, float power)
        {
            var ball = GetNewBall();
            ThrowBall(ball, target, power);
        }

        public void ThrowBall(Ball ball, Vector3 target, float power)
        {
            ball.gameObject.SetActive(true);
            var dir = target - _transform.position;
            dir = dir.normalized;
            var force = dir * power;
            ball.Rigidbody.AddForce(force, ForceMode.Impulse);
        }

        public Ball GetNewBall()
        {
            var spawnPos = _transform.position;
            var ball = _ballPool.Fetch();
            ball.gameObject.SetActive(false);
            ball.transform.position = spawnPos;
            return ball;
        }

        private Ball CreateNewBall()
        {
            var ball = Instantiate(ballPrefab);
            ball.Pool = _ballPool;
            return ball;
        }
    }
}