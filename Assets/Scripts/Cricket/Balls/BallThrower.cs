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
            var dir = target - _transform.position;
            dir = dir.normalized;
            var force = dir * power;
            ball.Rigidbody.AddForce(force, ForceMode.Impulse);
        }

        private Ball GetNewBall()
        {
            var spawnPos = _transform.position;
            var ball = _ballPool.Fetch();
            ball.transform.position = spawnPos;
            ball.gameObject.SetActive(true);
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