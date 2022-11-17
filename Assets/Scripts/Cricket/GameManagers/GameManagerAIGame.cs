using System;
using System.Collections;
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
        [SerializeField] private InningsManager inningsManager;

        [SerializeField] private ObjectFollower camFollow;
        [SerializeField] private List<Fielder> fielders;

        private int _selfScore, _oppScore;

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
                    case GameState.InningsChange:
                        OnInningsComplete();
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
            if (isBattingSide) DelayedRunner.Instance.RunWithDelay(0.5f, () => ChangeState(GameState.SettingPower));
        }

        private void OnSettingPower()
        {
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
            foreach (var fielder in fielders) fielder.ChaseBall(_currentBall);
            camFollow.Follow(_currentBall.transform);
        }

        private void OnInningsComplete()
        {
            IsBattingSide = !IsBattingSide;
            if (!inningsManager.IsFirstInnings) return;
            inningsManager.StartNextInning();
            DelayedRunner.Instance.RunWithDelay(2, () => ChangeState(GameState.Idle));
        }

        public void OnBallDestroyed(Ball ball)
        {
            if (fsm.State is not (GameState.BallHit or GameState.BallThrown)) return;
            OnBallFlowComplete();
            StartCoroutine(ProcessBall(ball));
        }

        private IEnumerator ProcessBall(Ball ball)
        {
            yield return new WaitForEndOfFrame();
            if (!ball.HitByBat) yield break;
            if (ball.CrossedBoundary) AddScore(ball.WasDropped ? 4 : 6);
            ball.Free();
        }

        private void AddScore(int count)
        {
            if (IsBattingSide) _selfScore += count;
            else _oppScore += count;

            uiManager.AddScore(count, IsBattingSide ? _selfScore : _oppScore);
        }

        public void OnAimSet() => ChangeState(GameState.SettingPower);
        public void OnPowerSet() => ChangeState(GameState.BallThrown);

        public void OnBallCaught(float elapsed)
        {
            if (_currentBall.WasDropped) AddScore(1 + (int)(elapsed / 5));
            else Out();
            _currentBall.Free();
            OnBallFlowComplete();
        }

        public void Out()
        {
            inningsManager.Out();
            uiManager.Out();

            _currentBall.Free();
            DelayedRunner.Instance.RunWithDelay(2, OnBallFlowComplete);
        }

        private void OnBallFlowComplete()
        {
            ChangeState(inningsManager.IsInningsOver ? GameState.InningsChange : GameState.Idle);
            camFollow.Reset();
        }
    }
}