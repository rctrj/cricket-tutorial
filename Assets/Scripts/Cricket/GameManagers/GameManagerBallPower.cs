using Cricket.Behaviour;
using Cricket.UI;
using Scene;
using TMPro;
using UnityEngine;

namespace Cricket.GameManagers
{
    public class GameManagerBallPower : MonoBehaviour
    {
        [SerializeField] private DelayedSceneLoader sceneLoader;
        [SerializeField] private PowerMeter powerMeter;
        [SerializeField] private TMP_Text toast;
        [SerializeField] private Bowler bowler;
        [SerializeField] private Transform target;

        public void OnStop()
        {
            var isPowerSufficient = powerMeter.IsPowerShot();
            if (isPowerSufficient)
            {
                ShowToast("Great!");
                sceneLoader.LoadNextScene();
                bowler.Bowl(target.position, 20);
                return;
            }

            ShowToast("Not enough power, try again");
            powerMeter.Reset();
        }

        private void ShowToast(string message)
        {
            toast.gameObject.SetActive(false);
            toast.text = message;
            toast.gameObject.SetActive(true);
        }
    }
}