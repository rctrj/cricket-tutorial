using System;
using System.Collections.Generic;
using Cricket.AIGame;
using Cricket.Balls;
using Cricket.Behaviour;
using Cricket.UI;
using UnityEngine;
using Utils;

namespace Cricket.GameManagers
{
    public class GameManagerAIGame : MonoBehaviour
    {
        [SerializeField] private Fsm fsm;

        [SerializeField] private Batsman batsman;
        [SerializeField] private AIBatsman aiBatsman;
        [SerializeField] private Bowler bowler;
        [SerializeField] private AIBowler aiBowler;
        [SerializeField] private Wickets wickets;

        [SerializeField] private RectTransform target;

        [SerializeField] private bool isBattingSide;
        [SerializeField] private PowerMeter powerMeter;

        [SerializeField] private UIManager uiManager;

        [SerializeField] private List<Fielder> fielders;

        private Ball _currentBall;

        public void SwingBat()
        {
            if (!isBattingSide) return;
            batsman.CurrentBall = _currentBall;
            batsman.Swing();
        }

        private bool IsBattingSide
        {
            get => isBattingSide;
            set
            {
                isBattingSide = value;
                uiManager.IsBattingSide = isBattingSide;
            }
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
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
                }
            };

            uiManager.IsBattingSide = isBattingSide;
            OnIdle();

            batsman.OnBallHit += () => ChangeState(GameState.BallHit);
            aiBatsman.OnBallHit += () => ChangeState(GameState.BallHit);
        }

        private void ChangeState(GameState newState) => fsm.State = newState;

        private void OnIdle()
        {
            foreach (var fielder in fielders) fielder.Reset();
            if (IsBattingSide) aiBowler.WaitForRunUp();
            else bowler.WaitForRunUp();
            wickets.Reset();

            DelayedRunner.Instance.RunWithDelay(2f, () => ChangeState(GameState.SettingAim));
        }

        private void OnSettingAim()
        {
            Debug.Log("Setting Aim");
            if (isBattingSide) DelayedRunner.Instance.RunWithDelay(0.5f, () => ChangeState(GameState.SettingPower));
        }

        private void OnSettingPower()
        {
            Debug.Log("Setting Power");
            if (isBattingSide) DelayedRunner.Instance.RunWithDelay(0.5f, () => ChangeState(GameState.BallThrown));
        }

        private void OnBallThrown()
        {
            if (IsBattingSide)
            {
                _currentBall = aiBowler.Bowl();
                return;
            }

            var targetPoint = target.position;
            _currentBall = bowler.Bowl(targetPoint, powerMeter.IsPowerShot() ? 30 : 20);

            aiBatsman.CurrentBall = _currentBall;
            aiBatsman.Swing();
        }

        private void OnBallHit()
        {
            Debug.Log("On Ball HIt. fielder count: " + fielders.Count);
            foreach (var fielder in fielders) fielder.ChaseBall(_currentBall);
        }

        public void OnBallDestroyed(Ball ball)
        {
            ChangeState(GameState.Idle);

            //do scoring
        }

        public void OnAimSet() => ChangeState(GameState.SettingPower);
        public void OnPowerSet() => ChangeState(GameState.BallThrown);

        public void OnBallCaught(float elapsed)
        {
            //todo
            ChangeState(GameState.Idle);
        }
    }
}