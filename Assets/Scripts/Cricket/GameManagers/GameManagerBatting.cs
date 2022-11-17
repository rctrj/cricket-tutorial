using System.Collections;
using Cricket.Balls;
using Cricket.Behaviour;
using Cricket.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Cricket.GameManagers
{
    public class GameManagerBatting : MonoBehaviour
    {
        [SerializeField] private AIBowler bowler;
        [SerializeField] private Batsman batsman;

        [SerializeField] private Indicators indicators;
        [SerializeField] private TMP_Text toast;

        [SerializeField] private string nextSceneName;

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

            DelayedRunner.Instance.RunWithDelay(2, () => SceneManager.LoadScene(nextSceneName));
        }

        public void OnBallDestroyed(Ball ball)
        {
            if (!ball.HitByBat) ShowToast("Try again!");
            StartCoroutine(Bowl(2));
        }

        private void ShowToast(string message)
        {
            toast.gameObject.SetActive(false);
            toast.text = message;
            toast.gameObject.SetActive(true);
        }
    }
}