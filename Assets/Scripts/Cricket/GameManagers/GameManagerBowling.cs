using Cricket.Balls;
using Cricket.Behaviour;
using Cricket.UI;
using UnityEngine;
using Utils;

namespace Cricket.GameManagers
{
    public class GameManagerBowling : GameManager
    {
        [SerializeField] private Transform target;
        [SerializeField] private PowerMeter powerMeter;
        [SerializeField] private GameObject shootButton;
        [SerializeField] private GameObject setButton;
        [SerializeField] private Indicators indicators;
        [SerializeField] private Bowler bowler;
        [SerializeField] private AIBatsman batsman;
        [SerializeField] private CanvasDragHandler dragHandler;
        [SerializeField] private Wickets wickets;

        private int _successfulDeliveryCount;

        private void Awake() => Reset();

        public void SetTarget()
        {
            setButton.SetActive(false);
            shootButton.SetActive(true);
            powerMeter.gameObject.SetActive(true);

            powerMeter.Reset();
            dragHandler.IsEnabled = false;
        }

        public void SetPower()
        {
            var isPowerShot = powerMeter.IsPowerShot();
            if (!isPowerShot)
            {
                ShowToast("Not enough power. try again");
                powerMeter.Reset();
                return;
            }

            powerMeter.gameObject.SetActive(false);
            shootButton.SetActive(false);

            bowler.Bowl(target.position, 30);
            _successfulDeliveryCount++;
            indicators.SetActiveCount(_successfulDeliveryCount);
            batsman.Swing();
            DelayedRunner.Instance.RunWithDelay(3, Reset);

            if (_successfulDeliveryCount != indicators.TotalIndicators) return;
            ShowToast("Great!");
            sceneLoader.LoadNextScene();
        }

        public void OnBallDestroyed(Ball ball) => Reset();

        private void Reset()
        {
            target.gameObject.SetActive(true);
            powerMeter.gameObject.SetActive(false);
            shootButton.SetActive(false);
            setButton.SetActive(true);
            dragHandler.IsEnabled = true;
            wickets.Reset();
        }
    }
}