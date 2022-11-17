using System;
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
        }

        private void OnSettingAim()
        {
            hitButton.SetActive(false);
            setAimButton.SetActive(!IsBattingSide);
            shootButton.SetActive(false);
            aimObject.SetActive(!IsBattingSide);
            powerMeter.SetActive(false);
            dragHandler.IsEnabled = !IsBattingSide;
        }

        private void OnSettingPower()
        {
            hitButton.SetActive(IsBattingSide);
            setAimButton.SetActive(false);
            shootButton.SetActive(!IsBattingSide);
            aimObject.SetActive(!IsBattingSide);
            powerMeter.SetActive(!IsBattingSide);
            dragHandler.IsEnabled = false;
        }

        private void OnBallThrown()
        {
            hitButton.SetActive(IsBattingSide);
            setAimButton.SetActive(false);
            shootButton.SetActive(false);
            aimObject.SetActive(false);
            powerMeter.SetActive(false);
            dragHandler.IsEnabled = false;
        }

        private void OnBallHit()
        {
            hitButton.SetActive(false);
            setAimButton.SetActive(false);
            shootButton.SetActive(false);
            aimObject.SetActive(false);
            powerMeter.SetActive(false);
            dragHandler.IsEnabled = false;
        }

        private void OnInningsChange()
        {
        }

        public void AddScore(int count, int finalCount)
        {
            Debug.Log("adding score: " + count + " total : " + finalCount);
        }

        public void Out()
        {
        }
    }
}