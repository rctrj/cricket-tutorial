using System.Collections;
using Cricket.Balls;
using Cricket.Behaviour;
using Cricket.UI;
using UnityEngine;

namespace Cricket.GameManagers
{
    public class GameManagerBatting : GameManager
    {
        [SerializeField] private AIBowler bowler;
        [SerializeField] private Batsman batsman;

        [SerializeField] private Indicators indicators;

        private int _successfulShotsCount;
        private Coroutine _ballDestructionCoroutine;

        private void Start()
        {
            batsman.OnBallHit += OnBallHit;
            StartCoroutine(Bowl(2));
        }

        private IEnumerator Bowl(float delay)
        {
            yield return new WaitForSeconds(delay);

            bowler.WaitForRunUp();
            yield return new WaitForSeconds(0.1f);

            var ball = bowler.Bowl();
            batsman.CurrentBall = ball;

            _ballDestructionCoroutine = StartCoroutine(DestroyBallAfterTimeout(5, ball));
        }

        private IEnumerator DestroyBallAfterTimeout(float timeout, Ball ball)
        {
            yield return new WaitForSeconds(timeout);
            OnBallDestroyed(ball);
            Destroy(ball);
        }

        private void OnBallHit()
        {
            _successfulShotsCount++;
            indicators.SetActiveCount(_successfulShotsCount);

            if (_successfulShotsCount != indicators.TotalIndicators) return;
            ShowToast("Great!");

            sceneLoader.LoadNextScene();
        }

        public void OnBallDestroyed(Ball ball)
        {
            if (!ball.HitByBat) ShowToast("Try again!");
            StartCoroutine(Bowl(2));
            StopCoroutine(_ballDestructionCoroutine);
            _ballDestructionCoroutine = null;
        }
    }
}