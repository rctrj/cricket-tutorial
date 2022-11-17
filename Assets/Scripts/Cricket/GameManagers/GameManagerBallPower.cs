using Cricket.Behaviour;
using Cricket.UI;
using UnityEngine;

namespace Cricket.GameManagers
{
    public class GameManagerBallPower : GameManager
    {
        [SerializeField] private PowerMeter powerMeter;
        [SerializeField] private Bowler bowler;
        [SerializeField] private Transform target;

        public void OnStop()
        {
            var isPowerSufficient = powerMeter.IsPowerShot();
            if (isPowerSufficient)
            {
                ShowToast("Great!");
                sceneLoader.LoadNextScene();
                bowler.Bowl(target.position, 30);
                return;
            }

            ShowToast("Not enough power, try again");
            powerMeter.Reset();
        }
    }
}