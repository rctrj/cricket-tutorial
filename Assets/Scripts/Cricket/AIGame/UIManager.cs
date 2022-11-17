using System;
using TMPro;
using UnityEngine;
using Utils;

namespace Cricket.AIGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Fsm fsm;

        [SerializeField] private GameObject hitButton;
        [SerializeField] private GameObject setAimButton;
        [SerializeField] private GameObject shootButton;
        [SerializeField] private GameObject aimObject;
        [SerializeField] private GameObject powerMeter;

        [SerializeField] private CanvasDragHandler dragHandler;
        [SerializeField] private bool isBattingSide;

        [SerializeField] private InningsManager inningsManager;

        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text targetText;
        [SerializeField] private GameObject inningsChangeGameObject;
        [SerializeField] private TMP_Text inningsTargetText;
        [SerializeField] private TMP_Text toast;

        [SerializeField] private TMP_Text winnerText;
        [SerializeField] private GameObject gameOverScreen;

        private int _score;
        private int _target;

        private bool _gameOver;

        public bool IsBattingSide
        {
            get => isBattingSide;
            set => isBattingSide = value;
        }

        private void Start()
        {
            fsm.OnStateChange += (_, newState) =>
            {
                switch (newState)
                {
                    case GameState.Idle:
                        OnIdle();
                        break;
                    case GameState.SettingAim:
                        OnSettingAim();
                        break;
                    case GameState.SettingPower:
                        OnSettingPower();
                        break;
                    case GameState.BallThrown:
                        OnBallThrown();
                        break;
                    case GameState.BallHit:
                        OnBallHit();
                        break;
                    case GameState.InningsChange:
                        OnInningsChange();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
                }
            };

            OnIdle();
        }

        private void OnIdle()
        {
            hitButton.SetActive(false);
            setAimButton.SetActive(false);
            shootButton.SetActive(false);
            aimObject.SetActive(false);
            powerMeter.SetActive(false);
            dragHandler.IsEnabled = false;
            inningsChangeGameObject.SetActive(false);

            UpdateScore();
        }

        private void OnSettingAim()
        {
            hitButton.SetActive(false);
            setAimButton.SetActive(!IsBattingSide);
            shootButton.SetActive(false);
            aimObject.SetActive(!IsBattingSide);
            powerMeter.SetActive(false);
            dragHandler.IsEnabled = !IsBattingSide;
            inningsChangeGameObject.SetActive(false);
        }

        private void OnSettingPower()
        {
            hitButton.SetActive(IsBattingSide);
            setAimButton.SetActive(false);
            shootButton.SetActive(!IsBattingSide);
            aimObject.SetActive(!IsBattingSide);
            powerMeter.SetActive(!IsBattingSide);
            dragHandler.IsEnabled = false;
            inningsChangeGameObject.SetActive(false);
        }

        private void OnBallThrown()
        {
            hitButton.SetActive(IsBattingSide);
            setAimButton.SetActive(false);
            shootButton.SetActive(false);
            aimObject.SetActive(false);
            powerMeter.SetActive(false);
            dragHandler.IsEnabled = false;
            inningsChangeGameObject.SetActive(false);
        }

        private void OnBallHit()
        {
            hitButton.SetActive(false);
            setAimButton.SetActive(false);
            shootButton.SetActive(false);
            aimObject.SetActive(false);
            powerMeter.SetActive(false);
            dragHandler.IsEnabled = false;
            inningsChangeGameObject.SetActive(false);
        }

        private void OnInningsChange()
        {
            hitButton.SetActive(false);
            setAimButton.SetActive(false);
            shootButton.SetActive(false);
            aimObject.SetActive(false);
            powerMeter.SetActive(false);
            dragHandler.IsEnabled = false;
            inningsChangeGameObject.SetActive(!_gameOver);
            targetText.text = "Target: " + _score;
            inningsTargetText.text = "Target: " + _score;
            _target = _score;
            _score = 0;
        }

        public void AddScore(int count, int finalCount)
        {
            ShowToast(count.ToString());
            _score = finalCount;
            UpdateScore();
        }

        private void UpdateScore() => scoreText.text = "Current: " + _score + "(" + NumOvers() + ")";

        private string NumOvers()
        {
            if (inningsManager.NumBalls < 6) return "0." + inningsManager.NumBalls + "/1.0";
            return "1.0/1.0";
        }

        public void Out() => ShowToast("Out!");

        protected void ShowToast(string message)
        {
            toast.gameObject.SetActive(false);
            toast.text = message;
            toast.gameObject.SetActive(true);
        }

        public void GameOver()
        {
            string txt;
            if (_score < _target) txt = "You win!";
            else if (_score > _target) txt = "You lose!";
            else txt = "Draw!";

            inningsChangeGameObject.SetActive(false);
            winnerText.SetText(txt);
            gameOverScreen.SetActive(true);
            _gameOver = true;
        }
    }
}