using System.Collections;
using Cricket.Balls;
using Cricket.Behaviour;
using Cricket.UI;
using Scene;
using TMPro;
using UnityEngine;

namespace Cricket.GameManagers
{
    public class GameManagerBatting : GameManager
    {
        [SerializeField] private AIBowler bowler;
        [SerializeField] private Batsman batsman;

        [SerializeField] private Indicators indicators;

        private int _successfulShotsCount;

        private void Start()
        {
            batsman.OnBallHit += OnBallHit;
            StartCoroutine(Bowl(2));
        }

        private IEnumerator Bowl(float delay)
        {
            yield return new WaitForSeconds(delay);

            bowler.WaitForRunUp();
            yield return new WaitForEndOfFrame();

            var ball = bowler.Bowl();
            batsman.CurrentBall = ball;
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
        }
    }
}