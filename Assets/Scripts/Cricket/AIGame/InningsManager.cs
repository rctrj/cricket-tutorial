using System;
using UnityEngine;

namespace Cricket.AIGame
{
    public class InningsManager : MonoBehaviour
    {
        [SerializeField] private Fsm fsm;

        public bool IsFirstInnings { get; private set; } = true;
        public int NumBalls { get; private set; }

        private bool _out;

        public bool IsInningsOver => NumBalls == 6 || _out;

        private void Start()
        {
            fsm.OnStateChange += (_, newState) =>
            {
                if (newState == GameState.BallThrown) NumBalls++;
            };
        }

        public void StartNextInning()
        {
            if (!IsFirstInnings) throw new Exception("Cannot start new innings");

            NumBalls = 0;
            IsFirstInnings = false;
            _out = false;
        }

        public void Out() => _out = true;
    }
}